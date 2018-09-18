using System.Linq;
using UnityEditor;
using UnityEngine;

public class CompareWindow : EditorWindow {

    private Vector2 _scroll;

    private string[] _leftSource;
    private string[] _rightSource;
    private string[] _intersection;
    private string[] _leftUnique;
    private string[] _rightUnique;

    private bool _leftFold;
    private bool _leftUniqueFold;
    private bool _rightUniqueFold;
    private bool _rightFold;
    private bool _intersectionFold;

    public static CompareWindow Show(string[] leftSource,string[] rightSource)
    {
        var window = EditorWindow.CreateInstance<CompareWindow>();
        window.Initialize(leftSource, rightSource);
        window.Show();
        return window;
    }

    public void Initialize(string[] leftSource, string[] rightSource) {

        _leftSource = leftSource;
        _rightSource = rightSource;
        _intersection = _leftSource.Intersect(_rightSource).ToArray();
        _leftUnique = _leftSource.Where(x => _intersection.Contains(x) == false).ToArray();
        _rightUnique = _rightSource.Where(x => _intersection.Contains(x) == false).ToArray();

    }

    private void OnGUI() {

        _scroll =GUILayout.BeginScrollView(_scroll);
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();

        DrawTextFoldField(_leftSource, "left source", ref _leftFold);
        DrawTextFoldField(_rightSource, "right source", ref _rightFold);

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        DrawTextFoldField(_leftUnique, "left unique", ref _leftUniqueFold);
        DrawTextFoldField(_rightUnique, "right unique", ref _rightUniqueFold);

        GUILayout.EndHorizontal();

        DrawTextFoldField(_intersection, "common", ref _intersectionFold);

        GUILayout.EndVertical();
        GUILayout.EndScrollView();

    }


    private void DrawTextFoldField(string[] data,string label, ref bool foldValue) {
        GUILayout.BeginVertical();

        foldValue = EditorGUILayout.Foldout(foldValue, label);
        if (foldValue) {
            EditorGUILayout.TextArea(string.Join("\n", data));
        }

        GUILayout.EndVertical();

    }
}
