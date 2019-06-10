using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PlayQ.Build;
using UnityEditor;
using UnityEngine;

namespace PlayQ
{
	public static class TCUnityBuild
	{
		private const string VERSION = "1.0.0";

		private const string PublishPathCommand = "-out";
		private const string BuildNumberCommand = "-buildNumber";

		private const string BuildTargetCommand = "-target";
		private const string AndroidSdkCommand = "-androidSDKPath";
		private const string AndroidNdkCommand = "-androidNDKPath";
		private const string JdkCommand = "-jdkPath";
		private const string KeystoreNameCommand = "-keystoreName";
		private const string KeystorePassCommand = "-keystorePass";
		private const string KeyAliasNameCommand = "-keyaliasName";
		private const string KeyAliasPassCommand = "-keyaliasPass";
		private const string AddDefinesCommand = "-addDefines";
		private const string RemoveDefinesCommand = "-removeDefines";
		private const string BuildModeCommand = "-buildMode";
		private const string BuildVersionCommand = "-buildVersion";
		private const string TestBuildCommand = "-makeTestBuild";

		private const char CommandStartCharacter = '-';

		/// <summary>
		/// Performs the command line build by using the passed command line arguments.
		/// </summary>
		private static void Build()
		{
			Debug.Log("Build started with TC Builder v" + VERSION);

			string publishPath;
			string buildNumber;
			string buildVersion;
			string buildTarget;
			string androidSdk;
			string androidNdk;
			string jdk;
			string keystoreName;
			string keystorePass;
			string keyaliasName;
			string keyaliasPass;
			string definesToAdd;
			string definesToRemove;
			string buildMode;
			string makeTestBuild;

			Dictionary<string, string> commandToValueDictionary = GetCommandLineArguments();

			// Extract our arguments from dictionary
			commandToValueDictionary.TryGetValue(PublishPathCommand, out publishPath);
			commandToValueDictionary.TryGetValue(BuildNumberCommand, out buildNumber);
			commandToValueDictionary.TryGetValue(BuildVersionCommand, out buildVersion);
			commandToValueDictionary.TryGetValue(BuildTargetCommand, out buildTarget);

			commandToValueDictionary.TryGetValue(AndroidSdkCommand, out androidSdk);
			commandToValueDictionary.TryGetValue(AndroidNdkCommand, out androidNdk);
			commandToValueDictionary.TryGetValue(JdkCommand, out jdk);
			commandToValueDictionary.TryGetValue(KeystoreNameCommand, out keystoreName);
			commandToValueDictionary.TryGetValue(KeystorePassCommand, out keystorePass);
			commandToValueDictionary.TryGetValue(KeyAliasNameCommand, out keyaliasName);
			commandToValueDictionary.TryGetValue(KeyAliasPassCommand, out keyaliasPass);
			commandToValueDictionary.TryGetValue(AddDefinesCommand, out definesToAdd);
			commandToValueDictionary.TryGetValue(RemoveDefinesCommand, out definesToRemove);
			commandToValueDictionary.TryGetValue(BuildModeCommand, out buildMode);
			commandToValueDictionary.TryGetValue(TestBuildCommand, out makeTestBuild);

//			var sdkPath = "";
			if (!string.IsNullOrEmpty(androidSdk))
			{
				Debug.Log("Android SDK path: " + androidSdk);
//				EditorSetup.AndroidSdkRoot = androidSdk;
			}
			else
				Debug.Log("Android SDK path is not specified.");

			if (!string.IsNullOrEmpty(androidNdk))
			{
				Debug.Log("Android NDK path: " + androidNdk);
//				EditorSetup.AndroidNdkRoot = androidNdk;
			}
			else
				Debug.Log("Android NDK path is not specified.");

			if (!string.IsNullOrEmpty(jdk))
			{
				Debug.Log("JDK path: " + jdk);
//				EditorSetup.JdkRoot = jdk;
			}
			else
				Debug.Log("JDK path is not specified.");

//			var cmdlineVar = "";
			if (!string.IsNullOrEmpty(keystoreName))
			{
				PlayerSettings.Android.keystoreName = keystoreName;
			}

			if (!string.IsNullOrEmpty(keystorePass))
			{
				PlayerSettings.Android.keystorePass = keystorePass;
			}

			if (!string.IsNullOrEmpty(keyaliasName))
			{
				PlayerSettings.Android.keyaliasName = keyaliasName;
			}

			if (!string.IsNullOrEmpty(keyaliasPass))
			{
				PlayerSettings.Android.keyaliasPass = keyaliasPass;
			}

			if (!string.IsNullOrEmpty(definesToAdd))
			{
				var androidSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
				var iosSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
				var webglSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL);
				var definesList = definesToAdd.Split(',');
				foreach (var define in definesList)
				{
					androidSettings = androidSettings + ";" + define;
					iosSettings = iosSettings + ";" + define;
					webglSettings = webglSettings + ";" + define;

					if (define == "DEVELOP")
					{
						PlayerSettings.productName = "DEV-" + PlayerSettings.productName;
						Debug.Log("DEVELOP Product Name changed to : " + PlayerSettings.productName);
					}

					if (define == "STAGING")
					{
						PlayerSettings.productName = "STG-" + PlayerSettings.productName;
						Debug.Log("Staging Product Name changed to : " + PlayerSettings.productName);
					}
				}

				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, androidSettings);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, iosSettings);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, webglSettings);
			}

			if (!string.IsNullOrEmpty(definesToRemove))
			{
				var androidSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
				var iosSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
				var webglSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL);
				var defines = definesToRemove.Split(',');
				foreach (var d in defines)
				{
					androidSettings = androidSettings.Replace(d, "");
					iosSettings = iosSettings.Replace(d, "");
					webglSettings = webglSettings.Replace(d, "");
				}

				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, androidSettings);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, iosSettings);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, webglSettings);
			}


			if (!string.IsNullOrEmpty(buildMode))
			{
				if (buildMode == "RELEASE")
				{
					EditorUserBuildSettings.allowDebugging = false;
					EditorUserBuildSettings.development = false;
					EditorUserBuildSettings.connectProfiler = false;
					Debug.Log("Building a release version.");
				}
				else
				{
					Debug.LogError("Unknown build mode!");
				}
			}

			Debug.Log("Android scripting symbols: " +
			          PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android));
			Debug.Log("iOS scripting symbols: " +
			          PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS));
			Debug.Log("WebGL scripting symbols: " +
			          PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL));


			if (!string.IsNullOrEmpty(makeTestBuild))
			{
				PrepareTestBuild();
			}

			try

			{
				if (!string.IsNullOrEmpty(buildNumber))
				{
					BundleVersionResolver.BuildNumber = int.Parse(buildNumber);
				}

				if (!string.IsNullOrEmpty(buildVersion))
				{
					BundleVersionResolver.PrettyVersion = buildVersion;
				}

				if (string.IsNullOrEmpty(buildTarget))
				{
					Debug.LogError("No target was specified for this build.");
				}
				else
				{
					BuildTarget parsedBuildTarget = (BuildTarget) Enum.Parse(typeof(BuildTarget), buildTarget);
					Debug.Log("Parsed build target: " + parsedBuildTarget);
					MobileTextureSubtarget? parsedTextureSubtarget = null;
//					if (!string.IsNullOrEmpty(androidTextureCompression))
//						parsedTextureSubtarget = (MobileTextureSubtarget) Enum.Parse(typeof(MobileTextureSubtarget),
//							androidTextureCompression);
//
//					BundleVersionResolver.Setup(parsedBuildTarget);
//					if (string.IsNullOrEmpty(bundleExclusionCommand) || !bool.Parse(bundleExclusionCommand))
//						BuildAndBakeAssetBundles();

					Builder.Build(parsedBuildTarget, publishPath, parsedTextureSubtarget);
					ExecutePostSteps(parsedBuildTarget, publishPath);
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
				throw; //Rethrow the exception so that the build process stops right here.
			}
		}

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

		/// <summary>
		/// Build asset bundles marked as baked and include them in StreamingAssets
		/// </summary>
		private static void BuildAndBakeAssetBundles()
		{
			Debug.Log("Starting Baked Asset Bundle Build.");

			var bunsToBake = AssetDatabase.GetAllAssetBundleNames().Where(bund => bund.StartsWith("baked")).ToArray();
			string outputPath = "Assets/StreamingAssets";
			string manifestPrefix = "baked";

			var builds = new List<AssetBundleBuild>();
			foreach (var bundle in bunsToBake)
			{
				AssetBundleBuild bund = new AssetBundleBuild();
				bund.assetBundleName = bundle;
				bund.assetBundleVariant = string.Empty;
				bund.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(bundle);
				builds.Add(bund);
			}

			var options = BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.StrictMode |
			              BuildAssetBundleOptions.ForceRebuildAssetBundle;
			var targetPlatform = EditorUserBuildSettings.activeBuildTarget;

			if (!Directory.Exists(outputPath))
				Directory.CreateDirectory(outputPath);
			else
			{
				var directory = new DirectoryInfo(outputPath);
				foreach (var file in directory.GetFiles())
				{
					file.Delete();
				}
			}

			BuildPipeline.BuildAssetBundles(outputPath, builds.ToArray(), options, targetPlatform);

			var slashIdx = outputPath.LastIndexOf("/");
			if (slashIdx < 0)
				slashIdx = outputPath.LastIndexOf("\\");
			var dirName = slashIdx < 0 ? outputPath : outputPath.Substring(slashIdx + 1);
			var manifestFiles = Directory.GetFiles(outputPath).Where(fi => Path.GetFileNameWithoutExtension(fi)
				.EndsWith(dirName));
			var platformName = GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
			foreach (var manifestFile in manifestFiles)
			{
				var fileName = Path.GetFileNameWithoutExtension(manifestFile);
				var nameIdx = manifestFile.LastIndexOf(fileName);
				var newName = manifestFile.Substring(0, nameIdx) + manifestPrefix + "_" + platformName +
				              Path.GetExtension(manifestFile);
				File.Move(manifestFile, newName);
			}

			Debug.Log("Baked asset bundles built successfully.");
		}

		private static string GetPlatformForAssetBundles(BuildTarget target)
		{
			switch (target)
			{
				case BuildTarget.Android:
					return "Android";
				case BuildTarget.iOS:
					return "iOS";
				case BuildTarget.WebGL:
					return "WebGL";
				case BuildTarget.StandaloneWindows:
				case BuildTarget.StandaloneWindows64:
					return "StandaloneWindows";
				case BuildTarget.StandaloneOSXIntel:
				case BuildTarget.StandaloneOSXIntel64:
					return "StandaloneOSX";
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Gets all the command line arguments relevant to the build process. All commands that don't have a value after them have their value at string.Empty.
		/// </summary>
		public static Dictionary<string, string> GetCommandLineArguments()
		{
			Dictionary<string, string> commandToValueDictionary = new Dictionary<string, string>();

			string[] args = System.Environment.GetCommandLineArgs();

			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].StartsWith(CommandStartCharacter.ToString()))
				{
					string command = args[i];
					string value = string.Empty;

					if (i < args.Length - 1 && !args[i + 1].StartsWith(CommandStartCharacter.ToString()))
					{
						value = args[i + 1];
						i++;
					}

					if (!commandToValueDictionary.ContainsKey(command))
					{
						commandToValueDictionary.Add(command, value);
					}
					else
					{
						Debug.LogWarning("Duplicate command line argument " + command);
					}
				}
			}

			return commandToValueDictionary;
		}
	}
}