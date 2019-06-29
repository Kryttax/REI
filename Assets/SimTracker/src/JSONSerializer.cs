using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SimTracker
{
    class JSONSerializer : ISerializer
    {
        //Serializes an event to JSON with Newtonsoft's lib. Returns serialized string.
        string ISerializer.Serialize(IEvent evnt)
        {
            return evnt.ToJson();
        }
    }
}
