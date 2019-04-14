using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
