using System;
using TSoft.Core;
using TSoft.Data.Monster;
using TSoft.InGame;
using TSoft.Map;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TSoft
{
    public class GameContext : Singleton<GameContext>
    {
        public struct StageInfo
        {
            public int stage;
            public int round;
        }
        
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
        
        public MonsterDataSO CurrentMonster { get; set; }

        public StageInfo currentStageInfo;

        public double currentBounty;
        
        private void OnCurrentNodeChanged(MapNode node)
        {
            switch (node.Blueprint.nodeType)
            {
                case NodeType.MinorEnemy:
                    currentStageInfo.round++;
                    
                    SceneManager.LoadScene(Define.InGame);
                    break;
                case NodeType.EliteEnemy:
                    currentStageInfo.round++;
                    
                    SceneManager.LoadScene(Define.InGame);
                    break;
                case NodeType.Boss:
                    currentStageInfo.round++;
                    
                    SceneManager.LoadScene(Define.InGame);
                    break;
                case NodeType.Treasure:
                    break;
                case NodeType.Store:
                    
                    SceneManager.LoadScene(Define.StageMap);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
