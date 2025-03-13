using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TSoft.Data.Monster;
using TSoft.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Views.InGame
{
    public class FieldInfoView : ViewBase
    {
        private enum FieldInfoText
        {
            MonsterNameTxt,
            FieldHpTxt
        }
        
        private TMPro.TextMeshProUGUI txtName;
        private TMPro.TextMeshProUGUI txtHp;
        private Image bg;
        
        private void Start()
        {
            Bind<TMPro.TextMeshProUGUI>(typeof(FieldInfoText));
         
            
            txtName = Get<TMPro.TextMeshProUGUI>((int)FieldInfoText.MonsterNameTxt);
            txtHp = Get<TMPro.TextMeshProUGUI>((int)FieldInfoText.FieldHpTxt);
        }
        
        public void SetMonsterNameText(string monsterName)
        {
            txtName.text = monsterName;
        }

        public void SetMonsterHpText(float hp, float maxHp)
        {
            txtHp.text = (int)hp + " / " + (int)maxHp;
        }
    }
}
