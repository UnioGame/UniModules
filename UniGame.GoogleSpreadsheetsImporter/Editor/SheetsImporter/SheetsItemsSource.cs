namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GoogleSpreadsheets.Runtime.Attributes;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UnityEngine;

    [Serializable]
    public class SheetsItemsSource
    {
        /// <summary>
        /// list of assets linked by attributes
        /// </summary>
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.TableList]
#endif
        public List<SheetSyncItem> assets = new List<SheetSyncItem>();

        public List<SheetSyncItem> Reload()
        {
            assets.Clear();
            assets.AddRange(LoadSyncScriptableObjects());
            return assets;
        }

        private List<SheetSyncItem> LoadSyncScriptableObjects()
        {
            var assets = AssetEditorTools.
                GetAssetsWithAttribute<ScriptableObject,SpreadsheetTargetAttribute>();
            return assets.
                Select(x=> new SheetSyncItem() {
                    asset = x.Value,
                    sheetName = x.Attribute == null || x.Attribute.UseTypeName ?
                        x.Value.GetType().Name : 
                        x.Attribute.SheetName
                }).
                ToList();
        }
        
    }
}