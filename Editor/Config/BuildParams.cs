using UnityEditor;

namespace TCUnityBuild.Config
{
    public class BuildParams
    {
        public string AndroidSdkPath; //-androidSDKPath;
        public string AndroidNdkPath; //-androidNDKPath;		
        public string JdkPath; //-jdkPath;
        public string KeystoreName; //-keystoreName
        public string KeystorePass; //-keystorePass
        public string KeyAliasName; //-keyaliasName
        public string KeyAliasPass; //-keyaliasPass

        public Defines Defines;

        public void Apply(IReporter reporter)
        {
            reporter.Log("Applying BuildParams");
            if (!string.IsNullOrEmpty(AndroidSdkPath))
            {
                reporter.Log("Android SDK path: " + AndroidSdkPath);
                SetAndroidSdkRoot(AndroidSdkPath);
            }

            if (!string.IsNullOrEmpty(AndroidNdkPath))
            {
                reporter.Log("Android NDK path: " + AndroidNdkPath);
                SetAndroidNdkRoot(AndroidNdkPath);
            }

            if (!string.IsNullOrEmpty(JdkPath))
            {
                reporter.Log("JDK path: " + JdkPath);
                SetJdkRoot(JdkPath);
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