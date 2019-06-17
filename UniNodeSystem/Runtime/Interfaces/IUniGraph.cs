namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Runtime;
    using UnityEngine;

    public interface IUniGraph : IUniNode, IDisposable
    {

        GameObject AssetInstance { get; }

        IReadOnlyList<IGraphPortNode> GraphOuputs { get; }

        IReadOnlyList<IGraphPortNode> GraphInputs { get; }

        IReadOnlyList<INode> Nodes { get; }
        
    }
    
}