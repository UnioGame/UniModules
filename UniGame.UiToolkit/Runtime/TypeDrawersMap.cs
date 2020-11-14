using System.Collections.Generic;
using UniGame.Core.EditorTools.Editor.UiElements;
using UnityEngine;

namespace UniModules.UniGame.UiElements.Runtime
{
    public class TypeDrawersMap : MonoBehaviour
    {
        public List<IUiElementsTypeDrawer> Drawers = new List<IUiElementsTypeDrawer>();
    }
}
