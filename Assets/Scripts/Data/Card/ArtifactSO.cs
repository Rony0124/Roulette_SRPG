using TSoft.InGame.GamePlaySystem;
using UnityEngine;

namespace TSoft.Data.Card
{
    [CreateAssetMenu(fileName = "Artifact", menuName = "Data/Artifact", order = 0)]
    public class ArtifactSO : ItemSO
    {
        public GameplayEffectSO effect;
    }
}
