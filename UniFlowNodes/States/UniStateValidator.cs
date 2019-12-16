namespace UniGreenModules.UniFlowNodes.States
{
    using UniCore.Runtime.Interfaces;
    using UnityEngine;

    public class UniStateValidator : ScriptableObject , IValidator<IContext>
    {
        [SerializeField]
        protected bool _defaultValidatorValue = false;


        public virtual bool Validate(IContext data)
        {
            return _defaultValidatorValue;
        }

    }
}