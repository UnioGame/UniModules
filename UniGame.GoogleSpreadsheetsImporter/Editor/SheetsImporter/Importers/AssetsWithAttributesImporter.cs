namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Importers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.EditorTools.Editor.AssetOperations;
    using Extensions;
    using GoogleSpreadsheets.Runtime.Attributes;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [Serializable]
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.HideLabel]
    [Sirenix.OdinInspector.BoxGroup("Attributes Source")]
#endif
    [CreateAssetMenu(menuName = "UniGame/Google/Spreadsheet/Importers/AssetsWithAttributesImporter",fileName = nameof(AssetsWithAttributesImporter))]
    public class AssetsWithAttributesImporter :  BaseSpreadsheetImporter 
    {
        
        /// <summary>
        /// list of assets linked by attributes
        /// </summary>
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.TableList]
#endif
        public List<SheetSyncItem> assets = new List<SheetSyncItem>();
        
        public override IEnumerable<object> Load()
        {
            var attributeAssets = AssetEditorTools.
                GetAssetsWithAttribute<ScriptableObject,SpreadsheetTargetAttribute>();

            assets = attributeAssets.
                Select(x=> new SheetSyncItem() {
                    asset = x.Value,
                    sheetName = x.Attribute == null || x.Attribute.UseTypeName ?
                        x.Value.GetType().Name : 
                        x.Attribute.SheetName
                }).
                ToList();
            
            return assets.Select(x => x.asset).
                OfType<object>().
                ToList();
        }

        public override List<object> Import(SpreadsheetData spreadsheetData)
        {
            var result = new List<object>();
            foreach (var item in assets) {
                if(!spreadsheetData.HasSheet(item.sheetName) || item.asset == null)
                    continue;
                var asset = item.asset.
                    ApplySpreadsheetData(spreadsheetData,item.sheetName);
                result.Add(asset);
            }

            return result;
        }
    }
}