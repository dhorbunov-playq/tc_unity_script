using UnityEditor;
using UnityEngine;

namespace TCUnityBuild
{
    public interface IReporter
    {
        void Log(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogStepSuccess();
        void LogStepFail(string message);
        void LogFatalFail(string message);

    }
    
    public class UnityReporter : IReporter
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }
        
        public void LogStepSuccess()
        {
            EditorUtility.DisplayDialog("Step Success", "Step successfully completed!", "Ok");
        }

        public void LogStepFail(string message)
        {
            EditorUtility.DisplayDialog("Step Failed", message, "Ok");
        }

        public void LogFatalFail(string message)
        {
            EditorUtility.DisplayDialog("Fatal Fail! ", message, "Ok");
        }
    }
    
    public class TCReporter : IReporter
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogWarning(string message)
        {
            Debug.Log("##teamcity[message text='" + message + "' status='WARNING']");
        }
        public void LogError(string message)
        {
            Debug.Log("##teamcity[message text='" + message + "' status='ERROR']");
        }

        public void LogStepSuccess()
        {
            Debug.Log("##teamcity[message text='Step successfully completed!' status='INFO']");
            Debug.Log("[TC Unity Script - Step Completed]");
        }

        public void LogStepFail(string message)
        {
            Debug.Log("##teamcity[message text='Step failed! " + message + "' status='ERROR']");
            Debug.Log("[TC Unity Script - Step Failed]");
        }

        public void LogFatalFail(string message)
        {
            Debug.Log("##teamcity[message text='Fatal fail! " + message + "' status='ERROR']");
            Debug.Log("[TC Unity Script - Fatal Error]");
        }
    }
}