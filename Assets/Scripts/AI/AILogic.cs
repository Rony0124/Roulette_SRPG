using System.Collections.Generic;
using System.Threading;
using HF.GamePlay;
using HF.Utils;
using InGame;
using TSoft.InGame.CardSystem;
using UnityEngine;
using UnityEngine.Profiling;

namespace HF.AI
{
    public class AILogic
    {
        public int AIDepth = 3;                //How many turns in advance does it check, higher number takes exponentially longer
        public int AIDepthWide = 1;           //For these first few turns, will consider more options, slow!
        public int ActionsPerTurn = 2;          //AI wont predict more than this number of sequential actions per turn, if more than that will EndTurn (Do A, then do B, then do C, then end turn)
        public int ActionsPerTurnWide = 3;     //Same but in wide depth
        public int NodesPerAction = 4;         //For a turn action (1st, 2nd, or 3rd...), cannot evaluate more than this number of child nodes, if more, will only process the AIActions with with best score
        public int NodesPerActionWide = 7;    //Same but in wide depth
        
        public int aiPlayerId;
        
        private GameLogic gameLogic; 
        private Game originalData; 
        private AIHeuristic heuristic;
        private Thread aiThread;
        
        private System.Random randomGen;
        
        private NodeState firstNode;
        private NodeState bestMove;
        
        private bool running;
        private int calcNum;
        private int reachedDepth;
        
        private Pool<NodeState> nodePool = new ();
        private Pool<Game> dataPool = new ();
        private Pool<AIAction> actionPool = new ();
        private Pool<List<AIAction>> listPool = new ();
        
        public static AILogic Create(int playerId)
        {
            var job = new AILogic
            {
                aiPlayerId = playerId
            };

            job.heuristic = new AIHeuristic(playerId);
            job.gameLogic = new GameLogic(true); //Skip all delays for the AI calculations

            return job;
        }
        
        public void RunAI(Game data)
        {
            if (running)
                return;
            
            Debug.Log("AI Player 1 - AI Run");

            originalData = Game.CloneNew(data);        //Clone game data to keep original data unaffected
            gameLogic.ClearResolve();                 //Clear temp memory
            gameLogic.SetData(originalData);          //Assign data to game logic
            randomGen = new System.Random();       //Reset random seed

            firstNode = null;
            reachedDepth = 0;
            calcNum = 0;
            running = true;

            //Uncomment these lines to run on separate thread (and comment Execute()), better for production so it doesn't freeze the UI while calculating the AI
            /*aiThread = new Thread(Execute);
            aiThread.Start();*/

            //Uncomment this line to run on main thread (and comment the thread one), better for debuging since you will be able to use breakpoints, profiler and Debug.Log
            Execute();
        }
        
        public void Stop()
        {
            running = false;
            if (aiThread != null && aiThread.IsAlive)
                aiThread.Abort();
        }
        
        private void Execute()
        {
            Debug.Log("AI Player 2 - AI Execute");
            //Create first node
            firstNode = CreateNode(null, null, aiPlayerId, 0, 0);
            firstNode.hvalue = heuristic.CalculateHeuristic(originalData, firstNode);
            firstNode.alpha = int.MinValue;
            firstNode.beta = int.MaxValue;

            Profiler.BeginSample("AI");
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

            //Calculate first node
            CalculateNode(originalData, firstNode);

            Debug.Log("AI: Time " + watch.ElapsedMilliseconds + "ms Depth " + reachedDepth + " Nodes " + calcNum);
            Profiler.EndSample();

            //Save best move
            bestMove = firstNode.best_child;
            running = false;
        }
        
          //Add list of all possible orders and search in all of them
        private void CalculateNode(Game data, NodeState node)
        {
            Debug.Log("AI Player 3 - 노드 계산");
            Profiler.BeginSample("Add Actions");
            Player player = data.GetPlayer(data.current_player);
            List<AIAction> action_list = listPool.Create();

            int max_actions = node.tdepth < AIDepthWide ? ActionsPerTurnWide : ActionsPerTurn;
            if (node.taction < max_actions)
            {
                if (data.selector == SelectorType.None)
                {
                    //Play card
                    for (int c = 0; c < player.cards_hand.Count; c++)
                    {
                        Card card = player.cards_hand[c];
                        if (card.Data.type == CardType.Joker)
                        {
                            Debug.Log("AI Player 4 - 조커 액션 추가");
                            AddJokerActions(action_list, data, node, GameAction.PlayJoker, card);    
                        }
                    }

                    CardPattern maxPattern = null;
                    int maxSum = 0;
                    for (var i = player.cards_pattern.Count - 1; i >= 0; --i)
                    {
                        int tempCardSum = 0;
                        for (var j = 0; j < player.cards_pattern[i].cards.Count; j++)
                        {
                            tempCardSum += player.cards_pattern[i].cards[j].Data.number;
                        }

                        if (maxSum < tempCardSum)
                        {
                            maxPattern = player.cards_pattern[i];
                        }
                    }

                    string t = maxPattern == null ? "null" : maxPattern.patternType.ToString();
                    Debug.Log($"AI Player 4 - 패턴 액션 추가 {t}");
                    AddPatternActions(action_list, data, node, GameAction.PlayPattern, maxPattern);

                    //Action on board
                    /*for (int c = 0; c < player.cards_board.Count; c++)
                    {
                        Card card = player.cards_board[c];
                        AddActions(action_list, data, node, GameAction.Attack, card);
                        AddActions(action_list, data, node, GameAction.AttackPlayer, card);
                        AddActions(action_list, data, node, GameAction.CastAbility, card);
                        //AddActions(action_list, data, node, GameAction.Move, card);        //Uncomment to consider move actions
                    }*/
                }
                else
                {
                   // AddSelectActions(action_list, data, node);
                }
            }

            //End Turn (dont add action if ai can still attack player, or ai hasnt spent any mana)
            //bool is_full_mana = HasAction(action_list, GameAction.PlayCard) && player.mana >= player.mana_max;
            bool can_attack = HasAction(action_list, GameAction.PlayPattern);
            bool can_end = !can_attack && data.selector == SelectorType.None;
            
            if (action_list.Count == 0 || can_end)
            {
                Debug.Log($"AI Player 4 - 패턴 end turn 추가");
                AIAction action = CreateAction(GameAction.EndTurn);
                action_list.Add(action);
            }

            //Remove actions with low score
            FilterActions(data, node, action_list);
            Profiler.EndSample();

            //Execute valid action and search child node
            for (int o = 0; o < action_list.Count; o++)
            {
                AIAction action = action_list[o];
                if (action.valid && node.alpha < node.beta)
                {
                    CalculateChildNode(data, node, action);
                }
            }

            action_list.Clear();
            listPool.Dispose(action_list);
        }
        
        //Mark valid/invalid on each action, if too many actions, will keep only the ones with best score
        private void FilterActions(Game data, NodeState node, List<AIAction> actionList)
        {
            int countValid = 0;
            for (int o = 0; o < actionList.Count; o++)
            {
                AIAction action = actionList[o];
                action.sort = heuristic.CalculateActionSort(data, action);
                action.valid = action.sort <= 0 || action.sort >= node.sort_min;
                if (action.valid)
                    countValid++;
            }

            int maxActions = node.tdepth < AIDepthWide ? NodesPerActionWide : NodesPerAction;
            int maxActionsSkip = maxActions + 2; //No need to calculate all scores if its just to remove 1-2 actions
            if (countValid <= maxActionsSkip)
                return; //No filtering needed

            //Calculate scores
            for (int o = 0; o < actionList.Count; o++)
            {
                AIAction action = actionList[o];
                if (action.valid)
                {
                    action.score = heuristic.CalculateActionScore(data, action);
                }
            }

            //Sort, and invalidate actions with low score
            actionList.Sort((a, b) => b.score.CompareTo(a.score));
            for (int o = 0; o < actionList.Count; o++)
            {
                AIAction action = actionList[o];
                action.valid = action.valid && o < maxActions;
            }
        }
        
        //Create a child node for parent, and calculate it
        private void CalculateChildNode(Game data, NodeState parent, AIAction action)
        {
            if (action.type == GameAction.None)
                return;

            int player_id = data.current_player;

            //Clone data so we can update it in a new node
            Profiler.BeginSample("Clone Data");
            Game ndata = dataPool.Create();
            Game.Clone(data, ndata); //Clone
            gameLogic.ClearResolve();
            gameLogic.SetData(ndata);
            Profiler.EndSample();

            //Execute move and update data
            Profiler.BeginSample("Execute AIAction");
            //DoAIAction(ndata, action, player_id);
            Profiler.EndSample();

            //Update depth
            bool new_turn = action.type == GameAction.EndTurn;
            int next_tdepth = parent.tdepth;
            int next_taction = parent.taction + 1;

            if (new_turn)
            {
                next_tdepth = parent.tdepth + 1;
                next_taction = 0;
            }

            //Create node
            Profiler.BeginSample("Create Node");
            NodeState child_node = CreateNode(parent, action, player_id, next_tdepth, next_taction);
            parent.childs.Add(child_node);
            Profiler.EndSample();

            //Set minimum sort for next AIActions, if new turn, reset to 0
            child_node.sort_min = new_turn ? 0 : Mathf.Max(action.sort, child_node.sort_min);

            //If win or reached max depth, stop searching deeper
            if (!ndata.HasEnded() && child_node.tdepth < AIDepth)
            {
                //Calculate child
                CalculateNode(ndata, child_node);
            }
            else
            {
                //End of tree, calculate full Heuristic
                child_node.hvalue = heuristic.CalculateHeuristic(ndata, child_node);
            }

            //Update parents hvalue, alpha, beta, and best child
            if (player_id == aiPlayerId)
            {
                //AI player
                if (parent.best_child == null || child_node.hvalue > parent.hvalue)
                {
                    parent.best_child = child_node;
                    parent.hvalue = child_node.hvalue;
                    parent.alpha = Mathf.Max(parent.alpha, parent.hvalue);
                }
            }
            else
            {
                //Opponent player
                if (parent.best_child == null || child_node.hvalue < parent.hvalue)
                {
                    parent.best_child = child_node;
                    parent.hvalue = child_node.hvalue;
                    parent.beta = Mathf.Min(parent.beta, parent.hvalue);
                }
            }

            //Just for debug, keep track of node/depth count
            calcNum++;
            if (child_node.tdepth > reachedDepth)
                reachedDepth = child_node.tdepth;

            //We are done with this game data, dispose it.
            //Dont dispose NodeState here (node_pool) since we want to retrieve the full tree path later
            dataPool.Dispose(ndata);
        }
        
        private AIAction CreateAction(ushort type)
        {
            AIAction action = actionPool.Create();
            action.Clear();
            action.type = type;
            action.valid = true;
            return action;
        }
        
        private AIAction CreateAction(ushort type, Card card)
        {
            AIAction action = actionPool.Create();
            action.Clear();
            action.type = type;
            action.card_uid = card.uid;
            action.valid = true;
            return action;
        }

        private AIAction CreateAction(ushort type, CardPattern pattern)
        {
            AIAction action = actionPool.Create();
            action.Clear();
            action.type = type;
            action.pattern = pattern;
            action.valid = true;
            return action;
        }
        
        //Simulate AI action
        private void DoAIAction(Game data, AIAction action, int player_id)
        {
            Player player = data.GetPlayer(player_id);

            if (action.type == GameAction.PlayJoker)
            {
                Debug.Log("Do Joker Action!!");
                Card card = player.GetHandCard(action.card_uid);
                gameLogic.PlayCard(card);
            }
            
            if (action.type == GameAction.PlayPattern)
            {
                player.currentPattern = action.pattern; 
                var oplayer = data.GetOpponentPlayer(player_id); 
                /*Card card = player.GetBoardCard(action.card_uid);
                Card target = data.GetBoardCard(action.target_uid);*/
                gameLogic.AttackTarget(player, oplayer);
            }

            if (action.type == GameAction.EndTurn)
            {
                gameLogic.EndTurn();
            }
        }
        
        private void AddJokerActions(List<AIAction> actions, Game data, NodeState node, ushort type, Card card)
        {
            Player player = data.GetPlayer(data.current_player);

            if (data.selector != SelectorType.None)
                return;
            
            if (type == GameAction.PlayJoker)
            {
                Debug.Log("Add Joker Action!!");
                AIAction action = CreateAction(type, card);
               // action.ability_id = ability.id;
                actions.Add(action);
                /*List<AbilityData> abilities = card.GetAbilities();
                for (int a = 0; a < abilities.Count; a++)
                {
                    AbilityData ability = abilities[a];
                    if (ability.trigger == AbilityTrigger.Activate && data.CanCastAbility(card, ability) && ability.HasValidSelectTarget(data, card))
                    {
                        AIAction action = CreateAction(type, card);
                        action.ability_id = ability.id;
                        actions.Add(action);
                    }
                }*/
            }
        }
        
        private void AddPatternActions(List<AIAction> actions, Game data, NodeState node, ushort type, CardPattern pattern)
        {
           //TODO action별 삽입 정의
           Player player = data.GetPlayer(data.current_player);

           if (data.selector != SelectorType.None)
               return;

           if (pattern == null)
               return;

           if (type == GameAction.PlayPattern)
           {
               AIAction action = CreateAction(type, pattern);
               actions.Add(action);
           }
        }
        
        private bool HasAction(List<AIAction> list, ushort type)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].type == type)
                    return true;
            }
            return false;
        }

        
        private NodeState CreateNode(NodeState parent, AIAction action, int player_id, int turn_depth, int turn_action)
        {
            NodeState node = nodePool.Create();
            node.current_player = player_id;
            node.tdepth = turn_depth;
            node.taction = turn_action;
            node.parent = parent;
            node.last_action = action;
            node.alpha = parent != null ? parent.alpha : int.MinValue;
            node.beta = parent != null ? parent.beta : int.MaxValue;
            node.hvalue = 0;
            node.sort_min = 0;
            return node;
        }
        
        public AIAction GetBestAction()
        {
            return bestMove?.last_action;
        }
        
        public bool IsRunning()
        {
            return running;
        }
        
        public string GetNodePath()
        {
            return GetNodePath(firstNode);
        }

        private string GetNodePath(NodeState node)
        {
            var path = "Prediction: HValue: " + node.hvalue + "\n";
            var current = node;

            while (current != null)
            {
                var move = current.last_action;
                if (move != null)
                    path += "Player " + current.current_player + ": " + move.GetText(originalData) + "\n";
                current = current.best_child;
            }
            return path;
        }
        
        public void ClearMemory()
        {
            System.GC.Collect(); //Free memory from AI
        }
    }
}
