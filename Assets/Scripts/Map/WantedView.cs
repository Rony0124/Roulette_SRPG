using System;
using TMPro;
using TSoft.Data.Registry;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace TSoft.Map
{
    public class WantedView : MonoBehaviour
    {
        public Action<MapNode> onGoClicked;
        public Action onBackClicked;
        
        [SerializeField] private Image icon;
        [SerializeField] private Image bg;
        [SerializeField] private SerializedDictionary<NodeType, Sprite> bgDictionary;
        [SerializeField] private TextMeshProUGUI bountyText;

        [SerializeField] private Button goButton;
        [SerializeField] private Button backButton;

        private MapNode mapNode;

        private void Awake()
        {
            goButton.onClick.AddListener(() => onGoClicked?.Invoke(mapNode));
            backButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        public void SetWanted(MapNode mapNode)
        {
            this.mapNode = mapNode;
            
            var blueprint = mapNode.Blueprint;
            if(!DataRegistry.Instance.MonsterRegistry.TryGetValue(blueprint.monsterId, out var monsterData))
                return;
            
            if (!bgDictionary.TryGetValue(blueprint.nodeType, out var bgImage))
                return;
            
            bg.sprite = bgImage;
            icon.sprite = monsterData.MonsterData.monsterMontage;
            
            var stageInfo = GameContext.Instance.currentStageInfo;
            bountyText.text = ((stageInfo.stage + stageInfo.round + 1) * 100 * (blueprint.nodeType == NodeType.Boss ? 1.5 : 1)).ToString();
        }
    }
}
