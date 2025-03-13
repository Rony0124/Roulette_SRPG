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
        }
        
        public void UpdateMonsterNameText(string monsterName)
        {
            txtName.text = monsterName;
        }

        public void UpdateMonsterHpText(float hp, float maxHp)
        {
            txtHp.text = (int)hp + " / " + (int)maxHp;
        }
    }
}
