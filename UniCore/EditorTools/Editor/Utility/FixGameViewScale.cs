#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class FixGameViewScale {
    private const string       PREFS_KEY     = "FIX_GAMEVIEW_SCALES";
    private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic;

    private static readonly Type      CachedGameViewType;
    private static readonly FieldInfo CachedZoomAreaFieldInfo;
    private static readonly FieldInfo CachedScaleFieldInfo;

    static FixGameViewScale() {
        CachedGameViewType      = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
        CachedZoomAreaFieldInfo = CachedGameViewType.GetField("m_ZoomArea", BINDING_FLAGS);
        CachedScaleFieldInfo    = CachedZoomAreaFieldInfo.FieldType.GetField("m_Scale", BINDING_FLAGS);

        EditorApplication.playModeStateChanged += OnPlayStateChanged;
    }

    private static void OnPlayStateChanged(PlayModeStateChange playModeStateChange) {
        var gameViews = Resources.FindObjectsOfTypeAll(CachedGameViewType);
        if (playModeStateChange == PlayModeStateChange.ExitingEditMode) {
            var scale = new ScaleCache(gameViews.Length);

            for (var i = 0; i < gameViews.Length; i++) {
                var gameViewWindow = gameViews[i];

                var zoomArea = CachedZoomAreaFieldInfo.GetValue(gameViewWindow);

                scale[i] = (Vector2) CachedScaleFieldInfo.GetValue(zoomArea);
            }

            EditorPrefs.SetString(PREFS_KEY, JsonUtility.ToJson(scale));
        }

        if (playModeStateChange == PlayModeStateChange.EnteredPlayMode) {
            var scale = JsonUtility.FromJson<ScaleCache>(EditorPrefs.GetString(PREFS_KEY));
            for (var i = 0; i < gameViews.Length; i++) {
                var gameViewWindow = gameViews[i];

                var zoomArea = CachedZoomAreaFieldInfo.GetValue(gameViewWindow);

                CachedScaleFieldInfo.SetValue(zoomArea, scale[i]);
            }
        }
    }

    [Serializable]
    private class ScaleCache {
        [SerializeField]
        private Vector2[] scales;

        public ScaleCache(int count) => this.scales = new Vector2[count];

        public Vector2 this[int index] {
            get => this.scales[index];
            set => this.scales[index] = value;
        }
    }
}
#endif