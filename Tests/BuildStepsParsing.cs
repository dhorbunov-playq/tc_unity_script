using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using TCUnityBuild.Config;
using TCUnityBuild.Config.Steps;
using UnityEditor;

namespace Tests
{
    public class BuildStepsParsing
    {
        private static void CheckBuildStepFullData(BuildConfig buildConfig)
        {
            Assert.AreEqual((buildConfig.Steps[0] as BuildStep).BuildPath, "Build Path");
            Assert.AreEqual((buildConfig.Steps[0] as BuildStep).BuildNumber, 10);
            Assert.AreEqual((buildConfig.Steps[0] as BuildStep).BuildVersion, "1.1.1");
            Assert.AreEqual((buildConfig.Steps[0] as BuildStep).Release, true);
        }

        private static void HandleBuildPathError(string json)
        {
            JObject jObject = JObject.Parse(json);

            try
            {
                BuildConfig buildConfig = jObject.ToObject<BuildConfig>();
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "BuildPath is required!");
            }
        }

        [Test]
        public void ParsingWithoutType()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Data: \"SomeData\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);

            try
            {
                BuildConfig buildConfig = jObject.ToObject<BuildConfig>();
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "Step Type is required!");
            }
        }

        [Test]
        public void ParsingUnknownType()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"SomeType\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);

            try
            {
                BuildConfig buildConfig = jObject.ToObject<BuildConfig>();
                Assert.Fail();
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "Unknown step type! SomeType");
            }
        }

#region NormalParsing

        [Test]
        public void CreateAndroidBuildStepFullParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"CreateAndroidBuild\",\n";
            json += "            BuildPath: \"Build Path\",\n";
            json += "            BuildNumber: 10,\n";
            json += "            BuildVersion: \"1.1.1\",\n";
            json += "            Release: \"True\",\n";
            json += "            TextureCompression: \"DXT\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(AndroidBuildStep));
            CheckBuildStepFullData(buildConfig);
            Assert.AreEqual((buildConfig.Steps[0] as AndroidBuildStep).TextureCompression, MobileTextureSubtarget.DXT);
        }

        [Test]
        public void CreateAmazoneBuildStepFullParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"CreateAmazoneBuild\",\n";
            json += "            BuildPath: \"Build Path\",\n";
            json += "            BuildNumber: 10,\n";
            json += "            BuildVersion: \"1.1.1\",\n";
            json += "            Release: \"True\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(AmazoneBuildStep));
            CheckBuildStepFullData(buildConfig);
        }
        
        [Test]
        public void CreateIOSBuildStepFullParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"CreateIOsBuild\",\n";
            json += "            BuildPath: \"Build Path\",\n";
            json += "            BuildNumber: 10,\n";
            json += "            BuildVersion: \"1.1.1\",\n";
            json += "            Release: \"True\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(iOSBuildStep));
            CheckBuildStepFullData(buildConfig);
        }
        
        [Test]
        public void CreateWebGlBuildStepFullParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"CreateWebGLBuild\",\n";
            json += "            BuildPath: \"Build Path\",\n";
            json += "            BuildNumber: 10,\n";
            json += "            BuildVersion: \"1.1.1\",\n";
            json += "            Release: \"True\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(WebGLBuildStep));
            CheckBuildStepFullData(buildConfig);
        }      
        
        [Test]
        public void CreateTestBuildBuildStepFullParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"CreateTestBuild\",\n";
            json += "            BuildPath: \"Build Path\",\n";
            json += "            BuildNumber: 10,\n";
            json += "            BuildVersion: \"1.1.1\",\n";
            json += "            Release: \"True\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(TestBuildStep));
            CheckBuildStepFullData(buildConfig);
        }
        
#endregion

#region OnlyRequiredParsing

        [Test]
        public void CreateAndroidBuildStepOnlyRequiredParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"CreateAndroidBuild\",\n";
            json += "            BuildPath: \"Build Path\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(AndroidBuildStep));
            Assert.AreEqual((buildConfig.Steps[0] as BuildStep).BuildPath, "Build Path");
        }

        [Test]
        public void CreateAmazoneBuildStepOnlyRequiredParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"CreateAmazoneBuild\",\n";
            json += "            BuildPath: \"Build Path\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(AmazoneBuildStep));
            Assert.AreEqual((buildConfig.Steps[0] as BuildStep).BuildPath, "Build Path");
        }
        
        [Test]
        public void CreateIOSBuildStepOnlyRequiredParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"CreateIOsBuild\",\n";
            json += "            BuildPath: \"Build Path\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(iOSBuildStep));
            Assert.AreEqual((buildConfig.Steps[0] as BuildStep).BuildPath, "Build Path");
        }
        
        [Test]
        public void CreateWebGlBuildStepOnlyRequiredParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"CreateWebGLBuild\",\n";
            json += "            BuildPath: \"Build Path\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(WebGLBuildStep));
            Assert.AreEqual((buildConfig.Steps[0] as BuildStep).BuildPath, "Build Path");
        }      
        
        [Test]
        public void CreateTestBuildBuildStepOnlyRequiredParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"CreateTestBuild\",\n";
            json += "            BuildPath: \"Build Path\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            JObject jObject = JObject.Parse(json);
            BuildConfig buildConfig = jObject.ToObject<BuildConfig>();

            Assert.AreEqual(buildConfig.Steps.Count, 1);
            Assert.AreEqual(buildConfig.Steps[0].GetType(), typeof(TestBuildStep));
            Assert.AreEqual((buildConfig.Steps[0] as BuildStep).BuildPath, "Build Path");
        }

#endregion

#region WithoutRequiredParsing

        [Test]
        public void CreateAndroidBuildStepWithoutRequiredParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"CreateAndroidBuild\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            HandleBuildPathError(json);
        }

        [Test]
        public void CreateAmazoneBuildStepWithoutRequiredParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"CreateAmazoneBuild\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            HandleBuildPathError(json);
        }
        
        [Test]
        public void CreateIOSBuildStepWithoutRequiredParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"CreateIOsBuild\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            HandleBuildPathError(json);
        }
        
        [Test]
        public void CreateWebGlBuildStepWithoutRequiredParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"CreateWebGLBuild\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            HandleBuildPathError(json);
        }      
        
        [Test]
        public void CreateTestBuildBuildStepWithoutRequiredParsing()
        {
            string json = string.Empty;
            json += "{\n";
            json += "    Steps :\n";
            json += "    [\n";
            json += "        {\n";
            json += "            Type: \"CreateTestBuild\",\n";
            json += "        },\n";
            json += "    ]\n";
            json += "}\n";
            
            HandleBuildPathError(json);
        }

#endregion
     
    }
}