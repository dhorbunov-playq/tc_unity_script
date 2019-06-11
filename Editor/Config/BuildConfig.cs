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

        public void Run()
        {
            foreach (var step in Steps)
            {
                try
                {
                    step.Run();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        public void ApplyBuildParams()
        {
            BuildParams.Apply();
        }
    }
}