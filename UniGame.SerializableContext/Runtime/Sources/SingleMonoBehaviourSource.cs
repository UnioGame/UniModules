namespace UniModules.UniGame.SerializableContext.Runtime.Components
{
    using Context.Runtime.Abstract;
    using Cysharp.Threading.Tasks;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    
    using UnityEngine;

    public class SingleMonoBehaviourSource<TObject> : SingleMonoBehaviourSource<TObject,TObject>
        where TObject : MonoBehaviour
    {
    }

    public class SingleMonoBehaviourSource<TObject, TApi> : AsyncContextDataSource
        where TObject : MonoBehaviour, TApi 
        where TApi : class
    {
        [SerializeField] 
        private TObject _prefab;

        private static TObject _instance;

        public TObject Asset => _instance;

        public override async UniTask<IContext> RegisterAsync(IContext context)
        {
            if (_instance == null || _instance.gameObject == null)
            {
                var go = GameObject.Instantiate(_prefab.gameObject);
                _instance = go.GetComponent<TObject>();
            }

            var instanceAsset = await OnInstanceReceive(_instance,context);
            
            context.Publish<TApi>(instanceAsset);

            return context;
        }
        
        
        protected virtual async UniTask<TApi> OnInstanceReceive(TObject asset,IContext context)
        {
            return asset;
        }



    }
}