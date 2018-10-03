using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssetBundlesModule;
using Assets.Tools.UnityTools.AssetBundleManager;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BundleGroupMap))]
public class BundleGroupEditor : Editor {

    private static List<string> _bundleTags;
    private static string[] _resourceGroupTags;

    private Vector2 _scroll;
    private BundleGroupItem _removed;
    private int _miniButtonMinWidth = 10;
    private ToggleWindow _toggleWindow;

    private GUILayoutOption[] _buttonOptions = new GUILayoutOption[] {
        GUILayout.MinWidth(30), GUILayout.MaxWidth(30)
    }; 

    public override void OnInspectorGUI() {
        
        if (_bundleTags == null) {
            _bundleTags = AssetDatabase.GetAllAssetBundleNames().ToList();
        }

        if (_resourceGroupTags == null) {
            _resourceGroupTags = Enum.GetNames(typeof(RuntimePlatform)).ToArray();
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

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel++;
            DrawInsertItem(group);
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

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

    private void OnGroupPlatformSelected(BundleGroupItem groupItem, RuntimePlatform[] tags) {
        groupItem.TargetPlatforms.Clear();
        groupItem.TargetPlatforms.AddRange(tags);
    }

    private void DrawPlatformSelection(BundleGroupItem groupItem) {

        if (GUILayout.Button("select runtime platforms", EditorStyles.miniButton))
        {
            var selectedItems = groupItem.TargetPlatforms.Select(x => x.ToString()).ToArray();
            ToggleWindow.Show(_resourceGroupTags, selectedItems, x =>
                OnGroupPlatformSelected(groupItem,
                    x.Select(y => (RuntimePlatform)Enum.Parse(typeof(RuntimePlatform), y)).ToArray()));
        }

        groupItem.ShowRuntimePlatforms = EditorGUILayout.Foldout(groupItem.ShowRuntimePlatforms, "plaforms");
        if (groupItem.ShowRuntimePlatforms) {
            var textValuesStrings = groupItem.TargetPlatforms.Select(x => x.ToString()).ToArray();
            var textValue = string.Join("\n", textValuesStrings);
            EditorGUILayout.TextArea(textValue);
        }
        
    }

    private void DrawInsertItem(BundleGroupItem groupItem) {

        DrawPlatformSelection(groupItem);

        EditorGUILayout.BeginHorizontal();

        var groupTag = EditorGUILayout.EnumPopup("Bundle group:", groupItem.GroupTag);
        groupItem.GroupTag = (ResourceGroupTag)groupTag;

        groupItem.ForceUnload = GUILayout.Toggle(groupItem.ForceUnload,"force", EditorStyles.miniButtonMid);
        groupItem.ForceUnloadMode = GUILayout.Toggle(groupItem.ForceUnloadMode, "mode", EditorStyles.miniButtonMid);

        if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.MinWidth(_miniButtonMinWidth))) {
            _removed = groupItem;
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Bundle list", EditorStyles.miniButton)) {
            _toggleWindow = ShowBundleWindow(groupItem,_bundleTags.ToArray(), groupItem.Bundles.ToArray());
        }

        GUILayout.EndHorizontal();

        var removedItem = string.Empty;

        var items = groupItem.Bundles.ToArray();

        groupItem.ShowBundlesInput = EditorGUILayout.Foldout(groupItem.ShowBundlesInput, "bundles input");
        if (groupItem.ShowBundlesInput) {

            var bundleText = string.Join("\n", items);
            GUILayout.BeginVertical();

            var newText = EditorGUILayout.TextArea(bundleText);
            if (newText != bundleText)
                ApplyTags(newText, groupItem);
            GUILayout.EndVertical();

        }

        groupItem.ShowBundles = EditorGUILayout.Foldout(groupItem.ShowBundles, "show bundles");
        if (groupItem.ShowBundles) {
            foreach (var bundle in items) {
                GUILayout.BeginHorizontal();

                GUILayout.Label(bundle);
                if (GUILayout.Button("-", EditorStyles.miniButtonRight, _buttonOptions)) {
                    removedItem = bundle;
                }

                GUILayout.EndHorizontal();
            }
            
            groupItem.Bundles.RemoveAll(x => x == removedItem);
        }
        
    }

    private void ApplyTags(string bundleText, BundleGroupItem groupItem) {
        bundleText = bundleText.Replace(" ", "");
        var bundles = bundleText.Split('\n');
        OnBundleSelected(groupItem, bundles.Where(x => _bundleTags.Contains(x)).ToArray());
    }

    private ToggleWindow ShowBundleWindow(BundleGroupItem item, string[] source, string[] selected) {

        var sourceArray = source.ToArray();
        if (_toggleWindow == null) {
            _toggleWindow = ToggleWindow.Show(sourceArray, selected,x => OnBundleSelected(item, x));
        }
        else {
            _toggleWindow.Initialize(sourceArray, selected, x => OnBundleSelected(item, selected));
        }
        return _toggleWindow;
    }

    private void OnBundleSelected(BundleGroupItem groupItem, string[] selection) {

        groupItem.Bundles.Clear();
        groupItem.Bundles.AddRange(selection);

    }
}
