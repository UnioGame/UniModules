using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ToggleWindow : EditorWindow {

    private SortedDictionary<string, bool> _items = new SortedDictionary<string, bool>();
    private Vector2 _scroll;
    private Action<string[]> _selectionChanged;
    private string[] _selectionSource;
    private string _tempSearchValue;
    private string _searchFilter;
    private string _previousSelectedItem;
    private string _selectedItem;
    private List<string> _multiSelection;

    private bool _showSelected;

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

        _multiSelection = new List<string>();
        _previousSelectedItem = string.Empty;
        _selectedItem = string.Empty;
        _selectionSource = source;
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

        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();

        _tempSearchValue = EditorGUILayout.TextField(_tempSearchValue);
        if (string.IsNullOrEmpty(_tempSearchValue))
            _searchFilter = string.Empty;

        if (GUILayout.Button("search",EditorStyles.miniButtonRight,GUILayout.MaxWidth(50))) {
            _searchFilter = _tempSearchValue;
        }
        
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("select all"))
        {
            foreach (var item in _selectionSource) {
                _items[item] = true;
            }
            ApplySelection(_selectionSource);
            return;
        }
        if (GUILayout.Button("clear selection")) {
            foreach (var item in _selectionSource)
            {
                _items[item] = false;
            }
            ApplySelection(new string[] { });
            return;
        }

        GUILayout.EndHorizontal();

        _showSelected = EditorGUILayout.ToggleLeft("show selected", _showSelected);

        EditorGUILayout.Space();
        EditorGUILayout.Separator();


        _scroll = GUILayout.BeginScrollView(_scroll);

        var selectionChanged = false;
        var selectedValue = string.Empty;

        foreach (var item in _items) {
            
            if (FilterShownItems(item.Key,item.Value) == false)
                continue;

            GUILayout.BeginHorizontal();

            var selection = GUILayout.Toggle(item.Value, item.Key);
            if (selection != _items[item.Key]) {
                selectedValue = item.Key;
                selectionChanged = true;
                _previousSelectedItem = _selectedItem;
                _selectedItem = item.Key;
            }

            GUILayout.EndHorizontal();
        }

        if (selectionChanged) {
            if (UpdateSelectionWithShift() == false) {
                _items[selectedValue] = !_items[selectedValue];               
            }
            ApplySelection(Selected);
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();

    }

    private bool UpdateSelectionWithShift() {
        _multiSelection.Clear();

        var keyEvent = Event.current;
        var isShiftDown = keyEvent.shift;
        if (!isShiftDown || string.IsNullOrEmpty(_selectedItem) ||
            string.IsNullOrEmpty(_previousSelectedItem))
            return false;
        var isSelectionActive = false;

        foreach (var item in _items)
        {
            var tag = item.Key;
            if (isSelectionActive && _previousSelectedItem != tag) {
                _multiSelection.Add(tag);
            }
            if (tag == _previousSelectedItem || tag == _selectedItem)
            {
                if (isSelectionActive) break;
                _multiSelection.Add(tag);
                isSelectionActive = true;
            }

        }

        foreach (var selection in _multiSelection) {
            _items[selection] = !_items[selection];
        }
        return true;
    }

    private bool FilterShownItems(string value, bool selected) {

        if (_showSelected && selected == false) return false;

        var filterResult = string.IsNullOrEmpty(_searchFilter) ||
                           value.IndexOf(_searchFilter, StringComparison.OrdinalIgnoreCase) >= 0;
        
        return filterResult;
    }

    private void ApplySelection(string[] selection) {
        if (_selectionChanged == null) return;
        _selectionChanged(selection);
    }

}
