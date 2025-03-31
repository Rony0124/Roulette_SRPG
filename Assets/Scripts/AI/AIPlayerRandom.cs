using HF.GamePlay;
using UnityEngine;

namespace HF.AI
{
    public class AIPlayerRandom : AIPlayer
    {
        private bool is_playing = false;
        private bool is_selecting = false;

        private System.Random rand = new System.Random();
        
        public AIPlayerRandom(GameLogic gameplay, int id)
        {
            this.gameplay = gameplay;
            playerId = id;
        }
        
        public override void Update()
        {
            if (!CanPlay())
                return;

            Game game_data = gameplay.GetGameData();
            Player player = game_data.GetPlayer(playerId);

            if (game_data.IsPlayerTurn(player) && !gameplay.IsResolving())
            {
                if(!is_playing && game_data.selector == SelectorType.None && game_data.current_player == playerId)
                {
                    is_playing = true;
                   // TimeTool.StartCoroutine(AiTurn());
                }

                if (!is_selecting && game_data.selector != SelectorType.None && game_data.selector_player_id == playerId)
                {
                    if (game_data.selector == SelectorType.SelectTarget)
                    {
                        //AI select target
                        is_selecting = true;
                       // TimeTool.StartCoroutine(AiSelectTarget());
                    }

                    if (game_data.selector == SelectorType.SelectorCard)
                    {
                        //AI select target
                        is_selecting = true;
                       // TimeTool.StartCoroutine(AiSelectCard());
                    }

                    if (game_data.selector == SelectorType.SelectorChoice)
                    {
                        //AI select target
                        is_selecting = true;
                       // TimeTool.StartCoroutine(AiSelectChoice());
                    }

                    if (game_data.selector == SelectorType.SelectorCost)
                    {
                        //AI select target
                        is_selecting = true;
                     //   TimeTool.StartCoroutine(AiSelectCost());
                    }
                }

            }

            /*if (!is_selecting && game_data.IsPlayerMulliganTurn(player))
            {
                is_selecting = true;
                TimeTool.StartCoroutine(AiSelectMulligan());
            }*/
        }
    }
}
