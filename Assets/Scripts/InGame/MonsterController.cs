using System;
using UnityEngine;
using UnityEngine.Events;

namespace TSoft.InGame
{
    public class MonsterController : MonoBehaviour
    {
        [SerializeField] private int hp;

        public UnityEvent onDamage;
        public UnityEvent onDead;

        private void Awake()
        {
            onDamage.AddListener(OnDamage);
            onDead.AddListener(OnDead);
        }

        public void TakeDamage(int damage)
        {
            hp = Mathf.Max(0, hp - damage);
            
            if (hp > 0)
            {
                onDamage?.Invoke();
            }
            else
            {
                onDead?.Invoke();
            }
        }
        
        //test
        private void OnDamage()
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