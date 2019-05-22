using System;

namespace UniStateMachine.Nodes
{
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public struct TypeDataChanged
    {
        public Type ValueType;
        public ITypeData Container;
    }
}