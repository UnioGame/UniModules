namespace UniGame.LeoEcs.Debug.Editor
{
    using System;
    using UnityEngine;

    [Serializable]
    public class CheckEditorStatusFilter : IEcsWorldSearchFilter
    {
        public const string notInPlaymodeMessage = "Filtering work only in Play Mode";

        public EcsFilterData Execute(EcsFilterData filterData)
        {
            if (!Application.isPlaying)
            {
                filterData.errorMessage = notInPlaymodeMessage;
                filterData.type = ResultType.Error;
            }

            return filterData;
        }
    }
}