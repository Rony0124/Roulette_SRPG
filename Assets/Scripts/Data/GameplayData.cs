using HF.Data.Card;
using UnityEngine;

namespace HF.Data
{
    [CreateAssetMenu(fileName = "GameplayData", menuName = "Data/GameplayData", order = 0)]
    public class GameplayData : ScriptableObject
    {
        [Header("Test")]
        public DeckData[] test_deck;          //For when starting the game directly from Unity game scene
        public DeckData[] test_deck_ai;       //For when starting the game directly from Unity game scene
    }
}
