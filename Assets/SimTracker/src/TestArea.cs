using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Net;

namespace SimTracker
{
    class TestArea
    {
        static void Main(string[] args)
        {
            // Display the number of command line arguments:
            System.Console.WriteLine("Testing. . .\n");

            SimTracker.Instance();

           // SimTracker.instance.PushEvent(new ProgressEvent());

            Thread.Sleep(1000);
            SimTracker.instance.PushEvent(new ProgressEvent(1));
            
            Thread.Sleep(1000);
            SimTracker.instance.PushEvent(new ProgressEvent(1));
            SimTracker.instance.PushEvent(new ProgressEvent(0, "first room", 20, 50, 70.5));

            SimTracker.instance.StopCleaning();
        }
    }
}


//public interface ISerializer
//{
//    string Serialize(TrackerEvent _event);
//}

//class CSVSerializer : ISerializer
//{
//    StringBuilder csvFile;

//    string ISerializer.Serialize(TrackerEvent evnt)
//    {
//        csvFile = new StringBuilder();

//        string p0, p1, p2;

//        //Depende del tipo de evento (por asignar)
//        p0 = evnt.timeStamp.ToString();
//        p1 = evnt.level.ToString();
//        p2 = evnt.type.ToString();

//        string newLine = string.Format("{0},{1},{2}", p0, p1, p2);
//        csvFile.AppendLine(newLine);


//        return csvFile.ToString();
//    }
//}

//class JsonSerializer : ISerializer
//{
//    string ISerializer.Serialize(TrackerEvent evnt)
//    {
//        return "";
//    }
//}

//public interface IPersistence
//{
//    void Send(string str);
//}

//class FilePersistence : IPersistence
//{
//    string filePath = "..\\..\\logs\\";
//    int index = 0;
//    void IPersistence.Send(string str)
//    {
//        //after your loop
//        File.WriteAllText(filePath + "newFile" + index + ".csv", str);
//        ++index;
//    }
//}

//class ServerPersistence : IPersistence
//{
//    void IPersistence.Send(string str)
//    {
//        //DESCARGAR SERVER GITHUB DE GUILLE (PHYTON)
//        string url = "google.es";
//        Uri myUri = new Uri(url);
//        var ip = Dns.GetHostAddresses(myUri.Host)[0];
//    }
//}

////datetime dt = new datetime(1970, 1, 1, 0, 0, 0, 0, datetimekind.local);

////datetime dtnow = datetime.now;

////timespan result = dtnow.subtract(dt);
////int seconds = convert.toint32(result.totalseconds);


//public class TrackerEvent
//{
//    public enum eventType { BUG, PROGRESS, LEVEL_AREA, COMPLETABLE }

//    public eventType type;
//    public int level { get; set; }  //0 == TUTORIAL, 1 == FIRST LEVEL
//    public string timeStamp { get; set; }
//    public string content { get; set; }


//    public TrackerEvent()
//    {
//        level = 0;
//        timeStamp = DateTime.Now.ToString();
//    }

//    public string ToCSV()
//    {
//        return MyTracker.instance.serializaionObjct[0].Serialize(this);
//    }

//    string ToJson()
//    {
//        return null;
//    }
//}

//public class MyTracker
//{
//    public static MyTracker instance = null;

//    public List<ISerializer> serializaionObjct = new List<ISerializer>();

//    public List<IPersistence> persistenceObject = new List<IPersistence>();

//    Queue<TrackerEvent> assetTrackerObject = new Queue<TrackerEvent>();
//    bool alive, flag;
//    Thread QueueCleaner;

//    private MyTracker()
//    {
//        serializaionObjct.Add(new CSVSerializer());
//        persistenceObject.Add(new FilePersistence());
//        alive = true;
//        flag = false;
//        QueueCleaner = new Thread(runnable);
//        QueueCleaner.Start();
//    }

//    private void runnable()
//    {
//        DateTime dt = DateTime.Now;
//        DateTime dtnow;
//        TimeSpan result;
//        int seconds = 0;
//        while (alive)
//        {
//            dtnow = DateTime.Now;

//            result = dtnow.Subtract(dt);
//            seconds = Convert.ToInt32(result.TotalSeconds);

//            if (seconds > 15)
//            {
//                flag = !flag;
//                seconds = 0;
//                dt = DateTime.Now;
//            }

//            while (flag && assetTrackerObject.Any())
//            {
//                System.Console.WriteLine("Traza creada");
//                TrackerEvent obj = assetTrackerObject.Dequeue();

//                persistenceObject[0].Send(obj.ToCSV());


//            }

//            flag = false;

//        }

//        while (assetTrackerObject.Any())
//        {
//            System.Console.WriteLine("Traza creada");
//            TrackerEvent obj = assetTrackerObject.Dequeue();

//            persistenceObject[0].Send(obj.ToCSV());
//        }
//    }

//    public void StopCleaning()
//    {
//        alive = false;
//    }
//    public static MyTracker Instance()
//    {
//        if (instance == null)
//            instance = new MyTracker();
//        return instance;
//    }

//    public void PushEvent(TrackerEvent evnt)
//    {
//        assetTrackerObject.Enqueue(evnt);
//    }

//    public void WriteInFile()
//    {
//        persistenceObject[0].Send(assetTrackerObject.Dequeue().ToCSV());
//        //persistenceObject[0].Send(serializaionObjct[0].Serialize(assetTrackerObject.Dequeue()));
//    }



//}

////public interface ITrackerAsset
////{
////    bool Accept(TrackerEvent evnt);
////}

////class ProgressionTracker : ITrackerAsset
////{
////    bool ITrackerAsset.Accept(TrackerEvent evnt)
////    {
////        return false;
////    }
////}

////class ResourceTracker : ITrackerAsset
////{
////    bool ITrackerAsset.Accept(TrackerEvent evnt)
////    {
////        return false;
////    }
////}

//public class Pruebas
//{
//    static void Main(string[] args)
//    {
//        // Display the number of command line arguments:
//        System.Console.WriteLine(args.Length);

//        MyTracker.Instance();

//        MyTracker.instance.PushEvent(new TrackerEvent());

//        Thread.Sleep(1000);
//        MyTracker.instance.PushEvent(new TrackerEvent());

//        Thread.Sleep(1000);
//        MyTracker.instance.PushEvent(new TrackerEvent());

//        MyTracker.instance.StopCleaning();
//    }
//}