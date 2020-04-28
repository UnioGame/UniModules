namespace UniModules.UniGame.EditorExtensions.Runtime.Attributes
{
    using System;

    public interface IAssetReferenceDropDownInfo
    {
        Type BaseType { get; }
        bool FoldOutOpen { get; }
    }
}