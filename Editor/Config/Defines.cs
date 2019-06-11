using Boo.Lang;
using UnityEditor;

namespace TCUnityBuild.Config
{
    public class Defines
    {
        public List<string> Add; //addDefines
        public List<string> Remove; //removeDefines

        public void Apply(IReporter reporter)
        {
			if (Add != null && Add.Count > 0)
			{
				var androidSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
				var iosSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
				var webglSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL);
				foreach (var define in Add)
				{
					androidSettings = androidSettings + ";" + define;
					iosSettings = iosSettings + ";" + define;
					webglSettings = webglSettings + ";" + define;

					if (define == "DEVELOP")
					{
						PlayerSettings.productName = "DEV-" + PlayerSettings.productName;
						reporter.Log("DEVELOP Product Name changed to : " + PlayerSettings.productName);
					}

					if (define == "STAGING")
					{
						PlayerSettings.productName = "STG-" + PlayerSettings.productName;
						reporter.Log("Staging Product Name changed to : " + PlayerSettings.productName);
					}
				}

				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, androidSettings);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, iosSettings);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, webglSettings);
			}

			if (Remove != null && Remove.Count > 0)
			{
				var androidSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
				var iosSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
				var webglSettings = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL);
				foreach (var d in Remove)
				{
					androidSettings = androidSettings.Replace(d, "");
					iosSettings = iosSettings.Replace(d, "");
					webglSettings = webglSettings.Replace(d, "");
				}

				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, androidSettings);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, iosSettings);
				PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, webglSettings);
			}
        }
    }
}