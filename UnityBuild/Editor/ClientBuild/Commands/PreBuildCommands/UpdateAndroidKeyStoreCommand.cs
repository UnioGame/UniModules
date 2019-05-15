namespace UniGreenModules.UnityBuild.Editor.ClientBuild.Commands.PreBuildCommands {
    using Interfaces;
    using UnityEditor;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UnityBuild/Commands/Update Android KeyStore", fileName = "UpdateAndroidKeyStore")]
    public class UpdateAndroidKeyStoreCommand : UnityPreBuildCommand
    {
        
        //android keys
        public const string KeyStorePath      = "-keystorePath";
        public const string KeyStorePass      = "-keystorePass";
        public const string KeyStoreAlias     = "-keystoreAlias";
        public const string KeyStoreAliasPass = "-keystoreAliasPass";
        
        public override void Execute(IArgumentsProvider arguments, 
            IBuildParameters buildParameters) 
        {
            UpdateAndroidBuildParameters(arguments);
        }
        
        public void UpdateAndroidBuildParameters(IArgumentsProvider arguments) {

            //update android key store parameters
            arguments.GetStringValue(KeyStorePath, 
                out var keystore, string.Empty);
            
            if (string.IsNullOrEmpty(keystore))
                return;
            
            arguments.GetStringValue(KeyStorePass,
                out var keypass, PlayerSettings.Android.keystorePass);

            arguments.GetStringValue(KeyStoreAlias,
                out var alias, PlayerSettings.Android.keyaliasName);

            arguments.GetStringValue(KeyStoreAliasPass,
                out var aliaspass, PlayerSettings.Android.keyaliasPass);
   
            PlayerSettings.Android.keystorePass = keypass;
            PlayerSettings.Android.keystoreName = keystore;
            PlayerSettings.Android.keyaliasName = alias;
            PlayerSettings.Android.keyaliasPass = aliaspass;
        }
    }
}
