using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using TSoft.InGame;
using TSoft.InGame.GamePlaySystem;
using TSoft.Managers.Event;
using UnityEngine;
using PlayerController = TSoft.InGame.Player.PlayerController;

namespace TSoft.UI.Views.InGame
{
    public class AttackInfoViewModel : ViewModelBase
    {
        private AttackInfoView View => view as AttackInfoView;
        private AttackInfoModel Model => model as AttackInfoModel;

        [SerializeField] 
        private TextObjectPool textPool;
        
        [Serializable]
        public struct AttrEffectInfo
        {
            public GameplayAttr attrType;
            public ModifierOpType opType;
            public Transform targetTransform;
        }
        
        [SerializeField][TableList]
        private List<AttrEffectInfo> effectInfos;

        private MMFeedback feedback;
        
        void Start()
        {
            EventManager.Instance.DmgAdderEvent.RemoveAllListener();
            
            Model.Player.onPatternSelected += OnCombinationSelected;

            /*foreach (var info in effectInfos)
            {
                void UnityAction(AppliedModifier appliedModifier)
                {
                    if (appliedModifier.attrType == info.attrType)
                    {
                        if (Mathf.Abs(appliedModifier.modifier.Add) > float.Epsilon && info.opType == ModifierOpType.Add)
                        {
                            ShowFloatingText(info.targetTransform.position, appliedModifier.modifier.Add, info.attrType);
                        }

                        if (Mathf.Abs(appliedModifier.modifier.Multiply - 1.0f) > float.Epsilon && info.opType == ModifierOpType.Multiply)
                        {
                            ShowFloatingText(info.targetTransform.position, appliedModifier.modifier.Multiply, info.attrType);
                        }
                    }
                }
                
                EventManager.Instance.DmgAdderEvent.AddEvent(UnityAction);
            }*/
        }

        private void OnCombinationSelected(PlayerController.CardPattern pattern)
        {
            View.SetCombinationText(pattern.PatternType);
        }

        private void ShowFloatingText(Vector2 position, float value, GameplayAttr attr)
        {
            var floatingText = textPool.GetPoolObj();
            floatingText.SetText(value.ToString(), attr == GameplayAttr.BasicAttackPower ? Color.blue : Color.red);
            floatingText.gameObject.transform.position = position;
            floatingText.gameObject.SetActive(true);
        }
    }
}
