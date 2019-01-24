using System;
using System.Collections;
using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniModule.UnityTools.ScriptableObjects
{
    public abstract class ScriptableObjectRoutine<TContext> : 
        ScriptableObject ,IRoutine<TContext,IEnumerator>
    {
	
        [NonSerialized]
        private bool _initialized = false;
	
        public IEnumerator Execute(TContext context) {
		
            if (_initialized == false)
            {
                _initialized = true;
                OnInitialize();
            }

            yield return OnExecute(context);
		
        }
	
	
        #region private methods

        protected virtual void OnInitialize() {}

        protected abstract IEnumerator OnExecute(TContext context);

        #endregion


    }
}
