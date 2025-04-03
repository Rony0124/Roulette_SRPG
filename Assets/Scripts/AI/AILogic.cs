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

            originalData = Game.CloneNew(data);        //Clone game data to keep original data unaffected
            gameLogic.ClearResolve();                 //Clear temp memory
            gameLogic.SetData(originalData);          //Assign data to game logic
            randomGen = new System.Random();       //Reset random seed

            firstNode = null;
            reachedDepth = 0;
            calcNum = 0;
            running = true;

            //Uncomment these lines to run on separate thread (and comment Execute()), better for production so it doesn't freeze the UI while calculating the AI
            aiThread = new Thread(Execute);
            aiThread.Start();

            //Uncomment this line to run on main thread (and comment the thread one), better for debuging since you will be able to use breakpoints, profiler and Debug.Log
            //Execute();
        }
        
        public void Stop()
        {
            running = false;
            if (aiThread != null && aiThread.IsAlive)
                aiThread.Abort();
        }
        
        private void Execute()
        {
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
                        AddActions(action_list, data, node, GameAction.PlayCard, card);
                    }

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
            /*bool is_full_mana = HasAction(action_list, GameAction.PlayCard) && player.mana >= player.mana_max;
            bool can_attack_player = HasAction(action_list, GameAction.AttackPlayer);
            bool can_end = !can_attack_player && !is_full_mana && data.selector == SelectorType.None;*/
            
            if (action_list.Count == 0)
            {
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
            /*if (action.type == GameAction.None)
                return;*/

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
            DoAIAction(ndata, action, player_id);
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
        
        //Simulate AI action
        private void DoAIAction(Game data, AIAction action, int player_id)
        {
            Player player = data.GetPlayer(player_id);

            /*if (action.type == GameAction.PlayCard)
            {
                Card card = player.GetHandCard(action.card_uid);
                game_logic.PlayCard(card, action.slot);
            }

            if (action.type == GameAction.Move)
            {
                Card card = player.GetBoardCard(action.card_uid);
                game_logic.MoveCard(card, action.slot);
            }

            if (action.type == GameAction.Attack)
            {
                Card card = player.GetBoardCard(action.card_uid);
                Card target = data.GetBoardCard(action.target_uid);
                game_logic.AttackTarget(card, target);
            }

            if (action.type == GameAction.AttackPlayer)
            {
                Card card = player.GetBoardCard(action.card_uid);
                Player tplayer = data.GetPlayer(action.target_player_id);
                game_logic.AttackPlayer(card, tplayer);
            }

            if (action.type == GameAction.CastAbility)
            {
                Card card = player.GetCard(action.card_uid);
                AbilityData ability = AbilityData.Get(action.ability_id);
                game_logic.CastAbility(card, ability);
            }

            if (action.type == GameAction.SelectCard)
            {
                Card target = data.GetCard(action.target_uid);
                game_logic.SelectCard(target);
            }

            if (action.type == GameAction.SelectPlayer)
            {
                Player target = data.GetPlayer(action.target_player_id);
                game_logic.SelectPlayer(target);
            }

            if (action.type == GameAction.SelectSlot)
            {
                game_logic.SelectSlot(action.slot);
            }

            if (action.type == GameAction.SelectChoice)
            {
                game_logic.SelectChoice(action.value);
            }

            if (action.type == GameAction.CancelSelect)
            {
                game_logic.CancelSelection();
            }*/

            if (action.type == GameAction.EndTurn)
            {
                gameLogic.EndTurn();
            }
        }
        
        private void AddActions(List<AIAction> actions, Game data, NodeState node, ushort type, Card card)
        {
           //TODO action별 삽입 정의
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

        public static void CalculateCardPatterns(List<Card> targetCards)
        {
            if (targetCards == null || targetCards.Count == 0)
                return;
            
            var grade = new bool[10];
            var gradeNumberCombined = new int[10];
            
            grade[(int)CardPatternType.None] = true;
            grade[(int)CardPatternType.HighCard] = true;
            
            int[] suits = new int[5];
            int[] numbers = new int[15];
            int[] suitsNumberCombined = new int[5]; 
            
            for(int i=0; i< targetCards.Count; i++) {
                Card card = targetCards[i];
                // 카드 숫자에 따른 count
                numbers[card.Data.number]++;
                // Ace의 경우 
                if(card.Data.number == 1)
                    numbers[14]++;
                
                // 카드 모양에 따른 count
                switch (card.Data.type) {
                    case CardType.Spade:
                        suits[1]++;
                        suitsNumberCombined[1] += card.Data.number; 
                        break;
                    case CardType.Diamond:
                        suits[2]++;
                        suitsNumberCombined[2] += card.Data.number;
                        break;
                    case CardType.Club:
                        suits[3]++;
                        suitsNumberCombined[3] += card.Data.number;
                        break;
                    case CardType.Heart:
                        suits[4]++;
                        suitsNumberCombined[4] += card.Data.number;
                        break;
                }
            }

            int biggestNumber = 0;
            for (int i = 14; i >= 0; i--)
            {
                if (numbers[i] > 0)
                {
                    biggestNumber = i;
                    break;
                }
            }

            gradeNumberCombined[(int)CardPatternType.HighCard] = biggestNumber;
            
            bool isStraight = false;
            // Ace를 1, 14로 처리해서 연속된 구간 확인
            for(int i=1; i<=10; i++){
                int temp = 0;
                // 5개의 연속 구간 확인
                for(int j=i; j<i+5; j++){
                    if(numbers[j] != 1) 
                        break;
                    
                    temp++;
                    gradeNumberCombined[5] += j;
                }
                
                if(temp == 5) 
                    isStraight = true;
            }
            
            if(isStraight)
                grade[5] = true;
            
            bool isFlush = false;
            for(int i=1; i<5; i++) {
                if (suits[i] == 5)
                {
                    isFlush = true;
                    gradeNumberCombined[6] = suitsNumberCombined[i];
                }
            }
            
            if(isFlush) 
                grade[6] = true;
            
            
            // [9] Straight Flush (같은 무늬의 연속된 숫자 5개가 존재)
            if (isFlush && isStraight)
            {
                grade[9] = true;
                gradeNumberCombined[9] = gradeNumberCombined[5];
            }
            
            int pairCnt = 0;
            int tripleCnt = 0;
            int[] pairNumbers = new int[3];
            int tripleNumber = 0;
            // Ace를 1로 단일 숫자로 처리해서 확인 (14 의미 제외)
            for(int i=1; i<14; i++) {
                // [7] 4 Card (네 개의 같은 숫자가 존재)
                if (numbers[i] == 4)
                {
                    grade[8] = true;
                    gradeNumberCombined[8] = i * 4;
                }
 
                // 같은 숫자가 2개
                if (numbers[i] == 2)
                {
                    ++pairCnt;

                    pairNumbers[pairCnt] = i * 2;
                } 
                // 같은 숫자가 3개
                else if (numbers[i] == 3)
                {
                    tripleCnt++;
                    tripleNumber = i;
                }
            }
            
            // [1] 1 Pair (같은 숫자가 한 쌍 존재)
            if (pairCnt == 1)
            {
                grade[2] = true;
                gradeNumberCombined[2] = pairNumbers[1];
            }
                
            // [2] 2 Pair (각기 같은 숫자가 두 쌍 존재)
            if (pairCnt == 2)
            {
                grade[3] = true;
                gradeNumberCombined[3] = pairNumbers[1] + pairNumbers[2];
            }
            
            // [3] Triple (세 개의 같은 숫자가 존재)
            if (tripleCnt == 1)
            {
                grade[4] = true;
                gradeNumberCombined[4] = tripleNumber;
            }
            
            // [6] Full House (Triple과 Pair가 함께 존재)
            if (tripleCnt == 1 && pairCnt == 1)
            {
                grade[7] = true;
                gradeNumberCombined[7] = pairNumbers[1] + tripleNumber;
            }
            
            for (int i = 9; i >= 0; i--)
            {
                /*if (i == 0)
                {
                    break; 
                }
                
                if (grade[i])
                {
                    // 현재 패턴 설정
                    CurrentPattern = cardPatterns.Find(pattern => pattern.PatternType == (CardPatternType)i);
                    
                    foreach (var modifier in cardSubmitEffect.gameplayEffect.modifiers)
                    {
                        if (modifier.attrType != GameplayAttr.BasicAttackPower)
                            continue;
                        
                        modifier.gameplayMagnitude.magnitude = gradeNumberCombined[i];
                    }
                    
                    break;
                }*/
            }
        }

    }
}
