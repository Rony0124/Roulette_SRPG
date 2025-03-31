using HF.Utils;

namespace HF.GamePlay
{
    public sealed class GameLogic
    {
        private Game gameData;
        
        private ResolveQueue resolveQueue;
        private bool isAi;
        
        private System.Random random = new ();
        
        public GameLogic(bool isAi)
        {
            resolveQueue = new ResolveQueue(null, isAi);
            this.isAi = isAi;
        }
        
        public GameLogic(Game game)
        {
            gameData = game;
            resolveQueue = new ResolveQueue(game, false);
        }
        
        public void SetData(Game game)
        {
            gameData = game;
            resolveQueue.SetData(game);
        }
        
        public void StartGame()
        {
            if (gameData.state == GameState.GameEnded)
                return;

            //Choose first player
            gameData.state = GameState.Play;
            gameData.first_player = random.NextDouble() < 0.5 ? 0 : 1;
            gameData.current_player = gameData.first_player;
            gameData.turn_count = 1;

            //Init each players
            foreach (Player player in gameData.players)
            {
                //Puzzle level deck
                //DeckPuzzleData pdeck = DeckPuzzleData.Get(player.deck);

                //Hp / mana
                /*player.hp_max = pdeck != null ? pdeck.start_hp : GameplayData.Get().hp_start;
                player.hp = player.hp_max;
                player.mana_max = pdeck != null ? pdeck.start_mana : GameplayData.Get().mana_start;
                player.mana = player.mana_max;*/

                //Draw starting cards
               // int dcards = pdeck != null ? pdeck.start_cards : GameplayData.Get().cards_start;
               // DrawCard(player, dcards);

                /*//Add coin second player
                bool is_random = level == null || level.first_player == LevelFirst.Random;
                if (is_random && player.player_id != game_data.first_player && GameplayData.Get().second_bonus != null)
                {
                    Card card = Card.Create(GameplayData.Get().second_bonus, VariantData.GetDefault(), player);
                    player.cards_hand.Add(card);
                }*/
            }

            //Start state
            /*RefreshData();
            onGameStart?.Invoke();*/

            /*if(should_mulligan)
                GoToMulligan();
            else*/
                StartTurn();
        }
        
        public void StartTurn()
        {
            if (gameData.state == GameState.GameEnded)
                return;

            ClearTurnData();
            gameData.phase = GamePhase.StartTurn;
            /*RefreshData();
            onTurnStart?.Invoke();*/

            Player player = gameData.GetActivePlayer();

            //Cards draw
            if (gameData.turn_count > 1 || player.player_id != gameData.first_player)
            {
               // DrawCard(player, GameplayData.Get().cards_per_turn);
            }

            //Mana 
            /*player.mana_max += GameplayData.Get().mana_per_turn;
            player.mana_max = Mathf.Min(player.mana_max, GameplayData.Get().mana_max);
            player.mana = player.mana_max;*/

            //Turn timer and history
            /*game_data.turn_timer = GameplayData.Get().turn_duration;
            player.history_list.Clear();

            //Player poison
            if (player.HasStatus(StatusType.Poisoned))
                player.hp -= player.GetStatusValue(StatusType.Poisoned);

            if (player.hero != null)
                player.hero.Refresh();

            //Refresh Cards and Status Effects
            for (int i = player.cards_board.Count - 1; i >= 0; i--)
            {
                Card card = player.cards_board[i];

                if (!card.HasStatus(StatusType.Sleep))
                    card.Refresh();

                if (card.HasStatus(StatusType.Poisoned))
                    DamageCard(card, card.GetStatusValue(StatusType.Poisoned));
            }*/

            //Ongoing Abilities
            /*UpdateOngoing();

            //StartTurn Abilities
            TriggerPlayerCardsAbilityType(player, AbilityTrigger.StartOfTurn);
            TriggerPlayerSecrets(player, AbilityTrigger.StartOfTurn);*/

            resolveQueue.AddCallback(StartMainPhase);
            resolveQueue.ResolveAll(0.2f);
        }
        
        public void StartNextTurn()
        {
            if (gameData.state == GameState.GameEnded)
                return;

            gameData.current_player = (gameData.current_player + 1) % gameData.settings.nb_players;

            if (gameData.current_player == gameData.first_player)
                gameData.turn_count++;

            CheckForWinner();
            StartTurn();
        }
        
        public void StartMainPhase()
        {
            if (gameData.state == GameState.GameEnded)
                return;

            gameData.phase = GamePhase.Main;
        }
        
        public void EndTurn()
        {
            if (gameData.state == GameState.GameEnded)
                return;
            
            if (gameData.phase != GamePhase.Main)
                return;

            gameData.selector = SelectorType.None;
            gameData.phase = GamePhase.EndTurn;

            //Reduce status effects with duration
            foreach (Player aplayer in gameData.players)
            {
                //aplayer.ReduceStatusDurations();
                /*foreach (Card card in aplayer.cards_board)
                    card.ReduceStatusDurations();
                foreach (Card card in aplayer.cards_equip)
                    card.ReduceStatusDurations();*/
            }

            //End of turn abilities
            Player player = gameData.GetActivePlayer();
            //TriggerPlayerCardsAbilityType(player, AbilityTrigger.EndOfTurn);

            //onTurnEnd?.Invoke();
            //RefreshData();

            resolveQueue.AddCallback(StartNextTurn);
            resolveQueue.ResolveAll(0.2f);
        }
        
        //End game with winner
        public void EndGame(int winner)
        {
            if (gameData.state != GameState.GameEnded)
            {
                gameData.state = GameState.GameEnded;
                gameData.phase = GamePhase.None;
                gameData.selector = SelectorType.None;
                gameData.current_player = winner; //Winner player
                resolveQueue.Clear();
                Player player = gameData.GetPlayer(winner);
               // onGameEnd?.Invoke(player);
              //  RefreshData();
            }
        }
        
        //Progress to the next step/phase 
        public void NextStep()
        {
            if (gameData.state == GameState.GameEnded)
                return;

            //CancelSelection();

            //Add to resolve queue in case its still resolving
            resolveQueue.AddCallback(EndTurn);
            resolveQueue.ResolveAll();
        }
        
        private void CheckForWinner()
        {
            int count_alive = 0;
            Player alive = null;
            foreach (Player player in gameData.players)
            {
                /*if (!player.IsDead())
                {
                    alive = player;
                    count_alive++;
                }*/
            }

            if (count_alive == 0)
            {
                EndGame(-1); //Everyone is dead, Draw
            }
            else if (count_alive == 1)
            {
                EndGame(alive.player_id); //Player win
            }
        }
        
        private void ClearTurnData()
        {
            gameData.selector = SelectorType.None;
            resolveQueue.Clear();
        }
        
        public void ClearResolve()
        {
            resolveQueue.Clear();
        }
        
        public bool IsResolving()
        {
            return resolveQueue.IsResolving();
        }
        
        public Game GetGameData()
        {
            return gameData;
        }
    }
}
