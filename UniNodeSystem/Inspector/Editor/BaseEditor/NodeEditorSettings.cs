using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniNodeSystemEditor
{
    
        [Serializable]
        public class NodeEditorSettings : ISerializationCallbackReceiver 
        {
            
            [SerializeField] private Color32 _gridLineColor = new Color(0.45f, 0.45f, 0.45f);
            public Color32 gridLineColor { get { return _gridLineColor; } set { _gridLineColor = value; _gridTexture = null; _crossTexture = null; } }

            [SerializeField] private Color32 _gridBgColor = new Color(0.18f, 0.18f, 0.18f);
            public Color32 gridBgColor { get { return _gridBgColor; } set { _gridBgColor = value; _gridTexture = null; } }

            public Color32 highlightColor = new Color32(255, 255, 255, 255);
            public bool gridSnap = true;
            public bool autoSave = false;
            [SerializeField] private string typeColorsData = "";
            [NonSerialized] public Dictionary<string, Color> typeColors = new Dictionary<string, Color>();
            public NodeEditorNoodleType noodleType = NodeEditorNoodleType.Curve;

            private Texture2D _gridTexture;
            public Texture2D gridTexture {
                get {
                    if (_gridTexture == null) _gridTexture = NodeEditorResources.GenerateGridTexture(gridLineColor, gridBgColor);
                    return _gridTexture;
                }
            }
            private Texture2D _crossTexture;
            public Texture2D crossTexture {
                get {
                    if (_crossTexture == null) _crossTexture = NodeEditorResources.GenerateCrossTexture(gridLineColor);
                    return _crossTexture;
                }
            }

            public void OnAfterDeserialize() {
                // Deserialize typeColorsData
                typeColors = new Dictionary<string, Color>();
                string[] data = typeColorsData.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < data.Length; i += 2) {
                    Color col;
                    if (ColorUtility.TryParseHtmlString("#" + data[i + 1], out col)) {
                        typeColors.Add(data[i], col);
                    }
                }
            }

            public void OnBeforeSerialize() {
                // Serialize typeColors
                typeColorsData = "";
                foreach (var item in typeColors) {
                    typeColorsData += item.Key + "," + ColorUtility.ToHtmlStringRGB(item.Value) + ",";
                }
            }
        }
}