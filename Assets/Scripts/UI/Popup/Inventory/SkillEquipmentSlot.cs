using TMPro;
using TSoft.Data.Registry;
using TSoft.InGame;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Popup.Inventory
{
    public class SkillEquipmentSlot : MonoBehaviour
    {
        [SerializeField] private Image slotBackgroundImage;
        [SerializeField] private Image slotImage;
        [SerializeField] private TMP_Text skillText;
        [SerializeField] private CardPatternType cardPatternType;
        
        public CardPatternType CardPatternType => cardPatternType;
        
        public void UpdateIcon()
        {
            if (GameSave.Instance.SkillEquippedDictionary.TryGetValue((int)cardPatternType, out var skillId))
            {
                var skill = DataRegistry.Instance.SkillRegistry.Get(skillId);
                slotImage.sprite = skill.image;
            }
            else
            {
                slotImage.sprite = null;
            }
        }
    }
}
