using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Editor.BaseEditor;
using UnityEngine;

namespace UniNodeSystemEditor
{
    using UniGreenModules.UniNodeSystem.Runtime.Runtime;

    public interface INodeEditor : IEditorItem
    {
        int GetWidth();
        Color GetTint();
        GUIStyle GetBodyStyle();
        void Rename(string newName);

        bool IsSelected();
        
        UniBaseNode Target { get; }

    }
}