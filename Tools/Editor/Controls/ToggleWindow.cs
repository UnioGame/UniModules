using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ToggleWindow : EditorWindow {

    private Dictionary<string, bool> _items = new Dictionary<string, bool>();
    private Vector2 _scroll;
    private Action<string[]> _selectionChanged;

    public string[] Selected {
        get {
            return _items.Where(x => x.Value).
                Select(x => x.Key).ToArray();
        }
    }

    public static ToggleWindow Show(string[] source,string[] selected, Action<string[]> onSelectionChanged)
    {
        var window = EditorWindow.CreateInstance<ToggleWindow>();
        window.Initialize(source,selected, onSelectionChanged);
        window.Show();
        return window;
    }

    public void Initialize(string[] source, string[] selected, Action<string[]> onSelectionChanged) {
        _selectionChanged = onSelectionChanged;
        _items.Clear();
        foreach (var item in source) {
            _items[item] = false;
        }
        foreach (var item in selected) {
            if (_items.ContainsKey(item))
                _items[item] = true;
        }
    }

    private void OnGUI() {

        _scroll =GUILayout.BeginScrollView(_scroll);
        GUILayout.BeginVertical();

        var selectionChanged = false;
        var selectedValue = string.Empty;

        foreach (var item in _items) {

            GUILayout.BeginHorizontal();

            var selection = GUILayout.Toggle(item.Value, item.Key);
            if (selection != _items[item.Key]) {
                selectedValue = item.Key;
                selectionChanged = true;
            }

            GUILayout.EndHorizontal();
        }

        if (selectionChanged && _selectionChanged!=null) {
            _items[selectedValue] = !_items[selectedValue];
            var selectedItems = Selected;
            _selectionChanged(selectedItems);
        }

        GUILayout.EndVertical();
        GUILayout.EndScrollView();

    }
    
}
