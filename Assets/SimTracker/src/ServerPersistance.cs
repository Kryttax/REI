using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
namespace SimTracker
{
    class ServerPersistance : IPersistence
    {

        private string webAddr = "http://localhost:8080/tracker";
        private HttpWebRequest httpWebRequest;
        private string requestType = "text/plain";                          //JSON/plainText
        private string requestMethod = "POST";                              //Two methods: GET and POST

        //Sends a serialized event to a given web address
        void IPersistence.Send(string str)
        {
            try
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = requestType;
                httpWebRequest.Method = requestMethod;


                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    streamWriter.Write(str);
                    
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    Console.WriteLine(streamReader.ReadToEnd());
                }


            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
