using Newtonsoft.Json;

namespace SimTracker
{
    class JSONSerializer : ISerializer
    {
        string ISerializer.Serialize(IEvent evnt)
        {
            return JsonConvert.SerializeObject(evnt);
        }
    }
}
