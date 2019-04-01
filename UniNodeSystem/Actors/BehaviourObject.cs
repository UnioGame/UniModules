using UniModule.UnityTools.Interfaces;

namespace UniModule.UnityTools.ActorEntityModel 
{
    
    public class BehaviourObject : IBehaviourObject {

        #region public properties
        
        public bool IsActive { get; protected set; } = false;
        
        #endregion
        
        public void SetEnabled(bool state)
        {
            IsActive = state;
            
            if (state)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }

        }

        
        public void Release()
        {
            SetEnabled(false);
            OnReleased();
        }
        
        #region private methods

        
        protected virtual void Activate()
        {
        }

        protected virtual void Deactivate()
        {
            
        }

        protected virtual void OnReleased()
        {
        }

        #endregion
    
    }
    
}
