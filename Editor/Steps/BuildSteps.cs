using System;
using System.Linq;
using System.Reflection;
using PlayQ.Build;
using UnityEditor;
using UnityEngine;

namespace TCUnityBuild.Config.Steps
{
    public abstract class BuildStep : Step
    {
        public string BuildPath; //-out
        public int? BuildNumber; //-buildNumber
        public string BuildVersion; //-buildVersion
        public bool Release; //-buildMode

        private void Prepare()
        {
            if (BuildNumber != null)
            {
                BundleVersionResolver.BuildNumber = BuildNumber;
            }

            if (string.IsNullOrEmpty(BuildVersion))
            {
                BundleVersionResolver.PrettyVersion = BuildVersion;
            }

            if (Release)
            {
                EditorUserBuildSettings.allowDebugging = false;
                EditorUserBuildSettings.development = false;
                EditorUserBuildSettings.connectProfiler = false;
                Debug.Log("Building a release version.");
            }
        }

        private void Build()
        {
            //					BuildTarget parsedBuildTarget = (BuildTarget) Enum.Parse(typeof(BuildTarget), buildTarget);
//					Debug.Log("Parsed build target: " + parsedBuildTarget);
//					MobileTextureSubtarget? parsedTextureSubtarget = null;
////					if (!string.IsNullOrEmpty(androidTextureCompression))
////						parsedTextureSubtarget = (MobileTextureSubtarget) Enum.Parse(typeof(MobileTextureSubtarget),
////							androidTextureCompression);
////
////					BundleVersionResolver.Setup(parsedBuildTarget);
////					if (string.IsNullOrEmpty(bundleExclusionCommand) || !bool.Parse(bundleExclusionCommand))
////						BuildAndBakeAssetBundles();
//
//					Builder.Build(parsedBuildTarget, publishPath, parsedTextureSubtarget);
//					ExecutePostSteps(parsedBuildTarget, publishPath);

        }
        
        public static void ExecutePostSteps(BuildTarget target, string builtPath)
        {
            Debug.Log("ExecutePostSteps Fetching PostBuild steps for target " + target.ToString() + ". Path: " +
                      builtPath);
            var methods = Assembly.GetExecutingAssembly()
                .GetTypes().SelectMany(allTypes => allTypes.GetMethods(), (allTypes, method) => new {allTypes, method})
                .Where(@t =>
                    @t.method.GetCustomAttributes(typeof(UnityEditor.Callbacks.PostProcessBuildAttribute), false)
                        .Count(atr => atr.GetType() == typeof(TCUnityBuild).Assembly.GetType("BuildConfig")) > 0 &&
                    @t.method.IsStatic)
                .Select(@t => @t.method);

            Debug.Log("Executing PostBuild steps...");
            foreach (var method in methods)
            {
                Debug.Log("ExecutePostSteps calling : " + method);
                try
                {
                    method.Invoke(null, new object[] {target, builtPath});
                }
                catch (Exception ex)
                {
                    Debug.Log("ExecutePostSteps Exception !!! : " + ex);
                }
            }

            Debug.Log("ExecutePostSteps DONE");
        }
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
        public static void PrepareTestBuild()
        {
            Debug.Log("Making test build!");

            var allTypes = Assembly.GetExecutingAssembly().GetTypes();
            var testToolBuildScriptType = allTypes
                .FirstOrDefault(type => type.Name.Equals("TestToolBuildScript", StringComparison.Ordinal));
            if (testToolBuildScriptType != null)
            {
                var testBuildMethod = testToolBuildScriptType.GetMethod("PrepareBuild", BindingFlags.Static |
                                                                                        BindingFlags.NonPublic |
                                                                                        BindingFlags.Public);

                if (testBuildMethod != null)
                {
                    Debug.Log("PrepareBuild: invoking testBuildMethod");
                    testBuildMethod.Invoke(null, new object[] { });
                }
                else
                {
                    Debug.Log("Method PrepareBuild was not found in type. " +
                              testToolBuildScriptType);
                }
            }
            else
            {
                Debug.Log("Type 'TestToolBuildScript' was not found in assembly. " +
                          allTypes.Aggregate("All types: ",
                              (concatinated, type) => { return concatinated + "; " + type; }));
            }
        }
        
        
        public override void Run(IReporter reporter)
        {
            PrepareTestBuild();
            throw new System.NotImplementedException("Test Build is not implemented yet.");
        }
    }
}