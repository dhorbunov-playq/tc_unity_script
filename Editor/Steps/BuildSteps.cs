using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Boo.Lang;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace TCUnityBuild.Config.Steps
{
    public abstract class BuildStep : Step
    {
        public string BuildPath; //-out
        public int? BuildNumber; //-buildNumber
        public string BuildVersion; //-buildVersion
        public bool Release; //-buildMode

        protected virtual void Prepare(IReporter reporter)
        {
            Directory.CreateDirectory(BuildPath);
            
            if (BuildNumber != null)
            {
                SetupBundleVersion(BuildNumber.Value);
            }

            if (!string.IsNullOrEmpty(BuildVersion))
            {
                SetupPrettyVersion(BuildVersion);
            }

            if (Release)
            {
                EditorUserBuildSettings.allowDebugging = false;
                EditorUserBuildSettings.development = false;
                EditorUserBuildSettings.connectProfiler = false;
                reporter.Log("Building a release version!");
            }
        }

        protected virtual void SetupBundleVersion(int bundleVersion)
        {
            PlayerSettings.bundleVersion = bundleVersion.ToString();
        }

        protected virtual void SetupPrettyVersion(string prettyVersion)
        {
            PlayerSettings.bundleVersion = prettyVersion;
        }
        
        /// <summary>
        /// Returns a list of all the enabled scenes.
        /// </summary>
        private static List<string> GetEnabledScenePaths(IReporter reporter)
        {
            List<string> scenePaths = new List<string>();

            reporter.Log("Enabled Scene paths: ");
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    reporter.Log(scene.path);
                    scenePaths.Add(scene.path);
                }
            }

            return scenePaths;
        }
        
        protected void Build(BuildTarget target, IReporter reporter)
        {
            reporter.Log("Preparing to Build.");
            Prepare(reporter);
            
            reporter.Log("Switching Platform to " + target);
            EditorUserBuildSettings.SwitchActiveBuildTarget(target);
            
			reporter.Log("Build started. Target: " + target);

            BuildReport buildReport = BuildPipeline.BuildPlayer(GetEnabledScenePaths(reporter).ToArray(), BuildPath,
                target, BuildOptions.None);
            switch (buildReport.summary.result)
            {
                case BuildResult.Succeeded:
                    reporter.LogSuccess("Build successful!");
                    break;
                case BuildResult.Cancelled:
                    reporter.LogFail("Build was canceled!");
                    break;
                case BuildResult.Failed:
                    reporter.LogError("Total errors: " + buildReport.summary.totalErrors);
                    reporter.LogFail(
                        "*** Error(s): Unity build player exited with errors. If you dont see any errors in the logs, go into Unity and do a manual export of the project for the current target and look into Unity console for errors.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Can't proccess build result: " + buildReport.summary.result);
            }
            
            reporter.Log("ExecutePostSteps Fetching PostBuild steps for target " + target.ToString() + ". Path: " +
                         BuildPath);
            ExecutePostSteps(target, BuildPath, reporter);
        }
        
        private static void ExecutePostSteps(BuildTarget target, string builtPath, IReporter reporter)
        {
            var methods = Assembly.GetExecutingAssembly()
                .GetTypes().SelectMany(allTypes => allTypes.GetMethods(), (allTypes, method) => new {allTypes, method})
                .Where(@t =>
                    @t.method.GetCustomAttributes(typeof(UnityEditor.Callbacks.PostProcessBuildAttribute), false)
                        .Count(atr => atr.GetType() == typeof(TCUnityBuild).Assembly.GetType("BuildConfig")) > 0 &&
                    @t.method.IsStatic)
                .Select(@t => @t.method);

            reporter.Log("Executing PostBuild steps...");
            foreach (var method in methods)
            {
                reporter.Log("ExecutePostSteps calling : " + method);
                try
                {
                    method.Invoke(null, new object[] {target, builtPath});
                }
                catch (Exception ex)
                {
                    reporter.LogError("ExecutePostSteps Exception : " + ex);
                }
            }

            reporter.Log("ExecutePostSteps DONE");
        }
    }

    public class AndroidBuildStep : BuildStep
    {
        public string TextureCompression;

        protected override void Prepare(IReporter reporter)
        {
            base.Prepare(reporter);
            if (!string.IsNullOrEmpty(TextureCompression))
            {
                var parsedTextureSubtarget = (MobileTextureSubtarget) Enum.Parse(typeof(MobileTextureSubtarget),
                    TextureCompression);
                EditorUserBuildSettings.androidBuildSubtarget = parsedTextureSubtarget;
            }
            
            //Enable Gradle and Proguard
            if (PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android) == ScriptingImplementation.IL2CPP)
            {
                EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

                var templatePath = Application.dataPath + "/Plugins/Android/UnityProGuardTemplate.txt";
                if (!File.Exists(templatePath))
                {
                    throw new FileNotFoundException(templatePath);
                }

                var osSpecific = SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows ? "Data" : "";
                var installPath = EditorApplication.applicationPath + "/../" + osSpecific +
                                  "/PlaybackEngines/AndroidPlayer/Tools/UnityProGuardTemplate.txt";
                if (File.Exists(installPath))
                {
                    File.Delete(installPath);
                }

                File.Copy(templatePath, installPath);
            }
        }

        public override void Run(IReporter reporter)
        {
            Build(BuildTarget.Android, reporter);
            
        }
        protected override void SetupBundleVersion(int bundleVersion)
        {
            base.SetupBundleVersion(bundleVersion);
            PlayerSettings.Android.bundleVersionCode = bundleVersion;
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
        protected override void SetupBundleVersion(int bundleVersion)
        {
            base.SetupBundleVersion(bundleVersion);
            PlayerSettings.iOS.buildNumber = bundleVersion.ToString();
        }
        
        public override void Run(IReporter reporter)
        {
            throw new System.NotImplementedException("iOs Build is not implemented yet.");
        }
    }
    
    public class TestBuildStep : BuildStep
    {
        public static void PrepareTestBuild(IReporter reporter)
        {
            reporter.Log("Making test build!");

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
                    reporter.Log("PrepareBuild: invoking testBuildMethod");
                    testBuildMethod.Invoke(null, new object[] { });
                }
                else
                {
                    reporter.Log("Method PrepareBuild was not found in type. " +
                              testToolBuildScriptType);
                }
            }
            else
            {
                reporter.Log("Type 'TestToolBuildScript' was not found in assembly. " +
                          allTypes.Aggregate("All types: ",
                              (concatinated, type) => concatinated + "; " + type));
            }
        }
        
        public override void Run(IReporter reporter)
        {
            PrepareTestBuild(reporter);
            throw new System.NotImplementedException("Test Build is not implemented yet.");
        }
    }
}