using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine.Nodes
{
    public interface INodeExecutor<in TContext>
    {
        
        void Execute(UniNode node, TContext context);

        void Stop(UniNode node, TContext context);
        
    }
}