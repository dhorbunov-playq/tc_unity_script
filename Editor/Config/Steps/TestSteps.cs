namespace TCUnityBuild.Config.Steps
{
    public abstract class TestsStep : Step
    {
        
    }
    
    public class UnitTestsStep : TestsStep 
    {
        public override void Run(IReporter reporter)
        {
            throw new System.NotImplementedException("Unit Tests Running is not implemented yet.");
        }
    }   
    
    public class PlayModeTestsStep : TestsStep 
    {
        public override void Run(IReporter reporter)
        {
            throw new System.NotImplementedException("Play Mode Tests running is not implemented yet.");
        }
    }
    
    public class PerformanceTestsStep : TestsStep 
    {
        public override void Run(IReporter reporter)
        {
            throw new System.NotImplementedException("Performance Tests Running is not implemented yet.");
        }
    }
    
    public class SmokeTestsStep : TestsStep 
    {
        public override void Run(IReporter reporter)
        {
            throw new System.NotImplementedException("Smoke Tests Running build is not implemented yet.");
        }
    }
}