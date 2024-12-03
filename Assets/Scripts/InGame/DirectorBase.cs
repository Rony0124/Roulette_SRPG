using System;
using TSoft.InGame.CardSystem;
using UnityEngine;

namespace TSoft.InGame
{
    public abstract class DirectorBase : MonoBehaviour
    {
        private void Awake()
        {

            InitOnAwake();
        }

        protected virtual void InitOnAwake()
        {
            GameContext.Instance.CurrentDirector = this;
        }
    }
}

