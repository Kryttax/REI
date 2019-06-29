using System;

namespace SimTracker
{
    /* Specific Events to use in-game. All of them are childs of
     * TrackerEvent, the base Event. Each of them have their own 
     * parameters and constructors to facilitate working with them.*/

    [Serializable]
    public sealed class BugEvent : TrackerEvent
    {
        public string _area { get; private set; }
        public string _errorMsg { get; set; }
        public string _keyWord { get; set; }

        public BugEvent(int level, string area, double xPos, double yPos, double zPos, string error, string keyWord)
            : base(level, xPos, yPos, zPos)
        {
            _area = area;
            _errorMsg = error;
            _keyWord = keyWord;
            _type = eventType.BUG.ToString();
        }

        public override string ToCSV()
        {
            string result = base.ToCSV();

            result += ",\"";
            result += string.Join(",", _area, _errorMsg, _keyWord);
            result += "\"";

            return result;
        }

    }

    [Serializable]
    public sealed class MilestoneEvent : TrackerEvent
    {
        public string _achievement { get; private set; }

        public MilestoneEvent(int level, double xPos, double yPos, double zPos, string achiv)
            : base(level, xPos, yPos, zPos)
        {
            _achievement = achiv;
            _type = eventType.MILESTONE.ToString();
        }

        public override string ToCSV()
        {
            string result = base.ToCSV();

            result += ",\"";
            result += string.Join(",", _achievement);
            result += "\"";
            return result;
        }

    }

    [Serializable]
    public sealed class ProgressEvent : TrackerEvent
    {
        public string _area { get; private set; }

        public ProgressEvent(int level)
            : this(level, string.Empty, -1, -1, -1) { }

        public ProgressEvent(int level, string area, double xPos, double yPos, double zPos)
            : base(level, xPos, yPos, zPos)
        {
            _area = area;
            _type = eventType.PROGRESS.ToString();
        }

        public override string ToCSV()
        {
            string result = base.ToCSV();

            result += ",\"";
            result += string.Join(",", _area);
            result += "\"";

            return result;
        }

    }

    [Serializable]
    public sealed class InteractionEvent : TrackerEvent
    {
        public string _object { get; private set; }
        public string _description { get; private set; }

        public InteractionEvent(int level, double xPos, double yPos, double zPos, string interaction, string description = "no description required")
            : base(level, xPos, yPos, zPos)
        {
            _object = interaction;
            _description = description;
            _type = eventType.INTERACTION.ToString();
        }

        public override string ToCSV()
        {
            string result = base.ToCSV();

            result += ",\"";
            result += string.Join(",", _object, _description);
            result += "\"";

            return result;
        }

    }
}
