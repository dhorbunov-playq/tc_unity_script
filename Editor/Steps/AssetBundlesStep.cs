using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Repository.TCUnityBuild.Editor;
using UnityEditor;

namespace TCUnityBuild.Config.Steps
{
    public class AssetBundlesStep : Step
    {
        		/// <summary>
		/// Build asset bundles marked as baked and include them in StreamingAssets
		/// </summary>
		private static void BuildAndBakeAssetBundles(IReporter reporter)
		{
			reporter.Log("Starting Baked Asset Bundle Build.");

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

			reporter.Log("Baked asset bundles built successfully.");
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

        public override void Run(IReporter reporter)
        {
            throw new NotImplementedException("AssetBundles build is not implemented yet.");
        }
    }
}