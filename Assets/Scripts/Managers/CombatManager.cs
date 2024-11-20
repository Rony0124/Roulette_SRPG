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
       
        void Start()
        {
            currentMonster = Instantiate(monsterPrefab);
        }

        public void Combat(int damage)
        {
            CurrentMonster.TakeDamage(damage);
        }
    }
}
