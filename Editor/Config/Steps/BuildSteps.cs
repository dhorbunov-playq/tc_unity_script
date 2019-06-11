using UnityEngine;
using ILogger = NUnit.Framework.Internal.ILogger;

namespace TCUnityBuild.Config.Steps
{
    public abstract class BuildStep : Step
    {
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

    public class AndroidBuildStep : BuildStep
    {
        public override void Run(IReporter reporter)
        {
            throw new System.NotImplementedException("Android Build is not implemented yet.");
        }
    }
    public class AmazoneBuildStep : BuildStep
    {
        public override void Run(IReporter reporter)
        {
            throw new System.NotImplementedException("Amazone Build is not implemented yet.");
        }
    }
    public class iOSBuildStep : BuildStep
    {
        public override void Run(IReporter reporter)
        {
            throw new System.NotImplementedException("iOs Build is not implemented yet.");
        }
    }
    
    public class TestBuildStep : BuildStep
    {
        public override void Run(IReporter reporter)
        {
            throw new System.NotImplementedException("Test Build is not implemented yet.");
        }
    }
}