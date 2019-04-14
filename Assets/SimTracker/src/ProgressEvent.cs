using System;

namespace SimTracker
{
    public struct PlayerPosition
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public PlayerPosition(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }

    [Serializable]
    public sealed class ProgressEvent : TrackerEvent
    {
        public PlayerPosition playerPos { get; set; }
        public string _room { get; set; }

        public ProgressEvent(int level)
            : this(level, string.Empty, -1, -1, -1) { }

        public ProgressEvent(int level, string room, double xPos, double yPos, double zPos)
            : base(level)
        {
            _room = room;
            _type = eventType.PROGRESS.ToString();
            playerPos = new PlayerPosition(xPos, yPos, zPos);
        }

    }
}
