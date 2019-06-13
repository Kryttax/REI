using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SimTracker
{
    class SimTracker
    {
        public static SimTracker instance = null;

        public List<ISerializer> serializaionObjct = new List<ISerializer>();

        public List<IPersistence> persistenceObject = new List<IPersistence>();

        Queue<TrackerEvent> assetTrackerObject = new Queue<TrackerEvent>();

        bool alive, flag; // bools for controling the thread
        Thread QueueCleaner;
        public int user { get; }
        int tick = 3;  //Thread tick (in seconds)

        private SimTracker()
        {
            user = GenerateUserId();
            serializaionObjct.Add(new CSVSerializer());
            serializaionObjct.Add(new JSONSerializer());
            persistenceObject.Add(new FilePersistence());
            persistenceObject.Add(new ServerPersistance());

            alive = true;
            flag = false;
            QueueCleaner = new Thread(Runnable);
            QueueCleaner.Start();
        }

        private int GenerateUserId()
        {
            Random random = new Random();
            int num = 0;
            int aux;
            for (int i = 0; i < 6;i++)
            {
                if (i > 0)
                    aux = random.Next(0, 10) * (10 * num);
                else
                    aux = random.Next(0, 10);
                num += aux;
            }
            return num;
        }

        private void Runnable()
        {
            DateTime dt = DateTime.Now;
            DateTime dtnow;
            TimeSpan result;
            int seconds = 0;
            while (alive)
            {
                dtnow = DateTime.Now;

                result = dtnow.Subtract(dt);
                seconds = Convert.ToInt32(result.TotalSeconds);

                //Checks if there is events yet to be serialized every 15 seconds
                if (seconds > tick)
                {
                    flag = !flag;
                    seconds = 0;
                    dt = DateTime.Now;
                }

                while (flag && assetTrackerObject.Any())
                {
                    Console.WriteLine("New trace generated");
                    TrackerEvent obj = assetTrackerObject.Dequeue();

                    persistenceObject[0].Send(obj.ToCSV());
                    persistenceObject[1].Send(obj.ToJson());
                }

                flag = false;

            }

            while (assetTrackerObject.Any())
            {
                Console.WriteLine("New trace generated");
                TrackerEvent obj = assetTrackerObject.Dequeue();

                persistenceObject[0].Send(obj.ToCSV());
                persistenceObject[1].Send(obj.ToJson());
            }
        }

        public void StopCleaning()
        {
            alive = false;
        }

        public static SimTracker Instance()
        {
            if (instance == null)
                instance = new SimTracker();
            return instance;
        }

        public void PushEvent(TrackerEvent evnt)
        {
            assetTrackerObject.Enqueue(evnt);
        }
    }
}
