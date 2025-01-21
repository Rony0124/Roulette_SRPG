namespace TSoft.Utils
{
    public static class Define
    {
        public enum UIEvent
        {
            Click,
            Drag,
        }

        public enum MouseEvent
        {
            Press,
            Click,
        }

        public static readonly string Lobby = "Lobby";
        public static readonly string InGame = "InGame";
        public static readonly string StageMap = "StageMap";
        
        public static readonly int MonsterSlotMax = 1;
        public static readonly int RewardSlot = 2;
        public static readonly int BossSlot = 3;
    }
}
