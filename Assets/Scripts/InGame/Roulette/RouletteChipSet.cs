using System;
using System.Collections.Generic;
using System.ComponentModel;
using TSoft.Utils;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;

namespace TSoft.InGame.Roulette
{
    public class RouletteChipSet : MonoBehaviour
    {
        [SerializeField] private GameObject[] chipPrefab;
        [SerializeField] private Transform chipDragParent;
        
        private ObservableList<RouletteChip> chips;
        
        private int chipIndex;
        
        private void Awake()
        {
            chips = new ObservableList<RouletteChip>();
            chips.ListChanged += OnChipChanged;
        }

        private void Start()
        {
            CreateChip();
        }

        private void OnChipChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                chips[e.NewIndex].OnDragBegin.AddListener(OnDrag);
            }
        }

        private void OnDrag(RouletteChip chip)
        {
            CreateChip();
        }

        private void CreateChip()
        {
            var obj = Instantiate(chipPrefab[chipIndex], transform).GetComponent<RouletteChip>();
            obj.onDragParent = chipDragParent;
            chips.Add(obj);    
        }

        private void DestroyChipOnChipSet()
        {
            var chip = GetComponentInChildren<RouletteChip>();
            Destroy(chip.gameObject);

            chips.Remove(chip);
        }

        public void ChangeChip(int index)
        {
            chipIndex = Math.Clamp(index + chipIndex, 0, chipPrefab.Length - 1);
            DestroyChipOnChipSet();
            CreateChip();
        }
    }
}
