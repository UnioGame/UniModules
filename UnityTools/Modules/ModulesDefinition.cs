using System.Collections.Generic;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.Modules;
using Assets.Tools.UnityTools.Interfaces;
using UniRx;
using UnityEngine;

namespace Assets.Tools.UnityTools.Moduls
{
    [CreateAssetMenu(menuName = "GameModules/ModuleDefinition", fileName = "ModuleDefinition")]
    public class ModulesDefinition : ScriptableObject
    {
        
        [SerializeField]
        private List<MessageConverter> _converters;
        
        
        public void Register(IContext moduleContext, IMessageBroker messageBroker)
        {
            var moduleLifeTime = moduleContext.LifeTime;
            for (int i = 0; i < _converters.Count; i++)
            {
                var converter = _converters[i];
                var disposable = converter.Register(messageBroker, moduleContext);
                moduleLifeTime.AddDispose(disposable);
            }
        }

    }
}
