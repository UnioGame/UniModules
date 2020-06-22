namespace UniGreenModules.UniCore.EditorTools.Editor.AssetOperations
{
    public interface IEditorAssetResource
    {
        bool HasData<T>()
            where  T : class;

        T Load<T>()
            where  T : class;
    }
}