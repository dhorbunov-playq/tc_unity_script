using UnityEditor;

namespace Repository.TCUnityBuild.Editor
{
    public class EditorSetup {
        public static string AndroidSdkRoot {
            set { EditorPrefs.SetString("AndroidSdkRoot", value); }
        }

        public static string JdkRoot {
            set { EditorPrefs.SetString("JdkPath", value); }
        }

        // This requires Unity 5.3 or later
        public static string AndroidNdkRoot {
            set { EditorPrefs.SetString("AndroidNdkRoot", value); }
        }
    }

}