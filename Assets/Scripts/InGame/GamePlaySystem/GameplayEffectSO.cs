using UnityEngine;

namespace TSoft.InGame.GamePlaySystem
{
    [CreateAssetMenu(fileName = "GameplayEffect", menuName = "Create GameplayEffect", order = 1)]
    public class GameplayEffectSO : ScriptableObject
    {
        public GameplayEffectModifier[] modifiers;
    }
}
