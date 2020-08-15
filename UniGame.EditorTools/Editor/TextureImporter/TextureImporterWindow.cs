#if ODIN_INSPECTOR

namespace UniModules.UniGame.EditorTools.Editor.TestureImporter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Core.EditorTools.Editor.AssetOperations;
    using Core.Runtime.Extension;
    using Sirenix.OdinInspector.Editor;
    using UniGreenModules.UniCore.EditorTools.Editor.Utility;
    using UniGreenModules.UniCore.Runtime.Extension;
    using UniGreenModules.UniCore.Runtime.Utils;
    using UnityEditor;
    using UnityEditor.U2D.PSD;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [Flags]
    public enum TextureImporterFilter
    {
        All = ~0,
        TextureImporter = 1<< 1,
        PSDImporter = 1 << 2,
    }
    
    public class TextureImporterWindow : OdinEditorWindow
    {
        #region statics

        private const string psdImporterPlatformField     = "m_PlatformSettings";
        private const string defaultTarget                = "Default";
        private const string textureImporterDefaultTarget = "DefaultTexturePlatform";

        private static FieldInfo _psdImporterPlatformsFieldInfo;

        private static List<string> buildTargets = defaultTarget.Yield().Concat(EnumValue<BuildTarget>.Names).ToList();

        [MenuItem("UniGame/Tools/Texture Importer")]
        public static void Open()
        {
            var window = GetWindow<TextureImporterWindow>();
            window.Show();
        }

        #endregion

        #region inspector

        [Space(6)]
        [Sirenix.OdinInspector.BoxGroup("Import Settings")]
        [Sirenix.OdinInspector.ValueDropdown("buildTargets")]
        public string buildTarget = "Default";
        [Space(4)]
        [Sirenix.OdinInspector.BoxGroup("Import Settings")]
        public TextureImporterFilter importersFilter = (TextureImporterFilter)~0;
        
        [Sirenix.OdinInspector.BoxGroup("Import Settings")]
        public bool overrideCurrentPlatformFormat = false;

        [SerializeField]
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
        [Sirenix.OdinInspector.BoxGroup("Import Settings")]
        public TexturePlatformSettings platformSettings = new TexturePlatformSettings();


        [Space(8)] 
        [Sirenix.OdinInspector.BoxGroup("Search Filter")]
        [Sirenix.OdinInspector.InlineEditor]
        public List<Object> targetAssets = new List<Object>();

        [Space(4)]
        [Sirenix.OdinInspector.FolderPath]
        [Sirenix.OdinInspector.BoxGroup("Search Filter")]
        public List<string> searchFolders = new List<string>();


        [Sirenix.OdinInspector.InlineEditor]
        [Space(8)]
        public List<AssetImporter> resultAssets = new List<AssetImporter>();

        #endregion

        #region public methods

        [Sirenix.OdinInspector.Button]
        [Sirenix.OdinInspector.GUIColor(0.2f, 1, 0.2f)]
        public void Search()
        {
            UpdateSearchResults();
        }

        [Sirenix.OdinInspector.Button]
        [Sirenix.OdinInspector.GUIColor(0.2f, 1, 0.2f)]
        public void ApplyImportSettings()
        {
            if (resultAssets.Count == 0) {
                Search();
            }
            
            foreach (var assetImporter in resultAssets) {
                Import(assetImporter);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            ClearSearch();
        }

        public void ResetOverrirdes()
        {
            if (resultAssets.Count == 0) {
                Search();
            }

            

        }        

        public void ClearSearch()
        {
            resultAssets = new List<AssetImporter>();
        }
        
        #endregion

        protected override void OnEnable()
        {
            base.OnEnable();
            _psdImporterPlatformsFieldInfo = typeof(PSDImporter).GetField(psdImporterPlatformField, BindingFlags.NonPublic |
                                                                                                    BindingFlags.Instance);
        }

        private void UpdateSearchResults()
        {
            ClearSearch();

            var folderFilters = searchFolders.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (folderFilters.Length > 0) {
                var importers = Filter(AssetEditorTools.GetAssetImporters<Object>(folderFilters));
                resultAssets.AddRange(importers);
            }

            resultAssets.AddRange(Filter(targetAssets.Where(x => x).Select(AssetEditorTools.GetAssetImporter)));
        }

        private IEnumerable<AssetImporter> Filter(IEnumerable<AssetImporter> importers)
        {
            foreach (var assetImporter in importers) {
                switch (assetImporter) {
                    case TextureImporter textureImporter:
                        if(!importersFilter.IsFlagSet(TextureImporterFilter.TextureImporter))
                            break;
                        yield return textureImporter;
                        break;
                    case PSDImporter psdImporter:
                        if(!importersFilter.IsFlagSet(TextureImporterFilter.PSDImporter))
                            break;
                        yield return psdImporter;
                        break;
                    default:
                        continue;
                }
            }
        }

        private void Import(AssetImporter assetImporter)
        {
            switch (assetImporter) {
                case TextureImporter textureImporter:
                    var target = string.Equals(buildTarget, defaultTarget, StringComparison.OrdinalIgnoreCase) ? 
                        textureImporterDefaultTarget : buildTarget;
                    var current = textureImporter.GetPlatformTextureSettings(target);
                    current = UpdateSettings(current);
                    textureImporter.SetPlatformTextureSettings(current);
                    break;
                case PSDImporter psdImporter:
                    var settings = _psdImporterPlatformsFieldInfo.GetValue(psdImporter) as List<TextureImporterPlatformSettings>;
                    var importerPlatformSettings = settings?.FirstOrDefault(x =>
                        string.Equals(x.name, buildTarget, StringComparison.OrdinalIgnoreCase));
                    if (importerPlatformSettings == null) {
                        importerPlatformSettings = new TextureImporterPlatformSettings() {
                            name = buildTarget,
                        };
                        settings.Add(importerPlatformSettings);
                    }

                    importerPlatformSettings = UpdateSettings(importerPlatformSettings);
                    _psdImporterPlatformsFieldInfo.SetValue(psdImporter, settings);
                    break;
                default:
                    return;
            }

            assetImporter.MarkDirty();
            assetImporter.SaveAndReimport();
        }

        private TextureImporterPlatformSettings UpdateSettings(TextureImporterPlatformSettings source)
        {
            source.overridden = platformSettings.overriden;
            source.format                      = overrideCurrentPlatformFormat ? platformSettings.textureImporterFormat : source.format;
            source.compressionQuality          = platformSettings.compressionQuality;
            source.crunchedCompression         = platformSettings.useCrunchedCompression;
            source.maxTextureSize              = platformSettings.maxTextureSize;
            source.allowsAlphaSplitting        = platformSettings.allowsAlphaSplitting;
            source.androidETC2FallbackOverride = platformSettings.androidETC2FallbackOverride;
            source.textureCompression = platformSettings.textureCompression;
            source.resizeAlgorithm = platformSettings.resizeAlgorithm;

            return source;
        }
    }
}

#endif