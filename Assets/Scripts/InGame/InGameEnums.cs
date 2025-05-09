using System;

namespace TSoft.InGame
{
    [Serializable]
    public enum StageState : int
    {
        None = 0,
        Intro = 1,
        PrePlaying = 2,         // 플레이 준비 단계
        Playing = 3,            // 실제 플레이 단계
        PostPlayingSuccess = 4,        // 타임아웃이나 클리어 등의 조건으로 플레이가 종료된 단계 (여기서 아웃트로를 선택)
        PostPlayingFailed = 5,
        Outro = 6,
        Exit = 7,               // 로비로 돌아간다.
    };
    
    [Serializable]
    public enum GamePhase
    {
        None = 0,
        StartTurn = 10, //Start of turn resolution
        Main = 20,      //Main play phase
        EndTurn = 30,   //End of turn resolutions
    }

    public enum ConditionType
    {
        None,
        Random,
        Card
    }
    
    public enum OddEven
    {
        Odd,
        Even
    }
    
    public enum CardConditionType
    {
        None,
        CardPattern,
        NumberCombination,
        OddEven,
        CardType
    }
    
    public enum CardType
    {
        None,
        Diamond,
        Club,
        Spade,
        Heart,
        Joker
    }

    public enum CardPatternType
    {
        None,
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfKind,
        Straight,
        Flush,
        FullHouse,
        FourOfKind,
        StraightFlush,
    }
    
    public enum ModifierOpType
    {
        None,
        Add,
        Multiply,
        Divide,
        Override
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
        SkillAttackPower = 2001,
        FinalDamageFactor = 2002
    }
    
    public enum GameplayPolicyType
    {
        None,
        Passive,
        OnRoundBegin,
        OnRoundFinished,
        OnTurnBegin,
        OnTurnFinished
    }
}
