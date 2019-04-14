using System;

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


        public TrackerEvent(int level)
        {
            _user = SimTracker.Instance().user;
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
}
