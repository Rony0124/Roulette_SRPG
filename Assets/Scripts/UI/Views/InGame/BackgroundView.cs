using System;
using System.Collections.Generic;
using System.Linq;
using HF.InGame;
using InGame;
using Sirenix.OdinInspector;
using TSoft.Data.Monster;
using TSoft.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace HF.UI.Views.InGame
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
