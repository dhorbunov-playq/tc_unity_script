namespace TCUnityBuild.Config.Steps
{
    public abstract class TestsStep : Step
    {
        /* unity native tests command line attributes:
         -runEditorTests
         * -editorTestsCategories	Filter editor tests by categories. Separate test categories with a comma.
        -editorTestsFilter	Filter editor tests by names. Separate test names with a comma.
        -editorTestsResultFile - Path location to place the result file. If the path is a folder, the command line uses a default file name. If not specified, it places the results in the project’s root folder.
         */
        
        //-batchmode -nographics - testools can't work

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