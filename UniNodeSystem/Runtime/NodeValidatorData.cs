namespace UniGreenModules.UniNodeSystem.Runtime
{
    using System;

    [Serializable]
    public class NodeValidatorData
    {
        public UniStateValidator Validator;
        
        public bool AllowNullContext = false;
        
        public bool DefaultValidationValue = true;

    }
}