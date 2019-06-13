using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SimTracker
{
    class FilePersistence : IPersistence
    {

        string enviromentPath = Environment.CurrentDirectory;
        string unityPath = @"Assets\SimTracker\logs\";
        string solutionPath = "..\\..\\logs\\";
        string path = null;
        string fileName = null;
        StreamWriter sw = null;

        void IPersistence.Send<T>(T str)
        {

            if (path == null)
            {
                if(str is JSONSerializer)
                    fileName = SimTracker.Instance().user + ".json";
                else if(str is CSVSerializer)
                    fileName = SimTracker.Instance().user + ".csv";
                else if (str is string)
                    fileName = SimTracker.Instance().user + ".txt";
                else
                {
                    Console.WriteLine("Object is not serialized correctly.");
                    return;
                }

#if UNITY_EDITOR
                path = Path.Combine(enviromentPath, unityPath, fileName);
#else
                path = Path.Combine(enviromentPath, solutionPath, fileName);
#endif
            }

            //if (!File.Exists(path))
            //{
            //    sw = File.AppendText(path);
            //    sw.WriteLine(str);
            //    sw.Close();
            //}
            //else
            {
                sw = File.AppendText(path);
                sw.WriteLine(str);
                sw.Close();
            }
        }
    }
}
