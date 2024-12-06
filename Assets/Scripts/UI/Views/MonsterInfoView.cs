using TSoft.Data.Monster;
using TSoft.InGame;
using TSoft.UI.Core;

namespace TSoft.UI.Views
{
    public class MonsterInfoView : ViewBase
    {
        private enum MonsterInfoText
        {
            MonsterNameTxt,
            MonsterHpTxt
        }
        
        private TMPro.TextMeshProUGUI txtName;
        private TMPro.TextMeshProUGUI txtHp;
        
        private void Start()
        {
            Bind<TMPro.TextMeshProUGUI>(typeof(MonsterInfoText));
            
            txtName = Get<TMPro.TextMeshProUGUI>((int)MonsterInfoText.MonsterNameTxt);
            txtHp = Get<TMPro.TextMeshProUGUI>((int)MonsterInfoText.MonsterHpTxt);

            MonsterController.OnMonsterDamaged += UpdateMonsterHp;
            MonsterController.OnMonsterSpawn += UpdateOnMonsterSpawn;
        }
        
        protected override void OnActivated()
        {
        }

        protected override void OnDeactivated()
        {
        }

        private void UpdateOnMonsterSpawn(MonsterData data)
        {
            txtName.text = data.Name;
            txtHp.text = (int)data.Hp + "";
        }

        private void UpdateMonsterHp(float hp)
        {
            txtHp.text = (int)hp + "";
        }
    }
}
