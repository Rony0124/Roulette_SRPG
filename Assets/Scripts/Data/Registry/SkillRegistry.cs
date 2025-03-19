using TSoft.Data.Skill;
using UnityEngine;

namespace TSoft.Data.Registry
{
    [CreateAssetMenu(fileName = "SkillRegistry", menuName = "DataRegistry/Skill Registry", order = 5)]
    public class SkillRegistry : Registry<SkillInfo>
    {
    }
}
