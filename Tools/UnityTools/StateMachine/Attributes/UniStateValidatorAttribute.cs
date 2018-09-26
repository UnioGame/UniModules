using System;

namespace UniStateMachine.Attributes
{
	
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class UniStateValidatorAttribute : Attribute
	{

		private Type _validatorType = typeof(UniTransitionValidator);
		
		public UniStateValidatorAttribute(Type validatorType)
		{
			if (_validatorType.IsAssignableFrom(validatorType) == false)
				return;

			if (UniFSMEditorModel.Validators.Contains(validatorType)) return;
			UniFSMEditorModel.Validators.Add(validatorType);
			
		}
		
	}
	
}

