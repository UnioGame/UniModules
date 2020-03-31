using System;
using System.Collections.Generic;
using UniGame.UniNodes.NodeSystem.Inspector.Editor.ContentContextWindow;
using UniGreenModules.UniContextData.Runtime.Entities;
using UniGreenModules.UniGame.Core.EditorTools.Editor.Controls.AssetDropDownControl.Examples;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class DemoContextClass
{
    public List<IntReactiveProperty> intReactiveProperties = new List<IntReactiveProperty>() {
        new IntReactiveProperty(77),
        new IntReactiveProperty(44),
        new IntReactiveProperty(88),
        new IntReactiveProperty(99),
    };
    
    public int One = 3;

    public float FloatValue = 4f;

    public ScriptableObject SOValue;

    public string Text = "DemoContextClass Text0";
}

[CreateAssetMenu(menuName = "UniGame/GameSystem/Examples/ContextDataWindowData",fileName = "ContextDataWindowData")]
public class DemoTypeDataContainer : ScriptableObject
{
    public EntityContext innerContext = new EntityContext();
    
    public ScriptableObject so;

    public List<int> intListValue = new List<int>(){5,4,33,222,1};

    public List<Vector3> vectorItems = new List<Vector3>();
    
    public List<ContextDemoAsset> soItems = new List<ContextDemoAsset>();
    
    public List<GameObject> gameObjects = new List<GameObject>();
    
    public List<Object> objects = new List<Object>();
    
    public GameObject go;

    public Sprite sprite;

    public Texture texture;

    public string stringValue;

    public Vector2 vector2Value;

    public Vector3 vector3Value;

    public DemoContextClass serializableClassValue = new DemoContextClass();
    
    [ContextMenu("ShowContextWindow")]
    public void ShowContextWindow()
    {
        Show();
    }

    [Sirenix.OdinInspector.Button]
    public void Show()
    {
        var data = new EntityContext();
        data.Publish(serializableClassValue);
        data.Publish(so);
        data.Publish(intListValue);
        data.Publish(go);
        data.Publish(sprite);
        data.Publish(texture);
        data.Publish(vector2Value);
        data.Publish(vector3Value);
        data.Publish(stringValue);
        data.Publish(gameObjects);
        data.Publish(objects);
        data.Publish(soItems);
        data.Publish(vectorItems);
        
        //inner context
        innerContext = new EntityContext();
        innerContext.Publish(new DemoContextClass());
        innerContext.Publish("INNER CONTEXT");
        innerContext.Publish(6666666666666);
        innerContext.Publish(true);
        innerContext.Publish(new List<string>() {
            "odwda","awfasf","xxxx","ooo"
        });
        
        data.Publish(innerContext);
        
        ContextContentWindow.Open(new ContextDescription() {
            Data = data,
            Label = nameof(DemoTypeDataContainer)
        });
    }
    
}
