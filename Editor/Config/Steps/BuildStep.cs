namespace TCUnityBuild.Config.Steps
{
    public abstract class BuildStep : Step
    {
        public string Target; //-target
        public string BuildPath; //-out
        public string BuildNumber; //-buildNumber
        public string BuildVersion; //-buildVersion
        public bool Release; //-buildMode

        //			if (!string.IsNullOrEmpty(buildMode))
//			{
//				if (buildMode == "RELEASE")
//				{
//					EditorUserBuildSettings.allowDebugging = false;
//					EditorUserBuildSettings.development = false;
//					EditorUserBuildSettings.connectProfiler = false;
//					Debug.Log("Building a release version.");
//				}
//				else
//				{
//					Debug.LogError("Unknown build mode!");
//				}
//			}
        //todo makeTestBuild
    }

}