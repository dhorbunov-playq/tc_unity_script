using System;
using Boo.Lang;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TCUnityBuild.Config.Steps;

namespace TCUnityBuild.Config
{
    [JsonConverter(typeof(BuildConfigSerializer))]
    public class BuildConfig
    {
        public List<Step> Steps;
        public BuildParams BuildParams;

        public void Run(IReporter reporter)
        {
            foreach (var step in Steps)
            {
                try
                {
                    step.Run(reporter);
                }
                catch (Exception e)
                {
                    reporter.LogFail(e.Message);
                }
            }
        }
        
        public void ApplyBuildParams(IReporter reporter)
        {
            BuildParams.Apply(reporter);
        }
    }

    public class BuildConfigSerializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var json = JObject.Load(reader);
            BuildConfig config = new BuildConfig();
            config.BuildParams = new BuildParams();
            JToken buildParamsToken;
            if (json.TryGetValue("BuildParams", out buildParamsToken))
            {
                JObject buildParams = buildParamsToken as JObject;

                TryToWrite(buildParams, "JdkPath", ref config.BuildParams.JdkPath);
                TryToWrite(buildParams, "AndroidSdkPath", ref config.BuildParams.AndroidSdkPath);
                TryToWrite(buildParams, "AndroidNdkPath", ref config.BuildParams.AndroidNdkPath);
                
                TryToWrite(buildParams, "KeystoreName", ref config.BuildParams.KeystoreName);
                TryToWrite(buildParams, "KeystorePass", ref config.BuildParams.KeystorePass);
                TryToWrite(buildParams, "KeyAliasName", ref config.BuildParams.KeyAliasName);
                TryToWrite(buildParams, "KeyAliasPass", ref config.BuildParams.KeyAliasPass);

                config.BuildParams.Defines = new Defines();
                
                JToken definesToken;
                if (json.TryGetValue("Defines", out definesToken))
                {
                    JObject defines = buildParamsToken as JObject;
                    config.BuildParams.Defines = defines.ToObject<Defines>();
                }
            }

            config.Steps = new List<Step>();
            JArray array = json["Steps"] as JArray;
            for (int i = 0; i < array.Count; i++)
            {
                JObject step = array[i] as JObject;
                StepTypes stepType = step["Type"].Value<StepTypes>();
                switch (stepType)
                {
                    case StepTypes.RunUnitTests:
                        config.Steps.Add(new UnitTestsStep());
                        break;
                    case StepTypes.RunPlayModeTests:
                        config.Steps.Add(new PlayModeTestsStep());
                        break;
                    case StepTypes.RunPerformanceTests:
                        config.Steps.Add(new PerformanceTestsStep());
                        break;
                    case StepTypes.RunSmokeTests:
                        config.Steps.Add(new SmokeTestsStep());
                        break;
                    case StepTypes.CreateAndroidBuild:
                        string buildPath = step["BuildPath"].Value<string>();
                        
                        int? buildNumber = null;
                        TryToWrite(step, "BuildNumber", ref buildNumber);
                        
                        string buildVersion = null;
                        TryToWrite(step, "BuildVersion", ref buildVersion);
                        
                        bool release = false;
                        TryToWrite(step, "Release", ref release);
                        
                        string textureCompression = null;
                        TryToWrite(step, "TextureCompression", ref textureCompression);
                        
                        config.Steps.Add(new AndroidBuildStep(buildPath, buildNumber, buildVersion, release, textureCompression));
                        break;
                    case StepTypes.CreateAmazoneBuild:
                        buildPath = step["BuildPath"].Value<string>();
                        
                        buildNumber = null;
                        TryToWrite(step, "BuildNumber", ref buildNumber);
                        
                        buildVersion = null;
                        TryToWrite(step, "BuildVersion", ref buildVersion);
                        
                        release = false;
                        TryToWrite(step, "Release", ref release);
                        
                        config.Steps.Add(new AmazoneBuildStep(buildPath, buildNumber, buildVersion, release));
                        break;
                    case StepTypes.CreateIOsBuild:
                        buildPath = step["BuildPath"].Value<string>();
                        
                        buildNumber = null;
                        TryToWrite(step, "BuildNumber", ref buildNumber);
                        
                        buildVersion = null;
                        TryToWrite(step, "BuildVersion", ref buildVersion);
                        
                        release = false;
                        TryToWrite(step, "Release", ref release);
                        
                        textureCompression = null;
                        TryToWrite(step, "TextureCompression", ref textureCompression);

                        config.Steps.Add(new iOSBuildStep(buildPath, buildNumber, buildVersion, release));
                        break;
                    case StepTypes.CreateTestBuild:
                        buildPath = step["BuildPath"].Value<string>();
                        
                        buildNumber = null;
                        TryToWrite(step, "BuildNumber", ref buildNumber);
                        
                        buildVersion = null;
                        TryToWrite(step, "BuildVersion", ref buildVersion);
                        
                        release = false;
                        TryToWrite(step, "Release", ref release);
                        
                        textureCompression = null;
                        TryToWrite(step, "TextureCompression", ref textureCompression);
                        
                        config.Steps.Add(new TestBuildStep(buildPath, buildNumber, buildVersion, release));
                        break;
                    case StepTypes.BuildAssetBundles:
                        config.Steps.Add(new AssetBundlesStep());
                        break;
                    case StepTypes.BuildUnityPackage:
                        config.Steps.Add(new UnityPackageStep());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Unknown step type! " + stepType);
                }
            }

            return config;
        }

        private enum StepTypes
        {
            RunUnitTests,
            RunPlayModeTests,
            RunPerformanceTests,
            RunSmokeTests,
            
            CreateAndroidBuild,
            CreateAmazoneBuild,
            CreateIOsBuild,
            CreateTestBuild,
            
            BuildAssetBundles,
            BuildUnityPackage
        }
        
        
        private void TryToWrite<T>(JObject jObject, string name, ref T value)
        {
            JToken token;
            if (jObject.TryGetValue(name, out token))
            {
                value = token.Value<T>();
            }
        }

    

    public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}