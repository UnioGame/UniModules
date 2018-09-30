using System;

namespace UnityToolsModule.Tools.UnityTools.ActorEntityModel 
{
    
    public class BehaviourObject : IBehaviourObject {

        #region public properties
        
        public bool IsActive { get; protected set; } = true;
        
        #endregion
        
        public void SetEnabled(bool state)
        {
            if (IsActive == state)
                return;
            if (state)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }

            IsActive = state;
        }

        
        public void Dispose()
        {
            
            if (IsActive)
                SetEnabled(false);
            
            OnDispose();
            
        }
        
        #region private methods

        
        protected virtual void Activate()
        {
        }

        protected virtual void Deactivate()
        {
            
        }

        protected virtual void OnDispose()
        {
        }

        #endregion
    
    }
    
}
