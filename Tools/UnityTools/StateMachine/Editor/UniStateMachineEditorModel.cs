using System;
using System.Collections.Generic;
using Assets.Scripts.Tools.StateMachine;
using Tools.ReflectionUtils;

public static class UniFSMEditorModel {

    private static Lazy<List<Type>> _validators = 
	    new Lazy<List<Type>>(() => CreateTypes(typeof(UniNodeValidator)));
    private static Lazy<List<Type>> _behaviours = 
	    new Lazy<List<Type>>(() => CreateTypes(typeof(UniStateBehaviour)));
	
	public static List<Type> Validators => _validators.Value;

	public static List<Type> Behaviours => _behaviours.Value;

	public static void Update() {
	    UpdateTypeCache();
    }
    
    private static void UpdateTypeCache()
    {
	    if (_validators.IsValueCreated) {
		    _validators.Value.Clear();
		    _validators.Value.AddRange(CreateTypes(typeof(UniNodeValidator)));
	    }

	    if (_behaviours.IsValueCreated) {
		    _behaviours.Value.Clear();
		    _behaviours.Value.AddRange(CreateTypes(typeof(UniStateBehaviour)));
	    }
    }

	private static List<Type> CreateTypes(Type type) {

		return new List<Type>(ReflectionTools.
			GetAllChildTypes(type));

	}
}
