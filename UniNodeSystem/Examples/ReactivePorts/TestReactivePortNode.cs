using UnityEngine;

namespace Tests.ReactivePorts
{
    using UniGreenModules.UniNodeSystem.Runtime;
    using UniRx;
    
    public class TestReactivePortNode : UniNode
    {
        protected override void OnNodeInitialize()
        {
            
            Input.Receive<string>().
                Subscribe(x => Debug.Log($"INPUT DATA {x}")).
                AddTo(this);

            Input.Connect(Output);
            
            Output.Receive<string>().
                Subscribe(x => Debug.Log($"OUTPUT DATA {x}")).
                AddTo(this);
            
        }
    }
}
