using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TCUnityBuild.Config;
using TCUnityBuild.Config.Steps;

namespace Tests
{
    public class TestStepsParsing
    {
        
        [Test]
        public void RunUnitTestsStepParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"RunUnitTests\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(UnitTestsStep));
        }   
        
        [Test]
        public void RunPlayModeTestsStepParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"RunPlayModeTests\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(PlayModeTestsStep));
        }
        
        [Test]
        public void RunPerformanceTestsStepParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"RunPerformanceTests\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(PerformanceTestsStep));
        }
        
        
        [Test]
        public void RunSmokeTestsStepParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"RunSmokeTests\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(SmokeTestsStep));
        }
        
    }
}