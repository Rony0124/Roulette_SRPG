using System;
using UnityEngine;

namespace TSoft.InGame.GamePlaySystem
{
    public partial class Gameplay : MonoBehaviour
    {
        private void Awake()
        {
            InitializeAttributes();
            InitializeEffect();
        }
    }
}
