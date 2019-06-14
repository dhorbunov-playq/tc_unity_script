using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TCUnityBuild.Config.Steps;
using UnityEditor;

namespace TCUnityBuild.Config
{
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
            config.BuildParams.Defines = new Defines();
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


                JToken definesToken;
                if (buildParams.TryGetValue("Defines", out definesToken))
                {
                    JObject defines = definesToken as JObject;
                    config.BuildParams.Defines = defines.ToObject<Defines>();
                }

            }

            if (config.BuildParams.Defines.Add == null)
            {
                config.BuildParams.Defines.Add = new Boo.Lang.List<string>();
            }

            if (config.BuildParams.Defines.Remove == null)
            {
                config.BuildParams.Defines.Remove = new Boo.Lang.List<string>();
            }

            config.Steps = new List<Step>();
            JArray array = json["Steps"] as JArray;
            for (int i = 0; i < array.Count; i++)
            {
                JObject step = array[i] as JObject;
                if (step["Type"] == null || step["Type"].Type == JTokenType.Null)
                {
                    throw new ArgumentException("Step Type is required!");
                }

                StepTypes stepType;
                if (!Enum.TryParse(step["Type"].Value<string>(), out stepType))
                {
                    throw new ArgumentException("Unknown step type! " + step["Type"].Value<string>());
                }

                switch (stepType)
                {
                    case StepTypes.RunEditModeTests:
                        config.Steps.Add(new EditModeTestsStep());
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
                        if (step["BuildPath"] == null || step["BuildPath"].Type == JTokenType.Null)
                        {
                            throw new ArgumentException("BuildPath is required!");
                        }
                        string buildPath = step["BuildPath"].Value<string>();

                        int? buildNumber = null;
                        TryToWrite(step, "BuildNumber", ref buildNumber);

                        string buildVersion = null;
                        TryToWrite(step, "BuildVersion", ref buildVersion);

                        bool release = false;
                        TryToWrite(step, "Release", ref release);

                        string textureCompressionStr = null;
                        TryToWrite(step, "TextureCompression", ref textureCompressionStr);
                        MobileTextureSubtarget? parsedTextureSubtarget = null;
                        if (!string.IsNullOrEmpty(textureCompressionStr))
                        {
                            parsedTextureSubtarget = (MobileTextureSubtarget) Enum.Parse(typeof(MobileTextureSubtarget),
                                textureCompressionStr);
                        }

                        config.Steps.Add(new AndroidBuildStep(buildPath, buildNumber, buildVersion, release,
                            parsedTextureSubtarget));
                        break;
                    case StepTypes.CreateAmazoneBuild:
                        if (step["BuildPath"] == null || step["BuildPath"].Type == JTokenType.Null)
                        {
                            throw new ArgumentException("BuildPath is required!");
                        }
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
                        if (step["BuildPath"] == null || step["BuildPath"].Type == JTokenType.Null)
                        {
                            throw new ArgumentException("BuildPath is required!");
                        }
                        buildPath = step["BuildPath"].Value<string>();

                        buildNumber = null;
                        TryToWrite(step, "BuildNumber", ref buildNumber);

                        buildVersion = null;
                        TryToWrite(step, "BuildVersion", ref buildVersion);

                        release = false;
                        TryToWrite(step, "Release", ref release);

                        config.Steps.Add(new iOSBuildStep(buildPath, buildNumber, buildVersion, release));
                        break;
                    case StepTypes.CreateWebGLBuild:
                        if (step["BuildPath"] == null || step["BuildPath"].Type == JTokenType.Null)
                        {
                            throw new ArgumentException("BuildPath is required!");
                        }
                        buildPath = step["BuildPath"].Value<string>();

                        buildNumber = null;
                        TryToWrite(step, "BuildNumber", ref buildNumber);

                        buildVersion = null;
                        TryToWrite(step, "BuildVersion", ref buildVersion);

                        release = false;
                        TryToWrite(step, "Release", ref release);

                        config.Steps.Add(new WebGLBuildStep(buildPath, buildNumber, buildVersion, release));
                        break;
                    case StepTypes.CreateTestBuild:
                        if (step["BuildPath"] == null || step["BuildPath"].Type == JTokenType.Null)
                        {
                            throw new ArgumentException("BuildPath is required!");
                        }
                        buildPath = step["BuildPath"].Value<string>();

                        buildNumber = null;
                        TryToWrite(step, "BuildNumber", ref buildNumber);

                        buildVersion = null;
                        TryToWrite(step, "BuildVersion", ref buildVersion);

                        release = false;
                        TryToWrite(step, "Release", ref release);

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
            RunEditModeTests,
            RunPlayModeTests,
            RunPerformanceTests,
            RunSmokeTests,
            
            CreateAndroidBuild,
            CreateAmazoneBuild,
            CreateIOsBuild,
            CreateWebGLBuild,
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