/// <summary>
/// ButtonMethodAttribute,
/// modified from https://github.com/Deadcows/MyBox/blob/master/Attributes/ButtonMethodAttribute.cs
/// </summary>
using UnityEngine;

namespace UnityAtlasGenerator.Helper.Internal
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Editor.Helper;
	using UnityEditor;


	[CustomEditor(typeof(AtlasGeneratorSettings), true), CanEditMultipleObjects]
	public class AtlasGeneratorSettingsEditor : Editor
	{
		private List<MethodInfo> _methods;
		private AtlasGeneratorSettings _target;
		private AtlasGeneratorOdinHandler _drawer;


		private void OnEnable()
		{
			_target = target as AtlasGeneratorSettings;
			_drawer = _drawer ?? new AtlasGeneratorOdinHandler();
			if (_target == null) return;

			_drawer.Initialize(_target);
			_methods = AtlasGeneratorMethodHandler.CollectValidMembers(_target.GetType());

		}

		private void OnDisable()
		{
			_drawer.Dispose();
		}

		public override void OnInspectorGUI()
		{
			DrawBaseEditor();

#if !ODIN_INSPECTOR
			if (_methods == null) return;

			AddressableImporterMethodHandler.OnInspectorGUI(_target, _methods);
#endif

			serializedObject.ApplyModifiedProperties();

		}

		private void DrawBaseEditor()
		{
#if ODIN_INSPECTOR
			_drawer.Draw();
#else
			base.OnInspectorGUI();
#endif
		}
	}
}