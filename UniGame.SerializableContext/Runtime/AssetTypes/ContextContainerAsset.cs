namespace UniGreenModules.UniGame.SerializableContext.Runtime.AssetTypes
{
    using UniContextData.Runtime.Entities;
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Assets/ContextContainerAsset", fileName = nameof(ContextContainerAsset))]
    public class ContextContainerAsset :
        TypeContainerAssetSource<EntityContext, IContext>
    {
        [SerializeField]
        private bool _createDefaultOnLoad = false;

        protected override void OnActivate()
        {
            base.OnActivate();
            if (_createDefaultOnLoad) {
                SetValue(new EntityContext());
            }
        }
    }
}
