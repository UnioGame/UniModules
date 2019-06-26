namespace UniGreenModules.UniNodeSystem.Runtime
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using UniCore.Runtime.Extension;
    using UniCore.Runtime.ObjectPool;
    using UniCore.Runtime.ObjectPool.Interfaces;

    public class NodePortHandler<TValue> : IDespawnable
    {
        private List<Action<IUniNode,IPortValue, TValue>> onNextActions = 
            new List<Action<IUniNode,IPortValue, TValue>>();
        
        private List<Action<IUniNode,IPortValue>> onCompleteActions = 
            new List<Action<IUniNode,IPortValue>>();

        public void Register(IUniNode node, IPortValue port, bool oneShot = false)
        {
            //create poolable obsever
            var observer = this.CreateRecycleObserver<TValue>(
                    x => OnNext(node,port,x),
                    () => OnCompleted(node,port));

            node.LifeTime.AddCleanUpAction(() => observer.MakeDespawn());
            
            node.RegisterPortHandler(port,observer,oneShot);
            
        }

        public void AddCommand(Action<IUniNode,IPortValue, TValue> onNext,
            Action<IUniNode,IPortValue> onComplete = null)
        {
            onNextActions.Add(onNext);
            if(onComplete !=null)
                onCompleteActions.Add(onComplete);
        }

        private void OnCompleted(IUniNode node, IPortValue port)
        {
            for (var i = 0; i < onNextActions.Count; i++) {
                onCompleteActions[i].Invoke(node,port);
            }
        }

        private void OnNext(IUniNode node, IPortValue port,TValue value)
        {
            for (var i = 0; i < onNextActions.Count; i++) {
                onNextActions[i].Invoke(node,port,value);
            }
        }

        public void Release()
        {
            onNextActions.Clear();
            onCompleteActions.Clear();
        }
        
        public void MakeDespawn()
        {
            Release();
            this.Despawn();
        }
    }
}
