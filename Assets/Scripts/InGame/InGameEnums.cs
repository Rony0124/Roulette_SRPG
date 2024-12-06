namespace TSoft.InGame
{
    public enum StageState
    {
        None,
        Intro,
        PrePlaying,         // 플레이 준비 단계
        Playing,            // 실제 플레이 단계
        PostPlaying,        // 타임아웃이나 클리어 등의 조건으로 플레이가 종료된 단계 (여기서 아웃트로를 선택)
        OutroSuccess,
        OutroFailed,
        OutroReset,
        Exit,               // 로비로 돌아간다.
    };
    
    public enum GameState
    {
        None,
        Ready,
        Play,
        FinishSuccess,
        FinishFailed
    };
    
    public enum CardType
    {
        None,
        Diamond,
        Club,
        Spade,
        Heart
    }

    public enum CardPatternType
    {
        StraightFlush,
        FourOfKind,
        FullHouse,
        Flush,
        Straight,
        ThreeOfKind,
        TwoPair,
        OnePair,
        HighCard
    }
    
    public enum GameplayAttr
    {
        None = 0,
        
        Heart = 1000,
        MaxHeart = 1001,
        Energy = 1002,
        MaxEnergy = 1003,
        Capacity = 1004,
        MaxCapacity = 1005,
        
        BasicAttackPower = 2000,
    }
}
