using System;
using UnityEngine;
using UnityEngine.Events;

namespace TSoft.InGame
{
    public class MonsterController : MonoBehaviour
    {
        public struct MonsterInfo
        {
            public string Name;
            public float Hp;
        }
        
        public static event Action<float> OnMonsterDamaged;
        public static event Action<MonsterInfo> OnMonsterSpawn;
        
        [SerializeField] private string monsterName;
        [SerializeField] private float hp;

        private MonsterInfo info;

        public UnityEvent<float> onDamage;
        public UnityEvent onDead;

        private void Awake()
        {
            onDamage.AddListener((p) => OnMonsterDamaged?.Invoke(p));
            onDamage.AddListener(OnDamage);
            onDead.AddListener(OnDead);

            info.Name = monsterName;
            info.Hp = hp;
        }

        private void Start()
        {
            OnMonsterSpawn?.Invoke(info);
        }

        public void TakeDamage(float damage)
        {
            hp = Mathf.Max(0, hp - damage);
            
            if (hp > 0)
            {
                onDamage?.Invoke(hp);
            }
            else
            {
                onDead?.Invoke();
            }
        }
        
        //test
        private void OnDamage(float hp)
        {
            Debug.Log("damaged");
            Debug.Log("remaining hp is " + hp);
        }

        private void OnDead()
        {
            Debug.Log("dead");
        }
    }
}
