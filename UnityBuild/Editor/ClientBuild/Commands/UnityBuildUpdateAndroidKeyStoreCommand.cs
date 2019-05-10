using Build;
using UnityEditor;

namespace Plavalaguna.Joy.Modules.UnityBuild.Commands {
    using UnityEngine;

    [CreateAssetMenu(menuName = "UnityBuild/Commands/Update Android KeyStore", fileName = "UpdateAndroidKeyStore")]
    public class UnityBuildUpdateAndroidKeyStoreCommand : UnityPreBuildCommand
    {
        
        //android keys
        public const string KeyStorePath      = "-keystorePath";
        public const string KeyStorePass      = "-keystorePass";
        public const string KeyStoreAlias     = "-keystoreAlias";
        public const string KeyStoreAliasPass = "-keystoreAliasPass";
        
        public override void Execute(BuildTarget target, 
            IArgumentsProvider arguments, 
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
