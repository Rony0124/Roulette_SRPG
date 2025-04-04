using System;
using System.Collections.Generic;
using HF.GamePlay;
using UnityEngine;

namespace HF.Utils
{
    public class ResolveQueue
    {
        private Pool<CallbackQueueElement> callbackElemPool = new ();
        private Pool<AttackQueueElement> attack_elem_pool = new ();
        
        private Queue<CallbackQueueElement> callbackQueue = new ();
        private Queue<AttackQueueElement> attack_queue = new ();
        
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
        
        public void Update(float delta)
        {
            if (resolveDelay > 0f)
            {
                resolveDelay -= delta;
                if (resolveDelay <= 0f)
                    ResolveAll();
            }
        }
        
        public void AddCallback(Action callback)
        {
            if (callback != null)
            {
                CallbackQueueElement elem = callbackElemPool.Create();
                elem.callback = callback;
                callbackQueue.Enqueue(elem);
            }
        }
        
        public void AddAttack(Player attacker, Player target, Action<Player, Player> callback)
        {
            if (attacker != null && target != null)
            {
                AttackQueueElement elem = attack_elem_pool.Create();
                elem.attacker = attacker;
                elem.target = target;
                elem.callback = callback;
                attack_queue.Enqueue(elem);
            }
        }
        
        public void ResolveAll(float delay)
        {
            SetDelay(delay);
            ResolveAll();  //Resolve now if no delay
        }
        
        public void ResolveAll()
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
    
    public class AttackQueueElement
    {
        public Player attacker;
        public Player target;
        public Action<Player, Player> callback;
    }
}
