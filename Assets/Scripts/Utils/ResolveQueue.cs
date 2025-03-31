using System;
using System.Collections.Generic;
using HF.GamePlay;
using UnityEngine;

namespace HF.Utils
{
    public class ResolveQueue
    {
        private Pool<CallbackQueueElement> callbackElemPool = new ();
        
        private Queue<CallbackQueueElement> callbackQueue = new ();
        
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
        
        public virtual void AddCallback(Action callback)
        {
            if (callback != null)
            {
                CallbackQueueElement elem = callbackElemPool.Create();
                elem.callback = callback;
                callbackQueue.Enqueue(elem);
            }
        }
        
        public virtual void ResolveAll(float delay)
        {
            SetDelay(delay);
            ResolveAll();  //Resolve now if no delay
        }
        
        public virtual void ResolveAll()
        {
            if (isResolving)
                return;

            isResolving = true;
            while (CanResolve())
            {
                Resolve();
            }
            isResolving = false;
        }
        
        public void Resolve()
        {
            if (callbackQueue.Count > 0)
            {
                CallbackQueueElement elem = callbackQueue.Dequeue();
                callbackElemPool.Dispose(elem);
                elem.callback.Invoke();
            }
        }
        
        public void SetDelay(float delay)
        {
            if (!skipDelay)
            {
                resolveDelay = Mathf.Max(resolveDelay, delay);
            }
        }
        
        public bool CanResolve()
        {
            if (resolveDelay > 0f)
                return false;   //Is waiting delay
            if (gameData.state == GameState.GameEnded)
                return false; //Cant execute anymore when game is ended
            if (gameData.selector != SelectorType.None)
                return false; //Waiting for player input, in the middle of resolve loop
            return callbackQueue.Count > 0;
        }
        
        public bool IsResolving()
        {
            return isResolving || resolveDelay > 0f;
        }
        
        public void Clear()
        {
          
        }
    }
    
    public class CallbackQueueElement
    {
        public Action callback;
    }
}
