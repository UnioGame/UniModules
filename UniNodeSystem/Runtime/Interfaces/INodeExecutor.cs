using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine.Nodes
{
    public interface INodeExecutor<in TContext>
    {
        
        void Execute(UniGraphNode node, TContext context);

        void Stop(UniGraphNode node);
        
    }
}