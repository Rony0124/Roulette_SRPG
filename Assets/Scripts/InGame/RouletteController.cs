using System;
using TSoft.InGame.Roulette;
using TSoft.Managers;
using TSoft.UI.Core;
using TSoft.UI.Views;
using TSoft.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TSoft.InGame
{
    public class RouletteController : MonoBehaviour
    {
        private IRouletteSlotHandler[] rouletteSlots;
        
        private RouletteView view;
        private RouletteNumber currentRouletteNumber;

        private void Awake()
        {
            view = GetComponent<RouletteView>();
            rouletteSlots = GetComponentsInChildren(typeof(IRouletteSlotHandler)).ConvertToArray<IRouletteSlotHandler>();
            
            view.OnRollClick += RollRoulette;
        }

        private void RollRoulette()
        {
            Debug.Log("룰렛 돌려보자");
            
            var ranNum = Random.Range(0, 37);
            
            switch (ranNum)
            {
                case 0:
                    Debug.Log("보너스!");
                    break;
                case > 0 and <= 12:
                    GameManager.Instance.sectionType = SectionType.Equipment;
                    currentRouletteNumber.SectionType = SectionType.Equipment;
                    break;
                case > 12 and <= 24:
                    GameManager.Instance.sectionType = SectionType.Monster;
                    currentRouletteNumber.SectionType = SectionType.Monster;
                    break;
                case > 24 and <= 36:
                    GameManager.Instance.sectionType = SectionType.Quest;
                    currentRouletteNumber.SectionType = SectionType.Quest;
                    break;
            }
            
            currentRouletteNumber.Number = ranNum;

            var result = false;
            var currentBet = 0;
            foreach (var slot in rouletteSlots)
            {
                result |= slot.CheckMatch(currentRouletteNumber, ref currentBet);
            }
            
            /*if (result)
            {
                Debug.Log("배팅 성공");
            }
            else
            {
                Debug.Log("배팅 실패");
            }*/
            
            Debug.Log($"배팅 결과{result}");
            Debug.Log($"룰렛 숫자{ranNum}");
            Debug.Log($"bet money {currentBet}");
        }
    }
}
