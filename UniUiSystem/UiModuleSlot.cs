using UniModule.UnityTools.DataFlow;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UniUiSystem.Models;

namespace UniUiSystem
{
    /// <summary>
    /// Ui module container
    /// </summary>
    public class UiModuleSlot : UIBehaviour, IUiModuleSlot
    {
        public string SlotName => name;
        
        public RectTransform Transform => transform as RectTransform;
        
        public void ApplySlotData(IContext context,IContextData<IContext> container,ILifeTime lifeTime)
        {
            
            var moduleData = CreateModuleData(context);
           
            lifeTime.AddCleanUpAction(() => moduleData.Despawn());
            container.UpdateValue(context,moduleData);

            UpdateContext(container,lifeTime);
            
        }

        /// <summary>
        /// create slot  
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected virtual UniUiModuleData CreateModuleData(IReadOnlyContext source)
        {
            var data = ClassPool.Spawn<UniUiModuleData>();
            data.Transform.Value = Transform;
            return data;
        }

        //update slot context data object
        protected virtual void UpdateContext(IContextData<IContext> container, ILifeTime lifeTime){}
    }
    
}
