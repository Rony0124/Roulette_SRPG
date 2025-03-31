namespace HF.GamePlay
{
    [System.Serializable]
    public class Player
    {
        public int player_id;
        
        public bool is_ai;
        public bool ready = false;

        public Player(int id)
        {
            player_id = id;
        }
    }
}
