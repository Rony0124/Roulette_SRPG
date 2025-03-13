using TSoft.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TSoft.UI.Views.Store
{
    public class StoreView : ViewBase
    {
        private enum TransformParent
        {
            Artifact,
            Joker,
            Skill
        }
        
        private enum StoreButton
        {
            BackButton
        }
        
        public Transform artifactDisplayParent { get; private set; }
        public Transform jokerDisplayParent { get; private set; }
        public Transform skillDisplayParent { get; private set; }
        public Button backButton { get; set; }

        private void Awake()
        {
            Bind<Transform>(typeof(TransformParent));
            Bind<Button>(typeof(StoreButton));
            
            artifactDisplayParent = Get<Transform>((int)TransformParent.Artifact);
            jokerDisplayParent = Get<Transform>((int)TransformParent.Joker);
            skillDisplayParent = Get<Transform>((int)TransformParent.Skill);
            backButton = Get<Button>((int)StoreButton.BackButton);
        }
    }
}
