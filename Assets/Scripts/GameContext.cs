using TSoft.Core;
using TSoft.Data.Registry;
using TSoft.InGame;
using UnityEngine;

namespace TSoft
{
    public class GameContext : Singleton<GameContext>
    {
        //TODO 임시로 에셋을 여기다 놓았다. 어드레서블 완성되면 옮ㅁ겨주자
        [SerializeField]
        private MonsterRegistry monsterRegistry; 
        public MonsterRegistry MonsterRegistry => monsterRegistry;
        
        [SerializeField]
        private StageRegistry stageRegistry; 
        public StageRegistry StageRegistry => stageRegistry;
        
        private DirectorBase currentDirector;

        public DirectorBase CurrentDirector
        {
            get => currentDirector;
            set
            {
                if (value != currentDirector)
                {
                    var oldDirName = currentDirector ? currentDirector.GetType().ToString() : "null";
                    Debug.Log($"local director has changed - old dir : {oldDirName} new dir : {value.GetType()}");
                    
                    OnDirectorChanged?.Invoke(currentDirector, value);
                }
               
                currentDirector = value;
            }
        }
        
        public delegate void OnLocalDirectorChanged(DirectorBase previousValue, DirectorBase newValue);
        
        public OnLocalDirectorChanged OnDirectorChanged;
    }
}
