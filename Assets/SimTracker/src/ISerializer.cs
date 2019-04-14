namespace SimTracker
{
    interface ISerializer
    {
        string Serialize(IEvent _event);
    }
}
