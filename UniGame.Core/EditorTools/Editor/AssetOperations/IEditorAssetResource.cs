namespace UniModules.UniGame.Core.EditorTools.Editor.AssetOperations
{
    public interface IEditorAssetResource
    {
        bool HasData<T>()
            where  T : class;

        T Load<T>()
            where  T : class;
    }
}