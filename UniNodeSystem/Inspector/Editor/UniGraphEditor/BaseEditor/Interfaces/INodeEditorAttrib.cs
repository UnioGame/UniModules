namespace UniGreenModules.UniNodeSystem.Inspector.Editor.BaseEditor.Interfaces
{
    using System;

    public interface INodeEditorAttribute {
        Type GetInspectedType();
    }
}