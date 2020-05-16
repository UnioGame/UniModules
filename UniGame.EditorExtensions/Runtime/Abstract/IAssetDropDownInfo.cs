namespace UniModules.UniGame.EditorExtensions.Runtime.Attributes
{
    using System;

    public interface IAssetDropDownInfo
    {
        Type BaseType { get; }
        bool FoldOutOpen { get; }
    }
}