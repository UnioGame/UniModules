using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IEditorItem
{
    
    SerializedObject SerializedObject { get; }

}
