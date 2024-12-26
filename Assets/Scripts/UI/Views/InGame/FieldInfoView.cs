using System.Collections.Generic;
using TSoft.InGame;
using TSoft.UI.Core;

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
        
        private void Start()
        {
            Bind<TMPro.TextMeshProUGUI>(typeof(FieldInfoText));
            
            txtName = Get<TMPro.TextMeshProUGUI>((int)FieldInfoText.MonsterNameTxt);
            txtHp = Get<TMPro.TextMeshProUGUI>((int)FieldInfoText.FieldHpTxt);

            FieldController.OnDamaged += UpdateMonsterHp;
            FieldController.OnFieldSpawn += UpdateOnMonsterSpawn;
        }
        
        protected override void OnActivated()
        {
        }

        protected override void OnDeactivated()
        {
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

        private void UpdateMonsterHp(float hp)
        {
            txtHp.text = (int)hp + "";
        }
    }
}
