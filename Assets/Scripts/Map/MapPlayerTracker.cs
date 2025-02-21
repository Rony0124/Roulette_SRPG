using System;
using System.Linq;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace TSoft.Map
{
    public class MapPlayerTracker : MonoBehaviour
    {
        public bool lockAfterSelecting = false;
        public float enterNodeDelay = 1f;
        public MapManager mapManager;
        public MapView view;
        public WantedView wantedView;
        [SerializeField] private MMFeedbacks OpenWantedFeedback;
        public static MapPlayerTracker Instance;

        public bool Locked { get; set; }

        private void Awake()
        {
            Instance = this;
            wantedView.onGoClicked += CheckSendPlayerToNode;
        }

        public void SelectNode(MapNode mapNode)
        {
            if (Locked) 
                return;

            Debug.Log("Selected node: " + mapNode.Node.point);

            CheckSendPlayerToNode(mapNode);
        }

        private void CheckSendPlayerToNode(MapNode mapNode)
        {
            if (mapManager.CurrentMap.path.Count == 0)
            {
                // player has not selected the node yet, he can select any of the nodes with y = 0
                if (mapNode.Node.point.y == 0)
                    CheckNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
            else
            {
                Vector2Int currentPoint = mapManager.CurrentMap.path[mapManager.CurrentMap.path.Count - 1];
                Node currentNode = mapManager.CurrentMap.GetNode(currentPoint);

                if (currentNode != null && currentNode.outgoing.Any(point => point.Equals(mapNode.Node.point)))
                    CheckNode(mapNode);
                else
                    PlayWarningThatNodeCannotBeAccessed();
            }
        }

        private void CheckNode(MapNode mapNode)
        {
            if (mapNode.Blueprint.nodeType is NodeType.EliteEnemy or NodeType.MinorEnemy or NodeType.Boss)
            {
                wantedView.SetWanted(mapNode);
                OpenWantedFeedback.PlayFeedbacks();
            }
            else
            {
                SendPlayerToNode(mapNode);
            }
        }

        private void SendPlayerToNode(MapNode mapNode)
        {
            Locked = lockAfterSelecting;
            mapManager.CurrentMap.path.Add(mapNode.Node.point);
            mapManager.SaveMap();
            view.SetAttainableNodes();
            view.SetLineColors();
            mapNode.ShowSwirlAnimation();

            DOTween.Sequence().AppendInterval(enterNodeDelay).OnComplete(() => EnterNode(mapNode));
        }

        private static void EnterNode(MapNode mapNode)
        {
            Debug.Log("Entering node: " + mapNode.Node.blueprintName + " of type: " + mapNode.Node.nodeType);

            GameContext.Instance.CurrentNode = mapNode.Blueprint;
        }

        private void PlayWarningThatNodeCannotBeAccessed()
        {
            Debug.Log("Selected node cannot be accessed");
        }
    }
}