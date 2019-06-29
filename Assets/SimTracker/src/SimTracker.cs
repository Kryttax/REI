using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SimTracker
{

    /* MAIN CLASS */
    class SimTracker
    {
        public static SimTracker instance = null;

        public static SimTracker Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SimTracker();
                }
                return instance;
            }
        }

        public static Serializer serializaionObjct;
        private static Persistence persistenceObject;
        private Queue<TrackerEvent> assetTrackerObject;

        public string user { get; }     //Player Id
        bool alive, flag;               // bools for controling the thread
        int tick = 3;                   //Thread tick (in seconds)
        Thread QueueCleaner;            

        public SimTracker()
        {
            user = GenerateUserId();
            assetTrackerObject = new Queue<TrackerEvent>();
            serializaionObjct = new Serializer();
            persistenceObject = new Persistence();

            alive = true;
            flag = false;
            QueueCleaner = new Thread(Runnable);
            QueueCleaner.Start();
        }

        //Generates Player's Id
        private string GenerateUserId()
        {
            RandomGenerator generate = new RandomGenerator();
            return generate.RandomId();
        }

        //Tracker Loop
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

                //Checks if there is events yet to be serialized every ((tick)) seconds
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

                    ProcessEvent(obj);                   
                }

                flag = false;

            }

            //Checks if there are any events left to be serialized.
            while (assetTrackerObject.Any())
            {
                Console.WriteLine("New trace generated");
                TrackerEvent obj = assetTrackerObject.Dequeue();

                ProcessEvent(obj);
            }
        }

        public void StopCleaning()
        {
            alive = false;
        }

        //Adds an event to queue
        public void PushEvent(TrackerEvent evnt)
        {
            assetTrackerObject.Enqueue(evnt);
        }

        //Serializes and sends a given event.
        //We can change Format and File/Server sending type.
        void ProcessEvent(TrackerEvent evnt)
        {     
            persistenceObject.SetType(new FilePersistence());
            serializaionObjct.SetType(new CSVSerializer());
            persistenceObject.Send(serializaionObjct.Serialize(evnt));
            serializaionObjct.SetType(new JSONSerializer());
            persistenceObject.Send(serializaionObjct.Serialize(evnt));
        }
    }

    //Random Number and String Generator
    public class RandomGenerator
    {
        // Generates a random number between two numbers    
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // Generates a random string with a given size    
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        // Generates a random Id    
        public string RandomId()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(3, false));
            builder.Append(RandomNumber(1000, 9999));            
            return builder.ToString();
        }
    }


}
