using UnityEditor;
using UnityEngine;

namespace TCUnityBuild
{
    public interface IReporter
    {
        void Log(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogSuccess(string message);
        void LogFail(string message);

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
        
        public void LogSuccess(string message)
        {
            EditorUtility.DisplayDialog("Step Success", message, "Ok");
        }

        public void LogFail(string message)
        {
            EditorUtility.DisplayDialog("Log Failed", message, "Ok");
        }
    }
    
    public class TCReporter : IReporter
    {
        public void Log(string message)
        {
            Debug.Log(message);
//            if (message.Contains("DisplayProgressbar: ")) {
//                var status = message.Substring(20);
//                Debug.Log("##teamcity[progressMessage '" + status + "']");
//            }
        }

        public void LogWarning(string message)
        {
            Debug.Log("##teamcity[message text='" + message + "'" + "status='WARNING']");
        }
        public void LogError(string message)
        {
            Debug.Log("##teamcity[message text='" + message + "'" + "status='ERROR']");
        }

        public void LogSuccess(string message)
        {
            // Magic string to indicate successful build.
            Debug.Log("Successful build ~0xDEADBEEF");        
        }

        public void LogFail(string message)
        {
            Debug.Log("##teamcity[message text='Step failed! " + message + "'" + "status='ERROR']");
        }
    }
}