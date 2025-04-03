using System.Collections.Generic;
using HF.API;
using HF.Data;
using HF.Data.Card;
using HF.InGame;
using HF.Utils;
using UnityEngine;

namespace HF.GamePlay
{
    public class GameLogic
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
        
        public  void Update(float delta)
        {
            resolveQueue.Update(delta);
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
                //Hp / mana
                /*player.hp_max = pdeck != null ? pdeck.start_hp : GameplayData.Get().hp_start;
                player.hp = player.hp_max;
                player.mana_max = pdeck != null ? pdeck.start_mana : GameplayData.Get().mana_start;
                player.mana = player.mana_max;*/

                //Draw starting cards
                DrawCard(player, CombatController.MaxCardCapacity);

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
            
            Debug.Log("current gameDataPhase - " + gameData.phase);
            /*RefreshData();
            onTurnStart?.Invoke();*/

            Player player = gameData.GetActivePlayer();

            //Cards draw
            if (gameData.turn_count > 1 || player.player_id != gameData.first_player)
            {
                DrawCard(player, CombatController.MaxCardCapacity - player.cards_hand.Count);
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

        public void Log()
        {
            resolveQueue.ResolveAll(0.2f);
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
        
         //--- Setup ------

        //Set deck using a Deck in Resources
        public void SetPlayerDeck(Player player, DeckData deck)
        {
            player.cards_all.Clear();
            player.cards_deck.Clear();

            foreach (CardData card in deck.cards)
            {
                if (card != null)
                {
                    Card acard = Card.Create(card, player);
                    player.cards_deck.Add(acard);
                }
            }

            ShuffleDeck(player.cards_deck);
        }

        //Set deck using custom deck in save file or database
        /*public void SetPlayerDeck(Player player, UserDeckData deck)
        {
            player.cards_all.Clear();
            player.cards_deck.Clear();

            foreach (UserCardData card in deck.cards)
            {
                CardData icard = CardData.Get(card.tid);
                if (icard != null)
                {
                    Card acard = Card.Create(icard, player);
                    player.cards_deck.Add(acard);
                }
            }

            //Shuffle deck
            ShuffleDeck(player.cards_deck);
        }*/
        
        public virtual void DrawCard(Player player, int nb = 1)
        {
            for (int i = 0; i < nb; i++)
            {
                if (player.cards_deck.Count > 0 && player.cards_hand.Count < CombatController.MaxCardCapacity)
                {
                    Card card = player.cards_deck[0];
                    player.cards_deck.RemoveAt(0);
                    player.cards_hand.Add(card);
                }
            }
        }
        
        public virtual void ShuffleDeck(List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Card temp = cards[i];
                int randomIndex = random.Next(i, cards.Count);
                cards[i] = cards[randomIndex];
                cards[randomIndex] = temp;
            }
        }

    }
}
