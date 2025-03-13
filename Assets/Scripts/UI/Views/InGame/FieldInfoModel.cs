using TSoft;
using TSoft.InGame;
using TSoft.Utils;
using UnityEngine;

public class FieldInfoModel : ModelBase
{
    [SerializeField] private CombatController combat;
    
    public CombatController Combat => combat;
    public ObservableVar<float> monsterHp => combat.CurrentMonster?.GamePlay?.GetAttrVar(GameplayAttr.Heart);

    private void OnDestroy()
    {
        combat.OnMonsterSpawn = null;
        monsterHp?.Dispose();
    }
}
