namespace UniGame.Shared.Editor.Hierarchy
{
    using UnityEditor;
    using UnityEngine;

    public class HierarchyItem
    {
        public Rect Rect;
        public GameObject GameObject;

        public void Set(int id, Rect rect)
        {
            Rect = rect;

            var target = EditorUtility.InstanceIDToObject(id);
            GameObject = target as GameObject;
        }
    }
}