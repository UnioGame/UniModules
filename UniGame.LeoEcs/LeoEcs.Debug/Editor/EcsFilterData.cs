namespace UniGame.LeoEcs.Debug.Editor
{
    using System;
    using System.Collections.Generic;
    using Leopotam.EcsLite;
    using Object = UnityEngine.Object;

    [Serializable]
    public class EcsFilterData
    {
        public static EcsFilterData NoneFilterData = new EcsFilterData()
        {
            message = "Empty Result",
            type = ResultType.None,
        };

        public EcsWorld world;
        public string filter;
        public string errorMessage;
        public string message;
        public ResultType type;
        public List<int> entities = new List<int>();
    }
}