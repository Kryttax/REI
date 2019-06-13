using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTracker
{
    interface ISerializer
    {
        string Serialize(IEvent _event);
    }
}
