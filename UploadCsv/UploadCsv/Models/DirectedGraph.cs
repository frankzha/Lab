using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UploadCsv.Models
{
    public class DirectedGraph
    {
        Dictionary<string, IList<string>> adjacent;
        
        public DirectedGraph()
        {
            this.adjacent = new Dictionary<string, IList<string>>();
        }

        public void AddEdge(string verticeFrom, string verticeTo)
        {
            if (!this.adjacent.ContainsKey(verticeFrom))
            {
                this.adjacent[verticeFrom] = new List<string>();
            }

            if (!this.adjacent[verticeFrom].Contains(verticeTo))
            {
                this.adjacent[verticeFrom].Add(verticeTo);
            }
        }

        bool IsCyclic(string vertex, Dictionary<string, bool> visited, Dictionary<string, bool> recStack)
        {
            if (visited.ContainsKey(vertex) == false || visited[vertex] == false)
            {
                visited[vertex] = true;
                recStack[vertex] = true;

                if (this.adjacent.ContainsKey(vertex))
                {
                    foreach (string adj in this.adjacent[vertex])
                    {
                        if (!visited.ContainsKey(adj) || !visited[adj])
                        {
                            return IsCyclic(adj, visited, recStack);
                        }
                        else if (recStack.ContainsKey(adj) && recStack[adj])
                        {
                            return true;
                        }
                    }
                }
            }
            recStack[vertex] = false;
            return false;
        }

        public bool IsCyclic()
        {
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            Dictionary<string, bool> recStack = new Dictionary<string, bool>();
            foreach (string vertex in this.adjacent.Keys)
            {
                visited[vertex] = false;
                recStack[vertex] = false;
            }

            foreach (string vertex in this.adjacent.Keys)
            {
                if (!visited.ContainsKey(vertex) || !visited[vertex])
                {
                    if (this.IsCyclic(vertex, visited, recStack))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}