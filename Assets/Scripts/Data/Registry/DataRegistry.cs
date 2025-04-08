using TSoft.Core;
using UnityEngine;

namespace TSoft.Data.Registry
{
    [CreateAssetMenu(fileName = "DataRegistry")]
    public class DataRegistry : SingletonScriptable<DataRegistry>
    {
        //TODO 현재 모든 에셋이 다 올라가있는 상태인듯 하다. 프로파일러로 확인해보자
        /*[SerializeField] private AssetReference monsterRegistryRef;
        [SerializeField] private AssetReference stageRegistryRef;*/
        
        [SerializeField]
        private MonsterRegistry monsterRegistry;
        [SerializeField]
        private ArtifactRegistry artifactRegistry;
        [SerializeField]
        private JokerRegistry jokerRegistry;
        [SerializeField]
        private SkillRegistry skillRegistry;
        
        public MonsterRegistry MonsterRegistry => monsterRegistry;
        public ArtifactRegistry ArtifactRegistry => artifactRegistry;
        public JokerRegistry JokerRegistry => jokerRegistry;
        public SkillRegistry SkillRegistry => skillRegistry;

        /*public async UniTask Load()
        {
            monsterRegistry  = await monsterRegistryRef.LoadAssetAsync<MonsterRegistry>();
            stageRegistry  = await stageRegistryRef.LoadAssetAsync<StageRegistry>();
        }*/
    }
}
