using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Tools.StateMachine;
using Tools.ReflectionUtils;
using UniStateMachine;
using UnityEditor;

public static class UniFSMEditorModel {
    private static Lazy<List<Type>> _validators =
        new Lazy<List<Type>>(() => CreateTypes(typeof(UniTransitionValidator)));

    private static Lazy<List<Type>> _behaviours =
        new Lazy<List<Type>>(() => CreateTypes(typeof(UniStateBehaviour)));

    public static List<Type> Validators => _validators.Value;


    private static string[] _validatorsNames;

    public static string[] ValidatorsNames
    {
        get
        {
            if (_validatorsNames == null) {
                _validatorsNames = _validators.Value.Select(x => x.Name).ToArray();
            }

            return _validatorsNames;
        }
    }

    public static List<Type> Behaviours => _behaviours.Value;

    private static string[] _behavioursNames;

    public static string[] BehavioursNames
    {
        get
        {
            if (_behavioursNames == null) {
                _behavioursNames = _behaviours.Value.Select(x => x.Name).ToArray();
            }

            return _behavioursNames;
        }
    }

    public static void Update() {
        UpdateTypeCache();
        _validatorsNames = null;
        _behavioursNames = null;
    }

    [MenuItem("UniFSM/Update Cache")]
    private static void UpdateTypeCache() {

        _validators.Value.Clear();

        var types = CreateTypes(typeof(UniTransitionValidator));
        _validators.Value.AddRange(types);

        _behaviours.Value.Clear();
        types = CreateTypes(typeof(UniStateBehaviour));
        _behaviours.Value.AddRange(types);
        
    }

    private static List<Type> CreateTypes(Type type) {
        return new List<Type>(ReflectionTools.GetAllChildTypes(type));
    }
}