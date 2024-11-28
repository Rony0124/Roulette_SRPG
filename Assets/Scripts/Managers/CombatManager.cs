using TSoft.Core;
using TSoft.InGame;
using UnityEngine;

namespace TSoft.Managers
{
    public class CombatManager : Singleton<CombatManager>
    {
        [SerializeField] private GameObject monsterPrefab;

        private GameObject currentMonster;
        
        public MonsterController CurrentMonster => currentMonster.GetComponent<MonsterController>();
       
        void Awake()
        {
            currentMonster = Instantiate(monsterPrefab);
        }

        public void Combat(float damage)
        {
            CurrentMonster.TakeDamage(damage);
        }
    }
}
