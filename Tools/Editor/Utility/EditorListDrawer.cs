using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace UniEditorTools
{

    public class ListDrawerOptions<TItem>
    {
        
        public bool DrawAddButton = true;
        public bool DrawDeleteButton = true;
        public Func<TItem> ItemFactory;

        public Action<TItem, int> DrawItem;
        
        public Action<TItem, int> OnItemDraw;
        public Action<TItem, int> OnItemAdded;
        public Action<TItem, int> OnItemDeleted;
        
    }


    public static class EditorListDrawer
    {

        public static void DrawListWithOptions<TItem>(
            IList<TItem> items,
            ListDrawerOptions<TItem> options = null)
        {

            for (int i = 0; i < items.Count; i++)
            {
               
            }
            
        }

        public static void DrawItem<TItem>()
        {
            
        }

        public static void DrawAddItem()
        {
            
        }

        public static void DrawDeleteItem()
        {
            
        }

    }
}