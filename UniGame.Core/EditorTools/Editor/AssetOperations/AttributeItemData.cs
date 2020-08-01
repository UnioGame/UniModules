namespace UniGreenModules.UniCore.EditorTools.Editor.AssetOperations
{
    using System;

    public static partial class AssetEditorTools
    {
        public struct AttributeItemData<TValue,TAttribute>
            where TAttribute : Attribute
        {
            public TValue     Value;
            public TAttribute Attribute;
        }
    }
}