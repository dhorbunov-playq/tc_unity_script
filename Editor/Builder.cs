using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Reflection;
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
		public static void BuildAndroid(string buildPath, MobileTextureSubtarget? textureCompression = null)
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

			ProcessBuild(buildPath, BuildTarget.Android);
		}

		/// <summary>
		/// Builds an XCode project at the specified path.
		/// </summary>
		/// <param name="buildPath">Path for the XCode project.</param>
		public static void BuildIos(string buildPath)
		{
			ProcessBuild(buildPath, BuildTarget.iOS);
		}

		public static void BuildWebGL(string buildPath, MobileTextureSubtarget? textureCompression = null)
		{
			ProcessBuild(buildPath, BuildTarget.WebGL);
		}

		private static void ProcessBuild(string buildPath, BuildTarget buildTarget)
		{
			BuildReport buildReport = BuildPipeline.BuildPlayer(GetEnabledScenePaths().ToArray(), buildPath,
				buildTarget, BuildOptions.None);
			switch (buildReport.summary.result)
			{
				case BuildResult.Succeeded:
//				BuildReporter.Current.IndicateSuccessfulBuild();
					Debug.Log("Build successful!");
					break;
				case BuildResult.Cancelled:
					Debug.LogError("Build was canceled!");
					EditorApplication.Exit(1);
					break;
				case BuildResult.Failed:
					Debug.LogError("Total errors: " + buildReport.summary.totalErrors);
					Debug.LogError(
						"*** Error(s): Unity build player exited with errors. If you dont see any errors in the logs, go into Unity and do a manual export of the project for the current target and look into Unity console for errors.");

					//Quiting this build step with error.
					EditorApplication.Exit(1);
					break;
				default:
					throw new ArgumentOutOfRangeException("Can't proccess build result: " + buildReport.summary.result);
			}
		}


		/// <summary>
		/// Returns a list of all the enabled scenes.
		/// </summary>
		private static List<string> GetEnabledScenePaths()
		{
			List<string> scenePaths = new List<string>();

			Debug.Log("Enabled Scene paths: ");
			foreach (var scene in EditorBuildSettings.scenes)
				if (scene.enabled)
				{
					Debug.Log(scene.path);
					scenePaths.Add(scene.path);
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
			MobileTextureSubtarget? parsedTextureSubtarget = null)
		{
			Directory.CreateDirectory(buildPath);

			switch (parsedBuildTarget)
			{
				case BuildTarget.Android:
					EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
					BuildAndroid(buildPath, parsedTextureSubtarget);
					break;
				case BuildTarget.iOS:
					EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
					BuildIos(buildPath);
					break;
				case BuildTarget.WebGL:
					EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WebGL);
					BuildWebGL(buildPath);
					break;
				default:
					throw new ArgumentException(parsedBuildTarget + " is not a supported build target.");
			}
		}

	}
}
