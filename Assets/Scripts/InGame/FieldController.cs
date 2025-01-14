using TSoft.Data;
using TSoft.Data.Registry;
using TSoft.Managers;
using TSoft.UI.Views.InGame;
using TSoft.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TSoft.InGame
{
    public class FieldController : MonoBehaviour
    {
        private BackgroundView bgView;

        private BackgroundView BgView
        {
            get
            {
                if (bgView == null)
                {
                    bgView = FindObjectOfType<BackgroundView>();   
                }

                return bgView;
            }
        } 
        
        private FieldInfoView infoView;

        private FieldInfoView InfoView
        {
            get
            {
                if (infoView == null)
                {
                    infoView = FindObjectOfType<FieldInfoView>();   
                }

                return infoView;
            }
        } 
        
        private CombatController.CycleInfo currentCycle;
        public CombatController.CycleInfo CurrentCycle
        {
            get => currentCycle;
            set
            {
                var val = value;

                OnCurrentRoundChanged(currentCycle, val);
                
                currentCycle = val;
            } 
        }
        
        private MonsterController currentMonster;

        public MonsterController CurrentMonster
        {
            get => currentMonster;
            set
            {
                InfoView.OnMonsterSpawn?.Invoke(currentMonster);
                currentMonster = value;
            }
        }

        private DataRegistryIdSO[] monsterIds;
        private DataRegistryIdSO bossId;

        public void SpawnField(Data.Stage.StageData stageData)
        {
            monsterIds = stageData.monsterIds;
            bossId = stageData.bossId;
            
            infoView = FindObjectOfType<FieldInfoView>();
            bgView = FindObjectOfType<BackgroundView>();
            
            bgView.SetBackground(stageData.background);
        }

        private void OnCurrentRoundChanged(CombatController.CycleInfo oldVal, CombatController.CycleInfo newVal)
        {
            if (oldVal.Round < newVal.Round)
            {
                if (newVal.Round == Define.RewardSlot)
                {
                    PopupContainer.Instance.ShowPopupUI(PopupContainer.PopupType.Store);
                }
                else if (newVal.Round == Define.BossSlot)
                {
                    currentMonster = SpawnMonster(bossId);
                    var currentHp = currentMonster.GamePlay.GetAttr(GameplayAttr.Heart);
                    currentHp *= (newVal.Stage+ 1) * 1.5f;
                    currentMonster.GamePlay.SetAttr(GameplayAttr.Heart, currentHp);
                    
                    currentMonster.onDamaged = InfoView.OnDamaged;
                }
                else
                {
                    var ranIndex = Random.Range(0, monsterIds.Length);
                    currentMonster = SpawnMonster(monsterIds[ranIndex]);
                    var currentHp = currentMonster.GamePlay.GetAttr(GameplayAttr.Heart);
                    Debug.Log("currentHp hp : " + currentHp );
                    currentHp *= (newVal.Stage + 1);
                    currentMonster.GamePlay.SetAttr(GameplayAttr.Heart, currentHp);
                    
                    currentMonster.onDamaged = InfoView.OnDamaged;
                }
            }
        }

        private MonsterController SpawnMonster(DataRegistryIdSO monsterId)
        {
            MonsterController monster = null;
            if (DataRegistry.Instance.MonsterRegistry.TryGetValue(monsterId, out var monsterDataSo))
            {
                var pos = Vector3.zero;
                monster = monsterDataSo.SpawnMonster(transform, pos);
            }

            return monster;
        }
    }
}
