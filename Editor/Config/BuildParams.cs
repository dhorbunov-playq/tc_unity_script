using Repository.TCUnityBuild.Editor;
using UnityEditor;
using UnityEngine;

namespace TCUnityBuild.Config
{
    public class BuildParams
    {
        public string AndroidSdk;//-androidSDKPath;
        public string AndroidNdk;//-androidNDKPath;		
        public string Jdk;//-jdkPath;
        public string KeystoreName;//-keystoreName
        public string KeystorePass;//-keystorePass
        public string KeyAliasName;//-keyaliasName
        public string KeyAliasPass;//-keyaliasPass

        public Defines Defines;

        public void Apply()
        {
            Debug.Log("Applying BuildParams");
            if (!string.IsNullOrEmpty(AndroidSdk))
            {
                Debug.Log("Android SDK path: " + AndroidSdk);
                EditorSetup.AndroidSdkRoot = AndroidSdk;
            }
            if (!string.IsNullOrEmpty(AndroidNdk))
            {
                Debug.Log("Android NDK path: " + AndroidNdk);
                EditorSetup.AndroidNdkRoot = AndroidNdk;
            }
            if (!string.IsNullOrEmpty(Jdk))
            {
                Debug.Log("JDK path: " + Jdk);
                EditorSetup.JdkRoot = Jdk;
            }
            if (!string.IsNullOrEmpty(KeystoreName))
            {
                PlayerSettings.Android.keystoreName = KeystoreName;
            }

            if (!string.IsNullOrEmpty(KeystorePass))
            {
                PlayerSettings.Android.keystorePass = KeystorePass;
            }

            if (!string.IsNullOrEmpty(KeyAliasName))
            {
                PlayerSettings.Android.keyaliasName = KeyAliasName;
            }

            if (!string.IsNullOrEmpty(KeyAliasPass))
            {
                PlayerSettings.Android.keyaliasPass = KeyAliasPass;
            }

            Defines.Apply();
            Debug.Log("Applying BuildParams is done!");
        }
    }
}