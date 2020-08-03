namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GoogleSpreadsheets.Runtime.Attributes;
    using UniGreenModules.UniCore.EditorTools.Editor.AssetOperations;
    using UnityEngine;

    [Serializable]
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.HideLabel]
    [Sirenix.OdinInspector.BoxGroup("Attributes Source")]
#endif
    [CreateAssetMenu(menuName = "UniGame/Google/Spreadsheet/Importers/AssetsWithAttributesImporter",fileName = nameof(AssetsWithAttributesImporter))]
    public class AssetsWithAttributesImporter :  SpreadsheetsSyncAssetsImporter 
    {

        
        /// <summary>
        /// list of assets linked by attributes
        /// </summary>
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.TableList]
#endif
        public List<SheetSyncItem> assets = new List<SheetSyncItem>();
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public override void Load()
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
            
        }

        public override void Import(SpreadsheetData spreadsheetData)
        {
            foreach (var item in assets) {
                item.asset?.ApplySpreadsheetData(spreadsheetData);
            }
        }
    }
}