using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTracker
{
    [Serializable]
    public abstract class TrackerEvent : IEvent
    {
        public enum eventType { NOTYPE, BUG, PROGRESS, LEVEL_AREA, COMPLETABLE }

        public int _userId { get; set; }
        public string _timeStamp { get; set; }
        public string _type { get; set; }
        public int _level { get; set; }  

        public TrackerEvent(int level)
        {
            _userId = SimTracker.Instance().user;
            _level = level;
            _timeStamp = DateTime.Now.ToString();
        }

        public string ToCSV()
        {

            return SimTracker.instance.serializaionObjct.Find(r => r.GetType() == typeof(CSVSerializer)).Serialize(this);
        }

        public string ToJson()
        {
            return SimTracker.instance.serializaionObjct.Find(r => r.GetType() == typeof(JSONSerializer)).Serialize(this);
        }
    }
    
}
