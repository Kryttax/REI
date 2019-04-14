using System.Collections.Generic;
using System.Text;
using CsvHelper;

namespace SimTracker
{
    class CSVSerializer : ISerializer
    {
        StringBuilder csvFile;

        string ISerializer.Serialize(IEvent evnt)
        {
            var records = new List<dynamic> { evnt };

            var writer = new System.IO.StringWriter();
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(records);
            }

            //return csvFile.ToString();
            return writer.ToString();
        }
    }
}
