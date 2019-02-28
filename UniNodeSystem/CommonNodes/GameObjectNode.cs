using System.Collections;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UnityEngine;
using XNode;

namespace UniStateMachine.CommonNodes
{
    
    public class GameObjectNode : AssetNode<GameObject>
    {
        private string _optionsPortName;

        public bool CreateInstance;
       
        public ObjectInstanceData Options;
                       
        //public UnityEvent Action;
        
        
        protected override IEnumerator ExecuteState(IContext context)
        {
            var target = CreateTarget(context);

            var lifeTime = GetLifeTime(context);
            lifeTime.AddCleanUpAction(() => { RemoveTarget(target, context); });
            
            return base.ExecuteState(context);
        }

        protected override void OnUpdatePortsCache()
        {
            _optionsPortName = nameof(Options);
            //option input values port
            this.UpdatePortValue(_optionsPortName, PortIO.Input);
            base.OnUpdatePortsCache();
        }

        private GameObject CreateTarget(IContext context)
        {
            
            if (!CreateInstance) return Target;
            if (!Target) return Target;

            var optionsValue = GetPortValue(_optionsPortName);
            var options = optionsValue.HasValue<ObjectInstanceData>(context) ? 
                optionsValue.Get<ObjectInstanceData>(context):
                Options;
            
            var target = ObjectPool.Spawn(Target,options.Position,
                Quaternion.identity,options.Parent,
                options.StayAtWorld);
            
            if (options.Immortal)
            {
                DontDestroyOnLoad(target);
            }

            return target;
            
        }

        private void RemoveTarget(GameObject target,IContext context)
        {

            if (!CreateInstance || !target) return;
            
            ObjectPool.Despawn(target);

        }
        
        
    }

}

