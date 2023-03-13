namespace UniGame.LeoEcs.Bootstrap.Runtime.Config
{
    using System.Collections;
#if UNITY_EDITOR
    using UniModules.Editor;
#endif
    
    public static class LeoEcsUpdateQueueIds
    {

        public static IEnumerable GetUpdateIds()
        {
#if UNITY_EDITOR

            var map = AssetEditorTools.GetAsset<LeoEcsUpdateMapAsset>();
            if(map == null) yield break;
            foreach (var updateQueue in map.updateQueue)
            {
                yield return updateQueue.OrderId;
            }
#endif
            yield break;
        }
        

    }
}