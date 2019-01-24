using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniRx;
using UnityEngine;

namespace UniModule.UnityTools.Modules
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
