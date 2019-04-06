using System.Collections.Generic;
using Modules.UniTools.UniNodeSystem.Editor.BaseEditor;
using UniNodeSystem;
using UnityEngine;

namespace UniNodeSystemEditor
{
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