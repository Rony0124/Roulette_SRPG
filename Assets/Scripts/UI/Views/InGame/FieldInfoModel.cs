using TSoft;
using TSoft.InGame;
using TSoft.Utils;
using UnityEngine;

public class FieldInfoModel : ModelBase
{
    [SerializeField] private CombatController combat;
    
    public CombatController Combat => combat;

    private void OnDestroy()
    {
        combat.OnMonsterSpawn = null;
        combat.Monster.GamePlay.GetAttrVar(GameplayAttr.Heart)?.Dispose();
    }
}
