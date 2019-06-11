using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using TCUnityBuild.Config;
using UnityEditor;
using UnityEngine;

namespace TCUnityBuild
{
	public static class TCUnityBuild
	{
		private const string VERSION = "1.0.0";

		private static class Commands
		{
			public const string BUILD_STEPS = "-unifiedBuildData";
		}

		private const char CommandStartCharacter = '-';

		/// <summary>
		/// Performs the command line build by using the passed command line arguments.
		/// </summary>
		[UsedImplicitly]
		private static void Build()
		{
			TCReporter reporter = new TCReporter();
			reporter.Log("Build started with TC Builder v" + VERSION);

			string buildSteps;

			Dictionary<string, string> commandToValueDictionary = GetCommandLineArguments(reporter);

			if (!commandToValueDictionary.TryGetValue(Commands.BUILD_STEPS, out buildSteps))
			{
				reporter.LogFail("Build method was called, but buildSteps are not found!");
				EditorApplication.Exit(1);
			}

			BuildConfig buildConfig = JObject.Parse(buildSteps).ToObject<BuildConfig>();
			buildConfig.ApplyBuildParams(reporter);

			reporter.Log("Android scripting symbols: " +
			             PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android));
			reporter.Log("iOS scripting symbols: " +
			             PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS));
			reporter.Log("WebGL scripting symbols: " +
			             PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL));

			buildConfig.Run(reporter);
		}


		/// <summary>
		/// Gets all the command line arguments relevant to the build process. All commands that don't have a value after them have their value at string.Empty.
		/// </summary>
		public static Dictionary<string, string> GetCommandLineArguments(IReporter reporter)
		{
			Dictionary<string, string> commandToValueDictionary = new Dictionary<string, string>();

			string[] args = Environment.GetCommandLineArgs();

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
						reporter.LogWarning("Duplicate command line argument " + command);
					}
				}
			}

			return commandToValueDictionary;
		}
	}
}