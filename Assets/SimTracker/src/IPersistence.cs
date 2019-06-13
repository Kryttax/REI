using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTracker
{
    interface IPersistence
    {
        void Send<T>(T str);
    };
}
