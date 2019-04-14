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
        public enum eventType { NULL, BUG, PROGRESS, LEVEL_AREA, COMPLETABLE }

        public int _user { get; set; }
        public string _timeStamp { get; set; }
        public string _type { get; set; }
        public int _level { get; set; }  //0 == TUTORIAL, 1 == FIRST LEVEL
        //public string content { get; set; }


        public TrackerEvent(int level)
        {
            _user = SimTracker.Instance().user;
            //_type = type;
            _level = level;
            _timeStamp = DateTime.Now.ToString();
        }

        public string ToCSV()
        {
            return SimTracker.instance.serializaionObjct[0].Serialize(this);
        }

        public string ToJson()
        {
            return SimTracker.instance.serializaionObjct[1].Serialize(this);
        }
    }

    /*
    [Serializable]
    class TrackerEvent
    {
        public enum eventType { BUG, PROGRESS, LEVEL_AREA, COMPLETABLE }

        public int user;
        public eventType type;
        public int level { get; set; }  //0 == TUTORIAL, 1 == FIRST LEVEL
        public string timeStamp { get; set; }
        public string content { get; set; }


        public TrackerEvent()
        {
            user = SimTracker.instance.user;
            level = 0;
            timeStamp = DateTime.Now.ToString();
        }

        public string ToCSV()
        {
            return SimTracker.instance.serializaionObjct[0].Serialize(this);
        }

        public string ToJson()
        {
            return SimTracker.instance.serializaionObjct[1].Serialize(this);
        }
    }*/
}
