namespace UniGreenModules.UniNodeSystem.Inspector.Editor.BaseEditor.Interfaces
{
    using Runtime.Core;
    using UniCore.EditorTools.Editor.Interfaces;
    using UnityEngine;

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