namespace UniModules.UniGame.ConvexHull.Editor
{
    using System;
    using Runtime;
    using Runtime.Abstract;
    using UnityEditor;
    using UnityEngine;

    public class ConvexHullSpritePreviewEditorWindow : EditorWindow
    {
        private Styles _styles;
        
        private const float ScrollbarMargin = 16f;
        private const float MinZoomPercentage = 0.5f;
        private const float MaxZoom = 50f;
        private const float WheelZoomSpeed = 0.03f;
        private const float MouseZoomSpeed = 0.005f;

        private Texture2D _texture;

        private Rect _textureViewRect;
        private Rect _textureRect;
        
        private float _currentZoom = -1f;
        private Vector2 _scrollPosition;
        
        private readonly ISpriteConvexHullBuilder _convexHullBuilder;
        private readonly IConvexHullAreaAdapter _convexHullAreaAdapter;
        private readonly IAreaCalculator _areaCalculator;

        private Vector2[] _convexHull;

        private Sprite _sourceSprite;
        private Sprite _previousSprite;
        
        private Matrix4x4 _oldHandlesMatrix;
        
        private Vector2 _screenSize = new Vector2(1920, 1080);
        private Vector2 _sceneSize = new Vector2(36.4f, 20.5f);
        private float _minSizeInPixels = 75.0f;

        private Vector2 _previousScreenSize = new Vector2(1920, 1080);
        private Vector2 _previousSceneSize = new Vector2(36.4f, 20.5f);
        private float _previousMinSizeInPixels = 75.0f;

        private Rect MaxScrollRect
        {
            get
            {
                var halfWidth  = _texture.width * .5f * _currentZoom;
                var halfHeight = _texture.height * .5f * _currentZoom;
                return new Rect(-halfWidth, -halfHeight, _textureViewRect.width + halfWidth * 2f, _textureViewRect.height + halfHeight * 2f);
            }
        }

        public ConvexHullSpritePreviewEditorWindow()
        {
            _convexHullBuilder = new SpriteConvexHullBuilder();
            _convexHullAreaAdapter = new ConvexHullAreaAdapter(new ConvexHullAreaCalculator());
            _areaCalculator = new MinAreaCalculator();
        }

        [MenuItem("UniGame/ConvexHull/Convex Hull Preview")]
        public static void ShowWindow()
        {
            var window = GetWindow<ConvexHullSpritePreviewEditorWindow>();
            window.titleContent = new GUIContent("Convex Hull Preview");
            window.minSize = new Vector2(400, 500);
            window.SetNewTexture(Selection.activeObject as Sprite);
            window.Show();
        }

        private void OnGUI()
        {
            InitStyles();

            _textureViewRect = new Rect(0f, 0f, Screen.width - ScrollbarMargin, Screen.height - ScrollbarMargin - 21);
            _oldHandlesMatrix = Handles.matrix;
            DoTextureGui();
            Handles.matrix = _oldHandlesMatrix;

            DoSettingsPanel();
        }

        private void InitStyles()
        {
            if (_styles == null)
                _styles = new Styles();
        }

        private float GetMinZoom()
        {
            if (_texture == null)
                return 1.0f;
            
            return Mathf.Min(_textureViewRect.width / _texture.width, _textureViewRect.height / _texture.height, MaxZoom) * MinZoomPercentage;
        }

        private void HandleZoom()
        {
            var zoomMode = Event.current.alt && Event.current.button == 1;
            if (zoomMode)
            {
                EditorGUIUtility.AddCursorRect(_textureViewRect, MouseCursor.Zoom);
            }

            if ((Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDown) && zoomMode ||
                (Event.current.type == EventType.KeyUp || Event.current.type == EventType.KeyDown) && Event.current.keyCode == KeyCode.LeftAlt)
            {
                Repaint();
            }

            if (Event.current.type == EventType.ScrollWheel || Event.current.type == EventType.MouseDrag && Event.current.alt && Event.current.button == 1)
            {
                var zoomMultiplier = 1f - Event.current.delta.y * (Event.current.type == EventType.ScrollWheel ? WheelZoomSpeed : -MouseZoomSpeed);
                
                var wantedZoom = _currentZoom * zoomMultiplier;

                var currentZoom = Mathf.Clamp(wantedZoom, GetMinZoom(), MaxZoom);

                if (Math.Abs(currentZoom - _currentZoom) > float.Epsilon)
                {
                    _currentZoom = currentZoom;
                    
                    if (Math.Abs(wantedZoom - currentZoom) > float.Epsilon)
                        zoomMultiplier /= wantedZoom / currentZoom;

                    Vector3 textureHalfSize = new Vector2(_texture.width, _texture.height) * 0.5f;
                    var mousePositionWorld = Handles.inverseMatrix.MultiplyPoint3x4(Event.current.mousePosition + _scrollPosition);
                    var delta = (mousePositionWorld - textureHalfSize) * (zoomMultiplier - 1f);

                    _scrollPosition += (Vector2)Handles.matrix.MultiplyVector(delta);

                    Event.current.Use();
                }
            }
        }

        private void HandlePanning()
        {
            var panMode = !Event.current.alt && Event.current.button > 0 || Event.current.alt && Event.current.button <= 0;
            if (panMode && GUIUtility.hotControl == 0)
            {
                EditorGUIUtility.AddCursorRect(_textureViewRect, MouseCursor.Pan);

                if (Event.current.type == EventType.MouseDrag)
                {
                    _scrollPosition -= Event.current.delta;
                    Event.current.Use();
                }
            }
            
            if ((Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDown) && panMode ||
                (Event.current.type == EventType.KeyUp || Event.current.type == EventType.KeyDown) && Event.current.keyCode == KeyCode.LeftAlt)
            {
                Repaint();
            }
        }

        private void DrawTexture()
        {
            var oldFilter = _texture.filterMode;
            _texture.filterMode = FilterMode.Trilinear;

            EditorGUI.DrawTextureTransparent(_textureRect, _texture, ScaleMode.StretchToFill, 0, 0);
            
            _texture.filterMode = oldFilter;
        }

        private void DrawScreenSpaceBackground()
        {
            if (Event.current.type == EventType.Repaint)
                _styles.BackgroundStyle.Draw(_textureViewRect, false, false, false, false);
        }

        private void HandleScrollbars()
        {
            var horizontalScrollBarPosition = new Rect(_textureViewRect.xMin, _textureViewRect.yMax, _textureViewRect.width, ScrollbarMargin);
            _scrollPosition.x = GUI.HorizontalScrollbar(horizontalScrollBarPosition, _scrollPosition.x, _textureViewRect.width, MaxScrollRect.xMin, MaxScrollRect.xMax);

            var verticalScrollBarPosition = new Rect(_textureViewRect.xMax, _textureViewRect.yMin, ScrollbarMargin, _textureViewRect.height);
            _scrollPosition.y = GUI.VerticalScrollbar(verticalScrollBarPosition, _scrollPosition.y, _textureViewRect.height, MaxScrollRect.yMin, MaxScrollRect.yMax);
        }

        private void SetupHandlesMatrix()
        {
            var handlesPos = new Vector3(_textureRect.x, _textureRect.yMax, 0f);
            var handlesScale = new Vector3(_currentZoom, -_currentZoom, 1f);
            
            Handles.matrix = Matrix4x4.TRS(handlesPos, Quaternion.identity, handlesScale);
        }

        private void DoTextureGui()
        {
            if (_texture == null || _sourceSprite == null)
                return;
            
            if (_currentZoom < 0f)
                _currentZoom = GetMinZoom();
            
            _textureRect = new Rect(_textureViewRect.width / 2f - _texture.width * _currentZoom / 2f, _textureViewRect.height / 2f - _texture.height * _currentZoom / 2f, 
                _texture.width * _currentZoom, _texture.height * _currentZoom);

            HandleScrollbars();
            SetupHandlesMatrix();
            DrawScreenSpaceBackground();

            GUI.BeginClip(_textureViewRect, -_scrollPosition, Vector2.zero, false);

            if (Event.current.type == EventType.Repaint)
            {
                DrawTexture();
            }

            DoTextureGuiExtras();

            GUI.EndClip();

            HandleZoom();
            HandlePanning();
        }

        private void DoTextureGuiExtras()
        {
            if (Event.current.type == EventType.Repaint) {
                Handles.color = Color.green;
                
                var currentHandlesMatrix = Handles.matrix;

                Handles.matrix = _oldHandlesMatrix;

                for (var i = 0; i < _convexHull.Length; i++) {
                    if (i == _convexHull.Length - 1) {
                        var a = ConvertLocalPointToRectPoint(_convexHull[i]);
                        var b = ConvertLocalPointToRectPoint(_convexHull[0]);
                        
                        Handles.DrawLine(a, b);
                    }
                    else {
                        var a = ConvertLocalPointToRectPoint(_convexHull[i]);
                        var b = ConvertLocalPointToRectPoint(_convexHull[i + 1]);

                        Handles.DrawLine(a, b);
                    }
                }

                Handles.matrix = currentHandlesMatrix;

                var savedGuiColor = GUI.color;
                GUI.color = Color.red;

                foreach (var point in _sourceSprite.vertices) {
                    GUI.Box(new Rect(ConvertLocalPointToRectPoint(point) - new Vector2(2.5f, 2.5f),
                        new Vector2(1.0f, 1.0f)), GUIContent.none, _styles.VertexStyle);
                }

                GUI.color = Color.green;

                foreach (var point in _convexHull) {
                    GUI.Box(new Rect(ConvertLocalPointToRectPoint(point) - new Vector2(2.5f, 2.5f),
                        new Vector2(1.0f, 1.0f)), GUIContent.none, _styles.ConvexHullPointStyle);
                }

                GUI.color = savedGuiColor;
            }
        }

        private void DoSettingsPanel()
        {
            var panelOffset = 10.0f + ScrollbarMargin;
            var panelWidth = 300.0f;
            var panelHeight = 205.0f;
            var panelRect = new Rect(Screen.width - panelWidth - panelOffset, Screen.height - panelHeight - panelOffset - 21, 
                panelWidth, panelHeight);
            
            GUI.Box(panelRect, "Settings", _styles.SettingsPanelStyle);
            GUILayout.BeginArea(new Rect(panelRect.x + 10.0f, panelRect.y + 30.0f, panelRect.width - 20.0f, panelRect.height - 40.0f));
            _previousSprite = EditorGUILayout.ObjectField("Texture:", _previousSprite, typeof(Sprite), false) as Sprite;
            _previousScreenSize = EditorGUILayout.Vector2Field("Screen size:", _previousScreenSize);
            _previousSceneSize = EditorGUILayout.Vector2Field("Scene size:", _previousSceneSize);
            _previousMinSizeInPixels = EditorGUILayout.Slider("Min size in pixels:", _previousMinSizeInPixels, 1.0f, 300.0f);
            GUILayout.EndArea();

            if (_previousSprite != _sourceSprite) {
                SetNewTexture(_previousSprite);
            }

            if (_previousScreenSize != _screenSize) {
                _screenSize = _previousScreenSize;
                UpdateConvexHull();
            }

            if (_previousSceneSize != _sceneSize) {
                _sceneSize = _previousSceneSize;
                UpdateConvexHull();
            }

            if (Math.Abs(_previousMinSizeInPixels - _minSizeInPixels) > float.Epsilon) {
                _minSizeInPixels = _previousMinSizeInPixels;
                UpdateConvexHull();
            }
        }

        private Vector2 ConvertLocalPointToRectPoint(Vector2 localPoint)
        {
            var pixelsPerUnit = _sourceSprite.pixelsPerUnit;
            pixelsPerUnit *= _currentZoom;
            var x = pixelsPerUnit * localPoint.x;
            var y = -pixelsPerUnit * localPoint.y;

            return new Vector2(x, y) + _textureRect.center;
        }

        private void UpdateConvexHull()
        {
            var convexHull = _convexHullBuilder.Build(_sourceSprite);
            var adaptedConvexHull = _convexHullAreaAdapter.Adapt(convexHull, 
                _areaCalculator.CalculateArea(_sceneSize, _screenSize, _minSizeInPixels));

            _convexHull = adaptedConvexHull;
        }

        private void SetNewTexture(Sprite sprite)
        {
            if (sprite != _sourceSprite)
            {
                _sourceSprite = sprite;
                _previousSprite = sprite;
                
                _currentZoom = -1;
                
                _texture = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height, TextureFormat.ARGB32, false);
                var pixels = sprite.texture.GetPixels((int) sprite.textureRect.x, (int) sprite.textureRect.y,
                    (int) sprite.textureRect.width, (int) sprite.textureRect.height);
                
                _texture.SetPixels(pixels);
                _texture.Apply();

                _texture.name = sprite.name;
                
                UpdateConvexHull();
            }
        }

        private class Styles
        {
            public readonly GUIStyle BackgroundStyle = "preBackground";

            public readonly GUIStyle VertexStyle = new GUIStyle();
            public readonly GUIStyle ConvexHullPointStyle = new GUIStyle();
            
            public readonly GUIStyle SettingsPanelStyle;

            public Styles()
            {
                VertexStyle.fixedHeight = 5f;
                VertexStyle.fixedWidth = 5f;
                VertexStyle.normal.background = EditorGUIUtility.whiteTexture;

                ConvexHullPointStyle.fixedHeight = VertexStyle.fixedHeight;
                ConvexHullPointStyle.fixedWidth = VertexStyle.fixedWidth;
                ConvexHullPointStyle.normal.background = EditorGUIUtility.whiteTexture;

                var backgroundTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                backgroundTexture.SetPixel(0, 0, new Color(0.7f, 0.7f, 0.7f, 0.9f));
                backgroundTexture.Apply();

                SettingsPanelStyle = new GUIStyle("Box") {
                    normal = {
                        background = backgroundTexture
                    }
                };
            }
        }
    }
}