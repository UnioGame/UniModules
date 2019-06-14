namespace UniGreenModules.UniUiSystem.Runtime
{
    using GBG.UI.Runtime;
    using Interfaces;
    using UniContextData.Runtime.Entities;
    using UniCore.Runtime.Interfaces;
    using UniRx;

    public class UiViewBehaviour : UiView<Unit>, IUiViewBehaviour
    {
        private EntityContext _context = new EntityContext();


        #region public property

        public IContext Context => _context;

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
