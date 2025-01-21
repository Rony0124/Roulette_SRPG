using System;
using TSoft.Data.Monster;
using TSoft.InGame;
using TSoft.UI.Core;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace TSoft.UI.Views.InGame
{
    public class BackgroundView : ViewBase
    {
        public Action<MonsterType> OnMonsterSpawn;
        
        private enum BgImage
        {
            Background
        }

        private Image bg;

        [SerializeField] 
        private SerializedDictionary<MonsterType, Sprite> bgDictionary;
        
        private void Awake()
        {
            Bind<Image>(typeof(BgImage));
            
            bg = Get<Image>((int)BgImage.Background);
        }
        
        protected override void OnActivated()
        {
            OnMonsterSpawn += SetBackground;
        }

        protected override void OnDeactivated()
        {
            OnMonsterSpawn -= SetBackground;
        }

        private void SetBackground(MonsterType type)
        {
            if (bgDictionary.TryGetValue(type, out var sp))
            {
                bg.sprite = sp;    
            }
        }
    }
}
