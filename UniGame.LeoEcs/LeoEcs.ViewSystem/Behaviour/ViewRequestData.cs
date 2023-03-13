namespace UniGame.LeoEcs.ViewSystem.Components
{
    using System;
    using UniModules.UniGame.UiSystem.Runtime;
    using UnityEngine;

    [Serializable]
    public class ViewRequestData
    {
        public ViewType LayoutType = ViewType.Window;
        public string Tag = string.Empty;
        public Transform Parent = null;
        public string ViewName = string.Empty;
        public bool StayWorld = false;
    }
}