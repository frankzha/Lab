using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UploadCsv.Models
{
    public class DirectedGraphUtil
    {
        public static IEnumerable<string> DirectedGraphHasCycle(IList<CsvRecord> records)
        {
            DirectedGraph graph = new DirectedGraph();

            foreach (CsvRecord record in records)
            {
                graph.AddEdge(record.Parent, record.Child);
            }

            return graph.CyclicPath();
        }

        public static IEnumerable<string> ShortestPath (IList<CsvRecord> records)
        {
            DirectedGraph graph = new DirectedGraph();

            foreach (CsvRecord record in records)
            {
                graph.AddEdge(record.Parent, record.Child);
            }

            return graph.ShortestPath();
        }
    }
}