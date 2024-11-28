using System;
using TSoft.Managers;
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
        
        public MonsterInfo Info => info;

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
            var currentHp = info.Hp;
            currentHp = Mathf.Max(0, currentHp - damage);
            
            onDamage?.Invoke(currentHp);
            
            if (currentHp <= 0)
            {
                onDead?.Invoke();
            }
            
            info.Hp = currentHp;
        }
        
        //test
        private void OnDamage(float health)
        {
            Debug.Log("damaged");
            Debug.Log("remaining hp is " + health);
        }

        private void OnDead()
        {
            PopupManager.Instance.OpenPopup(PopupManager.PopupType.Win);
        }
    }
}
