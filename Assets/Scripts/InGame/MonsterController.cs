using System;
using TSoft.Data.Monster;
using TSoft.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace TSoft.InGame
{
    public class MonsterController : MonoBehaviour
    {
        public static event Action<float> OnMonsterDamaged;
        public static event Action<MonsterData> OnMonsterSpawn;
        
        private MonsterData data;
        
        public MonsterData Data
        {
            get => data;
            set => data = value;
        }

        public UnityEvent<float> onDamage;
        public UnityEvent onDead;

        private void Awake()
        {
            onDamage.AddListener((p) => OnMonsterDamaged?.Invoke(p));
            onDamage.AddListener(OnDamage);
            onDead.AddListener(OnDead);
        }

        private void Start()
        {
            OnMonsterSpawn?.Invoke(data);
        }

        public void TakeDamage(float damage)
        {
            var currentHp = data.Hp;
            currentHp = Mathf.Max(0, currentHp - damage);
            
            onDamage?.Invoke(currentHp);
            
            if (currentHp <= 0)
            {
                onDead?.Invoke();
            }
            
            data.Hp = currentHp;
        }
        
        //test
        private void OnDamage(float health)
        {
            Debug.Log("damaged");
            Debug.Log("remaining hp is " + health);
        }

        private void OnDead()
        {
            PopupContainer.Instance.ShowPopupUI(PopupContainer.PopupType.Win);
        }
    }
}
