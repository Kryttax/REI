namespace SimTracker
{
    interface IEvent
    {
        string ToCSV();

        string ToJson();
    }
}
