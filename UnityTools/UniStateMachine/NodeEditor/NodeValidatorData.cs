using System;

namespace UniStateMachine
{
    [Serializable]
    public class NodeValidatorData
    {
        public UniStateValidator Validator;
        
        public bool AllowNullContext = false;
        
        public bool DefaultValidationValue = true;

    }
}