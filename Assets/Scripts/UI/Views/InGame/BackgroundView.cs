using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TSoft.Data.Monster;
using TSoft.InGame;
using TSoft.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Views.InGame
{
    public class BackgroundView : ViewBase
    {
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
        
        [SerializeField] private CombatController combat;
        
        private void Awake()
        {
            Bind<Image>(typeof(BgImage));
            
            bg = Get<Image>((int)BgImage.Background);
            
            //combat.OnMonsterSpawn += SetBackground;
        }

        private void SetBackground(MonsterController monsterController)
        {
            var type = monsterController.Data.monsterType;
            var monsterBg = bgList.FirstOrDefault(item => item.monsterType == type);
            if (monsterBg != null)
            {
                bg.sprite = monsterBg.background;
            }
        }
    }
}
