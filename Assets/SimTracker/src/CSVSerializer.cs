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
        string ISerializer.Serialize(IEvent evnt)
        {
            var records = new List<dynamic> { evnt };

            var writer = new System.IO.StringWriter();
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(records);
            }

            return writer.ToString();
        }
    }
}
