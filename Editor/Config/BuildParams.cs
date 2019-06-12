using UnityEditor;

namespace TCUnityBuild.Config
{
    public class BuildParams
    {
        public string AndroidSdk; //-androidSDKPath;
        public string AndroidNdk; //-androidNDKPath;		
        public string Jdk; //-jdkPath;
        public string KeystoreName; //-keystoreName
        public string KeystorePass; //-keystorePass
        public string KeyAliasName; //-keyaliasName
        public string KeyAliasPass; //-keyaliasPass

        public Defines Defines;

        public void Apply(IReporter reporter)
        {
            reporter.Log("Applying BuildParams");
            if (!string.IsNullOrEmpty(AndroidSdk))
            {
                reporter.Log("Android SDK path: " + AndroidSdk);
                SetAndroidSdkRoot(AndroidSdk);
            }

            if (!string.IsNullOrEmpty(AndroidNdk))
            {
                reporter.Log("Android NDK path: " + AndroidNdk);
                SetAndroidNdkRoot(AndroidNdk);
            }

            if (!string.IsNullOrEmpty(Jdk))
            {
                reporter.Log("JDK path: " + Jdk);
                SetJdkRoot(Jdk);
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

            Defines.Apply(reporter);
            reporter.Log("Applying BuildParams is done!");
        }

        private static void SetAndroidSdkRoot(string value)
        {
            EditorPrefs.SetString("AndroidSdkRoot", value);
        }

        private static void SetJdkRoot(string value)
        {
            EditorPrefs.SetString("JdkPath", value);
        }

        private static void SetAndroidNdkRoot(string value)
        {
            EditorPrefs.SetString("AndroidNdkRoot", value);
        }
    }
}