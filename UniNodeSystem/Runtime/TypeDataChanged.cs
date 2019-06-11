namespace UniGreenModules.UniNodeSystem.Runtime
{
    using System;
    using UniCore.Runtime.Interfaces;

    public struct TypeDataChanged
    {
        public Type ValueType;
        public ITypeData Container;
    }
}