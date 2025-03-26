using InGame;
using TSoft.InGame;

namespace TSoft.UI.Views.InGame
{
   public class FieldInfoViewModel : ViewModelBase
   {
      private FieldInfoView View => view as FieldInfoView;
      private FieldInfoModel Model => model as FieldInfoModel;

      private void Start()
      {
         Model.Combat.OnMonsterSpawn += OnMonsterSpawn;
      }

      private void OnMonsterSpawn(MonsterController monsterController)
      {
         var heart = monsterController.GamePlay.GetAttr(GameplayAttr.Heart);
         var maxHeart = monsterController.GamePlay.GetAttr(GameplayAttr.MaxHeart);
      
         View.SetMonsterNameText(monsterController.Data.name);
         View.SetMonsterHpText(heart, maxHeart);

         monsterController.GamePlay.GetAttrVar(GameplayAttr.Heart).OnValueChanged += OnMonsterDamaged;
      }

      private void OnMonsterDamaged(float oldValue, float newValue)
      {
         var maxHp = Model.Combat.CurrentMonster.GamePlay.GetAttr(GameplayAttr.MaxHeart);
         View.SetMonsterHpText(newValue, maxHp);
      }
   }
}
