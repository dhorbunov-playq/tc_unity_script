using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TCUnityBuild.Config;
using TCUnityBuild.Config.Steps;
using UnityEditor;

namespace Tests
{
    public class BuildToolParamsParsing
    {
        [Test]
        public void BuildParamsFullParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    BuildParams:\n";
            json += "    {\n";
            json += "        AndroidSdkPath: \"Android SDK Path\",\n";
            json += "        AndroidNdkPath: \"Android NDK Path\",\n";
            json += "        JdkPath: \"JDK Path\",\n";
            json += "        KeystoreName: \"Keystore Name\",\n";
            json += "        KeystorePass: \"Keystore Pass\",\n";
            json += "        KeyAliasName: \"Key Alias Name\",\n";
            json += "        KeyAliasPass: \"Key Alias Pass\",\n";
            json += "        Defines:\n";
            json += "        {\n";
            json += "            Add: [\"Key1\",\"Key2\",\"Key3\"],\n";
            json += "            Remove: [\"Key1\",\"Key2\",\"Key3\"]\n";
            json += "        }\n";
            json += "    },\n";
            json += "    Steps :\n";
            json += "    [\n";
            /*
            json += "        {\n";
            json += "            Type: \"RunUnitTests\",\n";
            json += "        },\n";
            
            json += "        {\n";
            json += "            Type: \"RunPlayModeTests\",\n";
            json += "        },\n";
            
            json += "        {\n";
            json += "            Type: \"RunPerformanceTests\",\n";
            json += "        },\n";
            
            json += "        {\n";
            json += "            Type: \"RunSmokeTests\",\n";
            json += "        },\n";
            
            
            json += "        {\n";
            json += "            Type: \"CreateAndroidBuild\",\n";
            json += "        },\n";
            
            json += "        {\n";
            json += "            Type: \"CreateAmazoneBuild\",\n";
            json += "        },\n";
            
            json += "        {\n";
            json += "            Type: \"CreateIOsBuild\",\n";
            json += "        },\n";
            
            json += "        {\n";
            json += "            Type: \"CreateWebGLBuild\",\n";
            json += "        },\n";
            
            json += "        {\n";
            json += "            Type: \"CreateTestBuild\",\n";
            json += "        },\n";
            
            
            json += "        {\n";
            json += "            Type: \"BuildAssetBundles\",\n";
            json += "        },\n";
            
            json += "        {\n";
            json += "            Type: \"BuildUnityPackage\",\n";
            json += "        },\n";
            */
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();
            
            Assert.AreEqual(buildConfig.BuildParams.AndroidSdkPath, "Android SDK Path");
            Assert.AreEqual(buildConfig.BuildParams.AndroidNdkPath, "Android NDK Path");
            Assert.AreEqual(buildConfig.BuildParams.JdkPath, "JDK Path");
            
            Assert.AreEqual(buildConfig.BuildParams.KeystoreName, "Keystore Name");
            Assert.AreEqual(buildConfig.BuildParams.KeystorePass, "Keystore Pass");
            Assert.AreEqual(buildConfig.BuildParams.KeyAliasName, "Key Alias Name");
            Assert.AreEqual(buildConfig.BuildParams.KeyAliasPass, "Key Alias Pass");
            
            Assert.AreEqual(buildConfig.BuildParams.Defines.Add.Count, 3);
            Assert.AreEqual(buildConfig.BuildParams.Defines.Add[0], "Key1");
            Assert.AreEqual(buildConfig.BuildParams.Defines.Add[1], "Key2");
            Assert.AreEqual(buildConfig.BuildParams.Defines.Add[2], "Key3");
            
            Assert.AreEqual(buildConfig.BuildParams.Defines.Remove.Count, 3);
            Assert.AreEqual(buildConfig.BuildParams.Defines.Remove[0], "Key1");
            Assert.AreEqual(buildConfig.BuildParams.Defines.Remove[1], "Key2");
            Assert.AreEqual(buildConfig.BuildParams.Defines.Remove[2], "Key3");
        }
        
        [Test]
        public void BuildParamsOptionalFields()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();
            
            Assert.AreEqual(buildConfig.BuildParams.AndroidSdkPath, null);
            Assert.AreEqual(buildConfig.BuildParams.AndroidNdkPath, null);
            Assert.AreEqual(buildConfig.BuildParams.JdkPath, null);
            
            Assert.AreEqual(buildConfig.BuildParams.KeystoreName, null);
            Assert.AreEqual(buildConfig.BuildParams.KeystorePass, null);
            Assert.AreEqual(buildConfig.BuildParams.KeyAliasName, null);
            Assert.AreEqual(buildConfig.BuildParams.KeyAliasPass, null);
            
            Assert.AreEqual(buildConfig.BuildParams.Defines.Add.Count, 0);
            Assert.AreEqual(buildConfig.BuildParams.Defines.Remove.Count, 0);
        }
    }
}
