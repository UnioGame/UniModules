namespace UniModules.UniGame.GoogleSpreadsheetsImporter.Editor.SheetsImporter.Importers
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface IAssetsProvider
    {
        List<Object> GetAssets();
    }
}