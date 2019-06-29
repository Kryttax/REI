using System.Threading;

namespace SimTracker
{
    class TestArea
    {
        static void Main(string[] args)
        {
            // Display the number of command line arguments:
            System.Console.WriteLine("Testing. . .\n");

            SimTracker tracker = new SimTracker();
            
            Thread.Sleep(1000);
            //SimTracker.Instance.PushEvent(new TrackerEvent(1, 2, 4, 2));
            SimTracker.Instance.PushEvent(new ProgressEvent(0, "turbine_area", 20, 44, 70));


            Thread.Sleep(1000);
            SimTracker.Instance.PushEvent(new BugEvent(0, "watchroom", 23, 85, 65, "Turbine not powering up correctly", "TURBINE_ERROR"));
            SimTracker.Instance.PushEvent(new MilestoneEvent(1, 30, 44, 9, "FUSE PLACED CORRECTLY"));
            SimTracker.Instance.PushEvent(new MilestoneEvent(1, 30, 44, 9, "DRON LANDED CORRECTLY"));
            SimTracker.Instance.PushEvent(new InteractionEvent(0, 45, 44, 12, "ITEM TAKEN", "object: fuse"));

            SimTracker.Instance.StopCleaning();
        }
    }
}

