namespace UniGreenModules.UniNodeSystem.Runtime.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Runtime;
    using UnityEngine;

    public interface IUniGraph : IUniNode, IDisposable
    {

        GameObject AssetInstance { get; }

        IReadOnlyList<IGraphPortNode> OutputsPorts { get; }

        IReadOnlyList<IGraphPortNode> InputsPorts { get; }

        IReadOnlyList<INode> Nodes { get; }
        
    }
    
}