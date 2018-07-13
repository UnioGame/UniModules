using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BundleGroupMap))]
public class BundleGroupEditor : Editor {

    private static string[] _bundleTags;
    private int _selectedEnumItem;
    private Vector2 _scroll;
    private BundleGroupItem _removed;
    private int _miniButtonMinWidth = 10;
    private int _miniButtonMaxWidth = 50;
    private ToggleWindow _toggleWindow;
    private GUILayoutOption[] _buttonOptions = new GUILayoutOption[] {
        GUILayout.MinWidth(30), GUILayout.MaxWidth(30)
    }; 

    public override void OnInspectorGUI() {
        
        if (_bundleTags == null) {
            _bundleTags = AssetDatabase.GetAllAssetBundleNames();
        }

        _scroll = EditorGUILayout.BeginScrollView(_scroll);

        var groupObject = serializedObject.targetObject as BundleGroupMap;


        if (groupObject.BundleGroups == null) {
            groupObject.BundleGroups = new List<BundleGroupItem>();
        }
        var groups = groupObject.BundleGroups;

        EditorGUILayout.BeginVertical();

        for (int i = 0; i < groups.Count; i++) {
            var group = groups[i];
            DrawInsertItem(group);
            EditorGUILayout.Separator();
            EditorGUILayout.Space();
        }

        if (_removed != null) {
            groups.Remove(_removed);
        }
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Separator();

        if(GUILayout.Button("Add Group"))
        {
            groupObject.BundleGroups.Add(new BundleGroupItem());
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();

        serializedObject.ApplyModifiedPropertiesWithoutUndo();
        EditorUtility.SetDirty(serializedObject.targetObject);

    }

    private void DrawInsertItem(BundleGroupItem groupItem) {

        GUILayout.BeginHorizontal();

        var groupTag = EditorGUILayout.EnumPopup("Bundle group:", groupItem.Group);
        groupItem.Group = (BundleGroup)groupTag;
        if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.MinWidth(_miniButtonMinWidth))) {
            _removed = groupItem;
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Bundle list", EditorStyles.miniButtonLeft, GUILayout.MinWidth(_miniButtonMinWidth))) {
            ShowBundleWindow(groupItem,_bundleTags, groupItem.Bundles.ToArray());
        }
        //_selectedEnumItem = EditorGUILayout.Popup(_selectedEnumItem, _bundleTags);
        //if (GUILayout.Button("+", EditorStyles.miniButtonRight, GUILayout.MinWidth(_miniButtonWidth)))
        //{
        //    var tag = _bundleTags[_selectedEnumItem];
        //    if (groupItem.Bundles.Contains(tag) == false)
        //        groupItem.Bundles.Add(tag);
        //}

        GUILayout.EndHorizontal();

        var removedItem = string.Empty;

        var items = groupItem.Bundles.ToArray();
        foreach (var bundle in items) {
            GUILayout.BeginHorizontal();

            GUILayout.Label(bundle);
            if (GUILayout.Button("-", EditorStyles.miniButtonRight,_buttonOptions)) {
                removedItem = bundle;
            }

            GUILayout.EndHorizontal();
        }

        groupItem.Bundles.RemoveAll(x => x == removedItem);

    }

    private ToggleWindow ShowBundleWindow(BundleGroupItem item,string[] source, string[] selected) {

        if (_toggleWindow == null) {
            _toggleWindow = ToggleWindow.Show(source,selected,x => OnBundleSelected(item, x));
        }
        else {
            _toggleWindow.Initialize(source, selected, x => OnBundleSelected(item, selected));
        }
        return _toggleWindow;
    }

    private void OnBundleSelected(BundleGroupItem groupItem, string[] selection) {

        groupItem.Bundles.Clear();
        groupItem.Bundles.AddRange(selection);

    }
}
