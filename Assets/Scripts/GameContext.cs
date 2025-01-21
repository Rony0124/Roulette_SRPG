using System;
using TSoft.Core;
using TSoft.InGame;
using TSoft.Map;
using TSoft.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TSoft
{
    public class GameContext : Singleton<GameContext>
    {
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

        private NodeBlueprint currentNode;
        public NodeBlueprint CurrentNode
        {
            get => currentNode;
            set
            {
                if (currentNode != value)
                {
                    OnCurrentNodeChanged(value);    
                }

                currentNode = value;
            }
        }

        private void OnCurrentNodeChanged(NodeBlueprint node)
        {
            switch (node.nodeType)
            {
                case NodeType.MinorEnemy:
                    SceneManager.LoadScene(Define.InGame);
                    break;
                case NodeType.EliteEnemy:
                    SceneManager.LoadScene(Define.InGame);
                    break;
                case NodeType.Boss:
                    SceneManager.LoadScene(Define.InGame);
                    break;
                case NodeType.Treasure:
                    break;
                case NodeType.Store:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
