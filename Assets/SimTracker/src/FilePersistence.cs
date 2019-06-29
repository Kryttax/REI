using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SimTracker
{
    //Creates a new file with player's Id name and writes a serialized event
    class FilePersistence : IPersistence
    {

        private static string solutionPath = "..\\..\\logs\\";
        private string enviromentPath = Environment.CurrentDirectory;
        private string path = null;
        private string fileName = null;
        private StreamWriter sw = null;

#if UNITY_EDITOR
        private static string unityPath = @"Assets\SimTracker\logs\";
#endif

        void IPersistence.Send(string str)
        {

            if (str[0] == '{')
            {
                fileName = SimTracker.Instance.user + ".json";

            }
            else
            {
                fileName = SimTracker.Instance.user + ".csv";
            }

            //Checks Unity Editor's specific path
#if UNITY_EDITOR
                path = Path.Combine(enviromentPath, unityPath, fileName);
#else
            path = Path.Combine(enviromentPath, solutionPath, fileName);
#endif

            sw = File.AppendText(path);
            sw.WriteLine(str);
            sw.Close();

        }
    }
}
