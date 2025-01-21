using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
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

        [Serializable]
        public class MonsterBackground
        {
            public MonsterType monsterType;
            public Sprite background;
        }

        private Image bg;

        [SerializeField][TableList]
        private List<MonsterBackground> bgList;
        
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
            var monsterBg = bgList.FirstOrDefault(item => item.monsterType == type);
            if (monsterBg != null)
            {
                bg.sprite = monsterBg.background;
            }
        }
    }
}
