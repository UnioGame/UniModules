namespace UniModules.UniGame.ConvexHull.Editor
{
    using System.Reflection;
    using Runtime;
    using Runtime.Abstract;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using UnityEngine;

    public class SpriteConvexHullWindow : OdinEditorWindow
    {
        [SerializeField]
        [OnValueChanged("UpdateConvexHull")]
        [BoxGroup("Target Settings")]
        private Sprite _source;

        [SerializeField]
        [Min(1)]
        [OnValueChanged("UpdateConvexHull")]
        [BoxGroup("Target Settings/Screen")]
        private int _screenWidth = 1920;
        [SerializeField]
        [Min(1)]
        [OnValueChanged("UpdateConvexHull")]
        [BoxGroup("Target Settings/Screen")]
        private int _screenHeight = 1080;

        [SerializeField]
        [Min(0.1f)]
        [OnValueChanged("UpdateConvexHull")]
        [BoxGroup("Target Settings/Scene")]
        private float _sceneWidth = 36.4f;
        [SerializeField]
        [Min(0.1f)]
        [OnValueChanged("UpdateConvexHull")]
        [BoxGroup("Target Settings/Scene")]
        private float _sceneHeight = 20.5f;

        [SerializeField]
        [Min(0.1f)]
        [OnValueChanged("UpdateConvexHull")]
        [BoxGroup("Target Settings/Finger")]
        private float _minSizeInPixels = 75.0f;

        private readonly ISpriteConvexHullBuilder _convexHullBuilder;
        private readonly IConvexHullAreaAdapter _convexHullAreaAdapter;
        private readonly IAreaCalculator _areaCalculator;

        private Vector2[] _convexHull;

        private Vector2 _cameraPosition;
        private float _cameraZoom = 1.0f;

        private float _minZoom = 0.1f;
        private float _maxZoom = 10.0f;

        private bool _isDragging;

        public SpriteConvexHullWindow()
        {
            _convexHullBuilder = new SpriteConvexHullBuilder();
            _convexHullAreaAdapter = new ConvexHullAreaAdapter(new ConvexHullAreaCalculator());
            _areaCalculator = new MinAreaCalculator();
        }

        [MenuItem("UniGame/ConvexHull/SpritePreview")]
        private static void ShowWindow()
        {
            var window = GetWindow<SpriteConvexHullWindow>();
            window.titleContent = new GUIContent("Sprite Convex Hull Preview");
            window.minSize = new Vector2(300, 600);
            window.wantsMouseMove = true;
            window.Show();
            
            EditorWindow.FocusWindowIfItsOpen<SpriteConvexHullWindow>();
        }

        protected override void OnGUI()
        {
            var rect = new Rect(0, 230, Screen.width, Screen.height - 230);
            
            EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f, 1.0f));

            if (_source != null) {
                HandleEvents(Event.current, rect);
                DrawTarget(rect);
            }
            
            base.OnGUI();
        }

        private void HandleEvents(Event currentEvent, Rect rect)
        {
            if(!_isDragging && !rect.Contains(currentEvent.mousePosition))
                return;
        
            if (currentEvent.isScrollWheel) {
                var screenMousePosition = currentEvent.mousePosition;
                var delta = currentEvent.delta;
                var zoomMousePosition = ConvertScreenToZoomPoint(screenMousePosition, rect);
                var zoomDelta = -delta.y * 0.05f;
                var oldZoom = _cameraZoom;

                _cameraZoom += zoomDelta;
                _cameraZoom = Mathf.Clamp(_cameraZoom, _minZoom, _maxZoom);
                _cameraPosition += zoomMousePosition - _cameraPosition - oldZoom / _cameraZoom * (zoomMousePosition - _cameraPosition);
                
                currentEvent.Use();
            }
            
            _isDragging = currentEvent.button == 2 && currentEvent.type == EventType.MouseDrag;

            if (_isDragging) {
                var delta = currentEvent.delta;

                delta /= _cameraZoom;
                _cameraPosition -= delta;
                
                currentEvent.Use();
            }
        }

        private void DrawTarget(Rect rect)
        {
            if(_convexHull == null)
                return;

            var clippedArea = EditorZoomArea.Begin(_cameraZoom, rect);

            var areaRect = new Rect(rect.x - _cameraPosition.x, rect.y - _cameraPosition.y, rect.width, rect.height);
            GUILayout.BeginArea(areaRect);

            var x = areaRect.width * 0.5f - _source.texture.width * 0.5f;
            var y = areaRect.height * 0.5f - _source.texture.height * 0.5f;
            var textureRect = new Rect(x, y, _source.texture.width, _source.texture.height);
            GUI.DrawTexture(textureRect, _source.texture, ScaleMode.StretchToFill, true);
            
            Handles.BeginGUI();
            Handles.color = Color.green;

            for (var i = 0; i < _convexHull.Length; i++) {
                if (i == _convexHull.Length - 1) {
                    var a = ConvertLocalPointToRectPoint(_convexHull[i], textureRect);
                    var b = ConvertLocalPointToRectPoint(_convexHull[0], textureRect);
                    Handles.DrawLine(a, b);
                }
                else {
                    var a = ConvertLocalPointToRectPoint(_convexHull[i], textureRect);
                    var b = ConvertLocalPointToRectPoint(_convexHull[i + 1], textureRect);

                    Handles.DrawLine(a, b);
                }
            }

            Handles.EndGUI();

            var savedGuiColor = GUI.color;
            GUI.color = Color.red;
            
            for (var i = 0; i < _source.vertices.Length; i++) {
                GUI.Box(new Rect(ConvertLocalPointToRectPoint(_source.vertices[i], textureRect) - new Vector2(0.5f, 0.5f), 
                    new Vector2(1.0f, 1.0f)), GUIContent.none);
            }
            
            GUI.color = Color.green;

            for (var i = 0; i < _convexHull.Length; i++) {
                GUI.Box(new Rect(ConvertLocalPointToRectPoint(_convexHull[i], textureRect) - new Vector2(0.5f, 0.5f), 
                    new Vector2(1.0f, 1.0f)), GUIContent.none);
            }

            GUI.color = savedGuiColor;

            GUILayout.EndArea();
            
            EditorZoomArea.End();
        }

        private Vector2 ConvertLocalPointToRectPoint(Vector2 localPoint, Rect rect)
        {
            var center = rect.center;
            var x = center.x + rect.width * 0.5f * localPoint.x;
            var y = center.y - rect.height * 0.5f * localPoint.y;

            return new Vector2(x, y);
        }

        private void UpdateConvexHull()
        {
            _convexHull = BuildConvexHull(new Vector2(_sceneWidth, _sceneHeight), new Vector2(_screenWidth, _screenHeight), _minSizeInPixels);
        }

        private Vector2[] BuildConvexHull(Vector2 sceneSize, Vector2 screenSize, float minFingerSize)
        {
            var convexHull = _convexHullBuilder.Build(_source);
            var adaptedConvexHull = _convexHullAreaAdapter.Adapt(convexHull, _areaCalculator.CalculateArea(sceneSize, screenSize, minFingerSize));

            return adaptedConvexHull;
        }

        private Vector2 ConvertScreenToZoomPoint(Vector2 screenPoint, Rect areaRect)
        {
            return (screenPoint - areaRect.TopLeft()) / _cameraZoom + _cameraPosition;
        }
    }

    public static class EditorZoomArea
    {
        private const float EditorWindowTabHeight = 21.0f;
        private static Matrix4x4 previousGuiMatrix;

        public static Rect Begin(float zoomScale, Rect areaRect)
        {
            GUI.EndGroup();

            var clippedArea = areaRect.ScaleSizeBy(1.0f / zoomScale, areaRect.TopLeft());
            clippedArea.y += EditorWindowTabHeight;
            GUI.BeginGroup(clippedArea);

            previousGuiMatrix = GUI.matrix;

            var translationMatrix = Matrix4x4.TRS(clippedArea.TopLeft(), Quaternion.identity, Vector3.one);
            var scaleMatrix = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1.0f));

            GUI.matrix = translationMatrix * scaleMatrix * translationMatrix.inverse * GUI.matrix;

            return clippedArea;
        }

        public static void End()
        {
            GUI.matrix = previousGuiMatrix;
            GUI.EndGroup();
            GUI.BeginGroup(new Rect(0.0f, EditorWindowTabHeight, Screen.width, Screen.height));
        }
    }

    public static class RectExtensions
    {
        public static Vector2 TopLeft(this Rect rect)
        {
            return new Vector2(rect.xMin, rect.yMin);
        }

        public static Rect ScaleSizeBy(this Rect rect, float scale)
        {
            return rect.ScaleSizeBy(scale, rect.center);
        }

        public static Rect ScaleSizeBy(this Rect rect, float scale, Vector2 pivotPoint)
        {
            var result = rect;
            result.x -= pivotPoint.x;
            result.y -= pivotPoint.y;
            result.xMin *= scale;
            result.xMax *= scale;
            result.yMin *= scale;
            result.yMax *= scale;
            result.x += pivotPoint.x;
            result.y += pivotPoint.y;
            return result;
        }
        
        public static Rect ScaleSizeBy(this Rect rect, Vector2 scale)
        {
            return rect.ScaleSizeBy(scale, rect.center);
        }
        
        public static Rect ScaleSizeBy(this Rect rect, Vector2 scale, Vector2 pivotPoint)
        {
            var result = rect;
            result.x -= pivotPoint.x;
            result.y -= pivotPoint.y;
            result.xMin *= scale.x;
            result.xMax *= scale.x;
            result.yMin *= scale.y;
            result.yMax *= scale.y;
            result.x += pivotPoint.x;
            result.y += pivotPoint.y;
            return result;
        }
    }
}