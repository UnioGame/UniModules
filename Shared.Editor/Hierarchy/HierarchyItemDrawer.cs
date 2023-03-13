namespace UniGame.Shared.Editor.Hierarchy
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    public static class HierarchyItemDrawer
    {
        private static Action<HierarchyItem> onStopped;

        private static List<Listener> listeners;
        private static bool isDirty;
        private static bool isStopped;
        private static HierarchyItem item;

        static HierarchyItemDrawer()
        {
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyItemGUI;
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItemGUI;
            
            item = new HierarchyItem();
        }

        private static void OnHierarchyItemGUI(int id, Rect rect)
        {
            if (listeners == null) return;

            if (isDirty)
            {
                listeners.Sort(delegate (Listener i1, Listener i2)
                {
                    if (Math.Abs(i1.Order - i2.Order) < float.Epsilon) 
                        return 0;
                    
                    if (i1.Order > i2.Order) 
                        return 1;
                    
                    return -1;
                });

                isDirty = false;
            }

            item.Set(id, rect);

            foreach (var listener in listeners)
            {
                if (listener.Action != null)
                {
                    try
                    {
                        listener.Action(item);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                }
                if (isStopped) break;
            }

            isStopped = false;
        }

        public static void Register(string id, Action<HierarchyItem> action, float order = 0)
        {
            if (string.IsNullOrEmpty(id)) 
                throw new Exception("ID cannot be empty");

            listeners ??= new List<Listener>();
            
            var hash = id.GetHashCode();
            foreach (var listener in listeners)
            {
                if (listener.Hash != hash || listener.ID != id)
                    continue;

                listener.Action = action;
                listener.Order = order;
                return;
            }
            
            listeners.Add(new Listener
            {
                ID = id,
                Hash = hash,
                Action = action,
                Order = order
            });

            isDirty = true;
        }

        public static void StopCurrentRowGUI()
        {
            isStopped = true;
            onStopped?.Invoke(item);
        }

        private class Listener
        {
            public int Hash;
            public string ID;
            public Action<HierarchyItem> Action;
            public float Order;
        }
    }
}