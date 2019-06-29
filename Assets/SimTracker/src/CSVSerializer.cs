namespace SimTracker
{

    //Serializes an event to CSV with CSVHelper's lib. Returns a serialized string.
    class CSVSerializer : ISerializer
    {

        string ISerializer.Serialize(IEvent evnt)
        {
            return evnt.ToCSV();
        }


    }
}
