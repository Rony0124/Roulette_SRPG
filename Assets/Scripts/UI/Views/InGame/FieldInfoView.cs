using System;
using System.Collections.Generic;
using TSoft.InGame;
using TSoft.UI.Core;
using UnityEngine.Serialization;

namespace TSoft.UI.Views.InGame
{
    public class FieldInfoView : ViewBase
    {
        public Action<float> OnDamaged;
        public Action<FieldController.FieldSlot> OnMonsterSpawn;
        public Action OnRewardSpawn;
        
        private enum FieldInfoText
        {
            MonsterNameTxt,
            FieldHpTxt
        }
        
        private TMPro.TextMeshProUGUI txtName;
        private TMPro.TextMeshProUGUI txtHp;
        
        private void Start()
        {
            Bind<TMPro.TextMeshProUGUI>(typeof(FieldInfoText));
            
            txtName = Get<TMPro.TextMeshProUGUI>((int)FieldInfoText.MonsterNameTxt);
            txtHp = Get<TMPro.TextMeshProUGUI>((int)FieldInfoText.FieldHpTxt);
        }
        
        protected override void OnActivated()
        {
            OnDamaged += UpdateMonsterHp;
            OnMonsterSpawn += UpdateOnMonsterSpawn;
        }

        protected override void OnDeactivated()
        {
            OnDamaged -= UpdateMonsterHp;
            OnMonsterSpawn -= UpdateOnMonsterSpawn;
        }

        private void UpdateOnMonsterSpawn(FieldController.FieldSlot data)
        {
            Dictionary<string, int> names = new Dictionary<string, int>();
            foreach (var monster in data.monsters)
            {
                var name = monster.Data.Name;
                if (names.ContainsKey(name))
                {
                    names[name]++;
                }
                else
                {
                    names.Add(name, 1);
                }
            }

            int i = 0;
            txtName.text = "";
            
            foreach (var name in names.Keys)
            {
                txtName.text += name;
                txtName.text += "x" + names[name];
                
                if (i == names.Keys.Count - 1)
                    break;
                
                txtName.text += ", ";
                
                i++;
            }
            
            txtHp.text = data.hp + "";
        }

        private void UpdateOnRewardSpawn()
        {
            
        }

        private void UpdateMonsterHp(float hp)
        {
            txtHp.text = (int)hp + "";
        }
    }
}
