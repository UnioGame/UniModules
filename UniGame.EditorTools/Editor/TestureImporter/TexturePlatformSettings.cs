namespace UniModules.UniGame.EditorTools.Editor.TestureImporter
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [Serializable]
    public class TexturePlatformSettings
    {
        public                 bool                        overriden            = true;
        public                 bool                        allowsAlphaSplitting = false;
        public                 AndroidETC2FallbackOverride androidETC2FallbackOverride;
        [Range(0, 100)] public int                         compressionQuality     = 50;
        public                 bool                        useCrunchedCompression = true;
        public                 TextureImporterFormat       textureImporterFormat  = TextureImporterFormat.ETC2_RGBA8Crunched;
        public                 TextureImporterCompression  textureCompression     = TextureImporterCompression.Compressed;
        public                 TextureResizeAlgorithm      resizeAlgorithm        = TextureResizeAlgorithm.Mitchell;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.ValueDropdown("TextureSizes")]
        public int maxTextureSize = 1024;
#endif

        private static IEnumerable<int> TextureSizes = new List<int>() {
            32, 64, 128, 256, 512, 1024, 2048, 4096
        };
    }
}