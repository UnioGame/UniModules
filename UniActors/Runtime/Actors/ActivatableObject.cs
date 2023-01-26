﻿namespace UniModules.UniActors.Runtime.Actors 
{
    using global::UniGame.Core.Runtime;

    public class ActivatableObject : IActivatableObject {

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
