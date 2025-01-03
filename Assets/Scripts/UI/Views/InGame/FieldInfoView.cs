using System;
using TSoft.InGame;
using TSoft.Managers;
using TSoft.UI.Core;

namespace TSoft.UI.Views.InGame
{
    public class FieldInfoView : ViewBase
    {
        public Action<float> OnDamaged;
        public Action<FieldController.FieldSlot> OnMonsterSpawn;
        
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
            txtName.text = data.monster.Data.Name;
            txtHp.text = (int)data.monster.GamePlay.GetAttr(GameplayAttr.Heart) + "";
        }

        private void UpdateOnRewardSpawn()
        {
            PopupContainer.Instance.ShowPopupUI(PopupContainer.PopupType.Store);
        }

        private void UpdateMonsterHp(float hp)
        {
            txtHp.text = (int)hp + "";
        }
    }
}
