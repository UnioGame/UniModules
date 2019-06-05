using System.Collections;
using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniUiSystem
{
    using UniGreenModules.GBG.UiManager.Runtime;
    using UniGreenModules.UniContextData.Runtime.Entities;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniRx;
    using UniTools.UniRoutine.Runtime;

    public class UiViewBehaviour : UiView<Unit>, IUiViewBehaviour
    {
        private EntityContext _context = new EntityContext();


        #region public property


        public IContext Context => _context;

        public RectTransform RectTransform => transform as RectTransform;

        #endregion
        
        #region public methods

        public void Initialize()
        {
            OnInitialize();
        }

        #endregion
       
        
        protected virtual void OnInitialize()
        {

        }

     
        
    }
}
