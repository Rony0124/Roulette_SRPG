using HF.GamePlay;
using UnityEngine;

namespace HF.Utils
{
    public class ResolveQueue
    {
        private Game gameData;
        private bool isResolving = false;
        private float resolveDelay = 0f;
        private bool skipDelay = false;
        
        public ResolveQueue(Game data, bool skip)
        {
            gameData = data;
            skipDelay = skip;
        }
        
        public void SetData(Game data)
        {
            gameData = data;
        }
        
        public bool IsResolving()
        {
            return isResolving || resolveDelay > 0f;
        }
        
        public void Clear()
        {
          
        }
    }
}
