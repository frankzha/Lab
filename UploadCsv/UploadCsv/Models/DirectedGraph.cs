using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UploadCsv.Models
{
    public class DirectedGraph
    {
        Dictionary<string, IList<string>> adjacent;
        Dictionary<string, int> parents;

        Dictionary<string, bool> marked;
        Dictionary<string, string> edgeTo;
        Dictionary<string, int> distTo;

        Dictionary<string, bool> onStack;
        Stack<string> cycle;

        public DirectedGraph()
        {
            this.adjacent = new Dictionary<string, IList<string>>();
            parents = new Dictionary<string, int>();
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

            if (!this.parents.ContainsKey(verticeTo))
            {
                this.parents[verticeTo] = 0;
            }
            this.parents[verticeTo]++;
        }

        void Bfs(string s)
        {
            marked = new Dictionary<string, bool>();
            edgeTo = new Dictionary<string, string>();
            distTo = new Dictionary<string, int>();

            Queue<string> queue = new Queue<string>();

            this.marked[s] = true;
            this.distTo[s] = 0;
            queue.Enqueue(s);

            while (queue.Count > 0)
            {
                string source = queue.Dequeue();
                if (this.adjacent.ContainsKey(source))
                {
                    foreach (string adj in this.adjacent[source])
                    {
                        if (!this.marked.ContainsKey(adj) || this.marked[adj] == false)
                        {
                            this.edgeTo[adj] = source;
                            this.distTo[adj] = distTo[source] + 1;
                            this.marked[adj] = true;
                            queue.Enqueue(adj);
                        }
                    }
                }
            }
        }

        void Bfs(IList<string> sources)
        {
            marked = new Dictionary<string, bool>();
            edgeTo = new Dictionary<string, string>();
            distTo = new Dictionary<string, int>();

            Queue<string> queue = new Queue<string>();

            foreach (string source in sources)
            {
                this.marked[source] = true;
                this.distTo[source] = 0;
                queue.Enqueue(source);
            }

            while (queue.Count > 0)
            {
                string source = queue.Dequeue();
                if (this.adjacent.ContainsKey(source))
                {
                    foreach (string adj in this.adjacent[source])
                    {
                        if (!this.marked.ContainsKey(adj) || this.marked[adj] == false)
                        {
                            this.edgeTo[adj] = source;
                            this.distTo[adj] = distTo[source] + 1;
                            this.marked[adj] = true;
                            queue.Enqueue(adj);
                        }
                    }
                }
            }
        }

        bool HasPathTo(string dest)
        {
            return this.marked.ContainsKey(dest) && this.marked[dest];
        }

        int DistTo(string dest)
        {
            return this.distTo[dest];
        }

        IEnumerable<string> PathTo(string dest)
        {
            if (!this.HasPathTo(dest)) return null;

            Stack<string> stack = new Stack<string>();
            string temp;
            for (temp = dest; this.distTo[temp] != 0; temp = this.edgeTo[temp])
            {
                stack.Push(temp);
            }
            stack.Push(temp);

            return stack;
        }

        public IEnumerable<string> ShortestPath(string s)
        {
            IEnumerable<string> result = null;
            int lengthOfPath = int.MaxValue;

            Bfs(s);

            HashSet<string> all = new HashSet<string>(this.adjacent.Keys);
            foreach (IList<string> adj in this.adjacent.Values)
            {
                foreach (string temp in adj)
                {
                    all.Add(temp);
                }
            }

            IList<string> leaves = all.Where(a => !this.adjacent.ContainsKey(a)).ToList();

            foreach (string dest in leaves)
            {
                if (this.HasPathTo(dest))
                {
                    IEnumerable<string> path = this.PathTo(dest);
                    if (lengthOfPath > path.Count())
                    {
                        lengthOfPath = path.Count();
                        result = path;
                    }
                }
            }

            return result;
        }

        public IEnumerable<string> ShortestPath()
        {
            HashSet<string> all = new HashSet<string>(this.adjacent.Keys);
            foreach (IList<string> adj in this.adjacent.Values)
            {
                foreach (string temp in adj)
                {
                    all.Add(temp);
                }
            }

            IList<string> roots = new List<string>();
            foreach (string source in this.adjacent.Keys)
            {
                if (!this.parents.ContainsKey(source) || this.parents[source] == 0)
                {
                    roots.Add(source);
                }
            }

            IList<string> leaves = all.Where(a => !this.adjacent.ContainsKey(a)).ToList();

            Bfs(roots);

            IEnumerable<string> result = null;
            int lengthOfPath = int.MaxValue;

            foreach (string dest in leaves)
            {
                if (this.HasPathTo(dest))
                {
                    IEnumerable<string> path = this.PathTo(dest);
                    if (lengthOfPath > path.Count())
                    {
                        lengthOfPath = path.Count();
                        result = path;
                    }
                }
            }

            return result;
        }

        void Dfs(string v)
        {
            onStack[v] = true;
            marked[v] = true;

            if (this.adjacent.ContainsKey(v))
            {
                foreach (string w in this.adjacent[v])
                {
                    if (cycle != null)
                    {
                        return;
                    }
                    else if (!marked[w])
                    {
                        edgeTo[w] = v;
                        Dfs(w);
                    }
                    else if (onStack[w])
                    {
                        cycle = new Stack<string>();
                        for (string temp = v; temp != w; temp = edgeTo[temp])
                        {
                            cycle.Push(temp);
                        }
                        cycle.Push(w);
                        cycle.Push(v);
                    }
                }
            }

            onStack[v] = false;
        }

        public IEnumerable<string> CyclicPath()
        {
            HashSet<string> all = new HashSet<string>(this.adjacent.Keys);
            foreach (IList<string> adj in this.adjacent.Values)
            {
                foreach (string temp in adj)
                {
                    all.Add(temp);
                }
            }

            marked = new Dictionary<string, bool>();
            onStack = new Dictionary<string, bool>();
            edgeTo = new Dictionary<string, string>();

            foreach (string vertex in all)
            {
                marked[vertex] = false;
                onStack[vertex] = false;
            }

            foreach (string vertex in all)
            {
                if (!marked[vertex] && cycle == null)
                {
                    Dfs(vertex);
                }
            }

            return cycle;
        }
    }
}