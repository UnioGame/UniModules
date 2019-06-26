namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UniCore.Runtime.Interfaces;
    using UniStateMachine.Runtime.Interfaces;
    using UniTools.UniRoutine.Runtime;

    public interface IUniNode : 
        INode,
        IState,
        INamedItem
    {

        IReadOnlyList<IPortValue> PortValues { get; }

        void UpdatePortsCache();

        /// <summary>
        /// register port value action
        /// </summary>
        /// <param name="portValue">target port</param>
        /// <param name="observer">on next action</param>
        /// <param name="oneShot">don't resub on complete</param>
        void RegisterPortHandler<TValue>(
            IPortValue portValue,
            IObserver<TValue> observer,
            bool oneShot = false);
    }
}