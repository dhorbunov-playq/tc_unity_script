using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TCUnityBuild.Config;
using TCUnityBuild.Config.Steps;

namespace Tests
{
    public class OtherStepsParsing
    {
        [Test]
        public void BuildAssetBundlesStepParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"BuildAssetBundles\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(AssetBundlesStep));
        }
        
        [Test]
        public void BuildUnityPackageStepParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"BuildUnityPackage\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(UnityPackageStep));
        }
    }
}