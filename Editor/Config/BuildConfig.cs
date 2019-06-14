using System;
using Newtonsoft.Json;
using TCUnityBuild.Config.Steps;

namespace TCUnityBuild.Config
{
    [JsonConverter(typeof(BuildConfigSerializer))]
    public class BuildConfig
    {
        public System.Collections.Generic.List<Step> Steps;
        public BuildParams BuildParams;

        public void Run(IReporter reporter)
        {
            foreach (var step in Steps)
            {
                try
                {
                    step.Run(reporter);
                    reporter.LogStepSuccess();
                }
                catch (Exception e)
                {
                    reporter.LogStepFail(e.Message);
                }
            }
        }
        
        public void ApplyBuildParams(IReporter reporter)
        {
            BuildParams.Apply(reporter);
        }
    }
}