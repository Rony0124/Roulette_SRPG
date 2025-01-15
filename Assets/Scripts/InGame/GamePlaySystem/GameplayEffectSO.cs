using Sirenix.OdinInspector;
using UnityEngine;

namespace TSoft.InGame.GamePlaySystem
{
    [CreateAssetMenu(fileName = "GameplayEffect", menuName = "Create GameplayEffect", order = 1)]
    public class GameplayEffectSO : ScriptableObject
    {
        [Header("Modifiers")]
        public GameplayDuration duration;
        
        [HideIf("@duration.policy", GameplayDuration.PolicyType.Instant)]
        [Tooltip("이팩트가 반복해서 적용되는 주기(초)입니다. 0으로 설정하면 주기적으로 실행되지 않습니다.")]
        public float period;

        [HideIf("@duration.policy", GameplayDuration.PolicyType.Instant)]
        [Tooltip("이팩트를 적용할때마다 Modifier의 값들을 다시 캡쳐합니다.")]
        public bool dynamic;
        
        public GameplayEffectModifier[] modifiers;
    }
}
