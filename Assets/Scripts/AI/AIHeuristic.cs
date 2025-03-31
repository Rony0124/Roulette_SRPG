using HF.GamePlay;

namespace HF.AI
{
    public class AIHeuristic
    {
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
            /*if (aiplayer.IsDead())
                score += -100000 + node.tdepth * 1000; //Add node depth to seek surviving longest
            if (oplayer.IsDead())
                score += 100000 - node.tdepth * 1000; //Reduce node depth to seek fastest win*/

            //Board state
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

            return score;
        }
           
           public int CalculateActionSort(Game data, AIAction order)
           {
               if (order.type == GameAction.EndTurn)
                   return 0; //End turn can always be performed, 0 means any order
               if (order.type == GameAction.Log)
                   return 1; //End turn can always be performed, 0 means any order
               /*if (data.selector != SelectorType.None)
                   return 0; //Selector actions not affected by sorting

               Card card = data.GetCard(order.card_uid);
               Card target = order.target_uid != null ? data.GetCard(order.target_uid) : null;
               bool is_spell = card != null && !card.CardData.IsBoardCard();*/

               int type_sort = 0;
               /*if (order.type == GameAction.PlayCard && is_spell)
                   type_sort = 1; //Play Spells first
               if (order.type == GameAction.CastAbility)
                   type_sort = 2; //Card Abilities second
               if (order.type == GameAction.Move)
                   type_sort = 3; //Move third
               if (order.type == GameAction.Attack)
                   type_sort = 4; //Attacks fourth
               if (order.type == GameAction.AttackPlayer)
                   type_sort = 5; //Player attacks fifth
               if (order.type == GameAction.PlayCard && !is_spell)
                   type_sort = 7; //Play Characters last

               int card_sort = card != null ? (card.Hash % 100) : 0;
               int target_sort = target != null ? (target.Hash % 100) : 0;
               int sort = type_sort * 10000 + card_sort * 100 + target_sort + 1;*/
               return 0;
           }
           
        //This calculates the score of an individual action, instead of the board state
        //When too many actions are possible in a single node, only the ones with best action score will be evaluated
        //Make sure to return a positive value
        public int CalculateActionScore(Game data, AIAction order)
        {
            /*if (order.type == GameAction.EndTurn)
                return 0; //Other orders are better

            if (order.type == GameAction.CancelSelect)
                return 0; //Other orders are better

            if (order.type == GameAction.CastAbility)
            {
                return 200;
            }

            if (order.type == GameAction.Attack)
            {
                Card card = data.GetCard(order.card_uid);
                Card target = data.GetCard(order.target_uid);
                int ascore = card.GetAttack() >= target.GetHP() ? 300 : 100; //Are you killing the card?
                int oscore = target.GetAttack() >= card.GetHP() ? -200 : 0; //Are you getting killed?
                return ascore + oscore + target.GetAttack() * 5;            //Always better to get rid of high-attack cards
            }
            if (order.type == GameAction.AttackPlayer)
            {
                Card card = data.GetCard(order.card_uid);
                Player player = data.GetPlayer(order.target_player_id);
                int ascore = card.GetAttack() >= player.hp ? 500 : 200;     //Are you killing the player?
                return ascore + (card.GetAttack() * 10) - player.hp;        //Always better to inflict more damage
            }
            if (order.type == GameAction.PlayCard)
            {
                Player player = data.GetPlayer(ai_player_id);
                Card card = data.GetCard(order.card_uid);
                if (card.CardData.IsBoardCard())
                    return 200 + (card.GetMana() * 5) - (30 * player.cards_board.Count); //High cost cards are better to play, better to play when not a lot of cards in play
                else if (card.CardData.IsEquipment())
                    return 200 + (card.GetMana() * 5) - (30 * player.cards_equip.Count);
                else
                    return 200 + (card.GetMana() * 5);
            }

            if (order.type == GameAction.Move)
            {
                return 100;
            }*/

            return 100; //Other actions are better than End/Cancel
        }
    }
}
