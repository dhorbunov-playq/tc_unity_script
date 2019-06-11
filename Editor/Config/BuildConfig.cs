using System;
using Boo.Lang;
using TCUnityBuild.Config.Steps;
using UnityEngine;

namespace TCUnityBuild.Config
{
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
}