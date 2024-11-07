using System;
using System.Linq;
using TSoft.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

namespace TSoft.InGame.Roulette
{
    [Serializable]
    public struct RouletteNumber
    {
        public int Number;
        //red이면 true, black이면 0
        public bool Color;
        public SectionType SectionType;
    }
    
    public abstract class RouletteSlotBase : MonoBehaviour, IRouletteSlotHandler, IDropHandler
    {
        GameObject Icon()
        {
            return transform.childCount > 1 ? transform.GetChild(1).gameObject : null;
        }

        public int Wager => CalcWager();

        public virtual bool IsBet() => GetComponentsInChildren<RouletteChip>().Length > 0;

        public abstract bool CheckMatch(RouletteNumber rouletteNum, ref int bet);
        
        public void OnDrop(PointerEventData eventData)
        {
            if (Icon() == null)
            {
                RouletteChip.DraggedIcon.transform.SetParent(transform);
                RouletteChip.DraggedIcon.transform.position = transform.position;
            }
        }

        private int CalcWager()
        {
            var chips = GetComponentsInChildren<RouletteChip>();

            return chips.Sum(chip => chip.wager);
        }
    }
}
