using TSoft.Data.Card;
using UnityEngine;

namespace TSoft.Data.Registry
{
    [CreateAssetMenu(fileName = "ArtifactRegistry", menuName = "DataRegistry/Artifact Registry", order = 3)]
    public class ArtifactRegistry : Registry<ArtifactInfo>
    {
    }
}
