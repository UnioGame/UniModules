using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace UnityTools.UniNodeEditor.Connections
{
    public interface INodeModuleAdapter
    {
        IReadOnlyList<string> Ports { get; }
        string name { get; set; }
        HideFlags hideFlags { get; set; }
        void BindValue(string key,IContextData<IContext> value);
        void Bind(IContext context);
        void Update( IContext context);
        void Release(IContext context);
        void SetDirty();
        int GetInstanceID();
        int GetHashCode();
        bool Equals(object other);
        string ToString();
    }
}