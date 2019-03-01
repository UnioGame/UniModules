using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniNodeSystemEditor 
{
	/// <summary> Handles caching of custom editor classes and their target types. Accessible with GetEditor(Type type) </summary>
	public class NodeEditorBase<T, A, K> where A : Attribute, 
		INodeEditorAttribute where T : NodeEditorBase<T, A, K> where K : Object
	{
		/// <summary> Custom editors defined with [CustomNodeEditor] </summary>
		private static Dictionary<Type, Type> _editorsTypesMap;

		public virtual void OnEnable()
		{
			
		}
		
		private static Dictionary<Type, Type> editorTypes
		{
			get
			{
				if (_editorsTypesMap == null)
				{
					CacheCustomEditors();
				}
				return _editorsTypesMap;
			}
			set { _editorsTypesMap = value; }
		}
		
		private static Dictionary<K, T> editors = new Dictionary<K, T>();
		public K target;
		public SerializedObject serializedObject;

		public static T GetEditor(K target) {
			if (target == null) return null;
			T editor;
			if (!editors.TryGetValue(target, out editor)) {
				Type type = target.GetType();
				Type editorType = GetEditorType(type);
				editor = Activator.CreateInstance(editorType) as T;
				editor.target = target;
				editor.serializedObject = new SerializedObject(target);
				editors.Add(target, editor);
			}
			if (editor.target == null) editor.target = target;
			if (editor.serializedObject == null) editor.serializedObject = new SerializedObject(target);
			return editor;
		}

		private static Type GetEditorType(Type type) {
			if (type == null) return null;
			if (editorTypes == null) CacheCustomEditors();
			Type result;
			if (editorTypes.TryGetValue(type, out result)) return result;
			//If type isn't found, try base type
			return GetEditorType(type.BaseType);
		}

		private static void CacheCustomEditors() {
			editorTypes = new Dictionary<Type, Type>();

			//Get all classes deriving from NodeEditor via reflection
			Type[] nodeEditors = UniNodeSystemEditor.NodeEditorWindow.GetDerivedTypes(typeof(T));
			for (int i = 0; i < nodeEditors.Length; i++) {
				if (nodeEditors[i].IsAbstract) continue;
				var attribs = nodeEditors[i].GetCustomAttributes(typeof(A), false);
				if (attribs == null || attribs.Length == 0) continue;
				A attrib = attribs[0] as A;
				editorTypes.Add(attrib.GetInspectedType(), nodeEditors[i]);
			}
		}
	}
}