namespace TSoft.InGame
{
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
