using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTracker
{
    //Context Class
    class Serializer
    {
        private ISerializer _type;
        
        public void SetType(ISerializer type)
        {
            this._type = type;
        }

        public string Serialize(TrackerEvent evnt)
        {
            return _type.Serialize(evnt);
        }
    }
}
