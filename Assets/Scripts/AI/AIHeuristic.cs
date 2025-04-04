using HF.GamePlay;
using InGame;

namespace HF.AI
{
    public class AIHeuristic
    {
        public int joker_value = 15;
        public int pattern_value = 10;
        
        private int aiPlayerId;
        private int heuristicModifier = 10; //Randomize heuristic for lower level ai, set 10 for some randomness
        private System.Random aiRan;
        
        public AIHeuristic(int playerId)
        {
            aiPlayerId = playerId;
            aiRan = new System.Random();
        }
        
        public int CalculateHeuristic(Game data, NodeState node)
        {
            Player aiPlayer = data.GetPlayer(aiPlayerId);
            Player oPlayer = data.GetOpponentPlayer(aiPlayerId);
            return CalculateHeuristic(data, node, aiPlayer, oPlayer);
        }
        
        public int CalculateHeuristic(Game data, NodeState node, Player aiPlayer, Player oPlayer)
        {
            int score = 0;
            
            //Victories
            if (aiPlayer.IsDead())
                score += -100000 + node.tdepth * 1000; //Add node depth to seek surviving longest
            if (oPlayer.IsDead())
                score += 100000 - node.tdepth * 1000; //Reduce node depth to seek fastest win

            //Board state
            score += aiPlayer.cards_pattern.Count * pattern_value;

            int aiJokerCount = 0;
            foreach (var card in aiPlayer.cards_hand)
            {
                if (card.Data.type == CardType.Joker)
                    aiJokerCount++;
            }

            score += aiJokerCount * joker_value;
            
            
            /*score += aiplayer.cards_board.Count * board_card_value;
            score += aiplayer.cards_equip.Count * board_card_value;
            score += aiplayer.cards_secret.Count * secret_card_value;
            score += aiplayer.cards_hand.Count * hand_card_value;
            score += aiplayer.kill_count * kill_value;
            score += aiplayer.hp * player_hp_value;

            score -= oplayer.cards_board.Count * board_card_value;
            score -= oplayer.cards_equip.Count * board_card_value;
            score -= oplayer.cards_secret.Count * secret_card_value;
            score -= oplayer.cards_hand.Count * hand_card_value;
            score -= oplayer.kill_count * kill_value;
            score -= oplayer.hp * player_hp_value;

            
            foreach (Card card in aiplayer.cards_board)
            {
                score += card.GetAttack() * card_attack_value;
                score += card.GetHP() * card_hp_value;

                foreach (CardStatus status in card.status)
                    score += status.StatusData.hvalue * card_status_value;
                foreach (CardStatus status in card.ongoing_status)
                    score += status.StatusData.hvalue * card_status_value;
            }
            foreach (Card card in oplayer.cards_board)
            {
                score -= card.GetAttack() * card_attack_value;
                score -= card.GetHP() * card_hp_value;

                foreach (CardStatus status in card.status)
                    score -= status.StatusData.hvalue * card_status_value;
                foreach (CardStatus status in card.ongoing_status)
                    score -= status.StatusData.hvalue * card_status_value;
            }

            if (heuristic_modifier > 0)
                score += random_gen.Next(-heuristic_modifier, heuristic_modifier);*/
            
            if (heuristicModifier > 0)
                score += aiRan.Next(-heuristicModifier, heuristicModifier);

            return score;
        }
           
       public int CalculateActionSort(Game data, AIAction order)
       {
           if (order.type == GameAction.EndTurn)
               return 0; //End turn can always be performed, 0 means any order
           if (data.selector != SelectorType.None)
               return 0; //Selector actions not affected by sorting

           var att = data.GetActivePlayer();
           var target = data.GetOpponentPlayer(att.player_id);

           int type_sort = 0;
           if (order.type == GameAction.PlayJoker)
               type_sort = 1; //Play Spells first
           if (order.type == GameAction.PlayPattern)
               type_sort = 2; //Player attacks second

           /*int card_sort = att != null ? (att.Hash % 100) : 0;
           int target_sort = target != null ? (target.Hash % 100) : 0;*/
           int sort = type_sort * 10000;
           return sort;
       }
           
        //This calculates the score of an individual action, instead of the board state
        //When too many actions are possible in a single node, only the ones with best action score will be evaluated
        //Make sure to return a positive value
        public int CalculateActionScore(Game data, AIAction order)
        {
            if (order.type == GameAction.EndTurn)
                return 0; //Other orders are better

            if (order.type == GameAction.PlayJoker)
            {
                return 200;
            }

            if (order.type == GameAction.PlayPattern)
            {
                var att = data.GetActivePlayer();
                var target = data.GetOpponentPlayer(att.player_id);
                int ascore = att.GetAttack() >= target.hp ? 500 : 200;
                return ascore + (att.GetAttack() * 10) - target.hp;      
            }
            
            return 100; //Other actions are better than End/Cancel
        }
    }
}
