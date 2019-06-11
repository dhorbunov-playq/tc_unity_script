using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using TCUnityBuild;
using UnityEditor.Build.Reporting;

namespace PlayQ.Build
{
	public static class Builder
	{
		/// <summary>
		/// Builds an APK at the specified path with the specified texture compression.
		/// </summary>
		/// <param name="buildPath">Path for the output APK.</param>
		/// <param name="textureCompression">If not null, will override the texture compression subtarget.</param>
		public static void BuildAndroid(string buildPath, MobileTextureSubtarget? textureCompression, IReporter reporter)
		{
			if (textureCompression != null)
			{
				EditorUserBuildSettings.androidBuildSubtarget = textureCompression.Value;
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

			ProcessBuild(buildPath, BuildTarget.Android, reporter);
		}

		/// <summary>
		/// Builds an XCode project at the specified path.
		/// </summary>
		/// <param name="buildPath">Path for the XCode project.</param>
		public static void BuildIos(string buildPath, IReporter reporter)
		{
			ProcessBuild(buildPath, BuildTarget.iOS, reporter);
		}

		public static void BuildWebGL(string buildPath, MobileTextureSubtarget? textureCompression, IReporter reporter)
		{
			ProcessBuild(buildPath, BuildTarget.WebGL, reporter);
		}

		private static void ProcessBuild(string buildPath, BuildTarget buildTarget, IReporter reporter)
		{
			BuildReport buildReport = BuildPipeline.BuildPlayer(GetEnabledScenePaths(reporter).ToArray(), buildPath,
				buildTarget, BuildOptions.None);
			switch (buildReport.summary.result)
			{
				case BuildResult.Succeeded:
//				BuildReporter.Current.IndicateSuccessfulBuild();
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

		/// <summary>
		/// Builds the specified build target.
		/// </summary>
		/// <param name="parsedBuildTarget">Build target to build.</param>
		/// <param name="buildPath">Output path for the build.</param>
		/// <param name="parsedTextureSubtarget">Texture compression subtarget for Android.</param>
		public static void Build(BuildTarget parsedBuildTarget, string buildPath,
			MobileTextureSubtarget? parsedTextureSubtarget, IReporter reporter)
		{
			Directory.CreateDirectory(buildPath);

			switch (parsedBuildTarget)
			{
				case BuildTarget.Android:
					EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
					BuildAndroid(buildPath, parsedTextureSubtarget, reporter);
					break;
				case BuildTarget.iOS:
					EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
					BuildIos(buildPath, reporter);
					break;
				case BuildTarget.WebGL:
					EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WebGL);
					BuildWebGL(buildPath, null, reporter);
					break;
				default:
					throw new ArgumentException(parsedBuildTarget + " is not a supported build target.");
			}
		}

	}
}
