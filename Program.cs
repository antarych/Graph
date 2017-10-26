using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graph
{
    class Graph
    {
        public Graph(int[] i, int[] j, int[] c)
        {
            I = new List<int>();
            J = new List<int>();
            C = c.ToList();

            H = new List<int>();
            L = new List<int>();

            n = Math.Max(i.Max(), j.Max()) + 1;

            K = Enumerable.Repeat(-1, n).ToList();

            for (int k = 0; k < n; k++) H.Insert(k, -1);

            for (int l = 0; l < i.Length; l++)
            {
                Add(i[l], j[l]);
            }


        }

        public void Add(int i, int j)
        {
            if (I.Count != 0 && J.Count != 0)
            {
                if (i > n - 1 && j > i || j > n - 1 && i > j)
                {
                    H.Add(-1);
                    H.Add(-1);
                    K.Add(-1);
                    K.Add(-1);
                    n += 2;
                }
                else if (i > n - 1 && j > n - 1)
                {
                    H.Add(-1);
                    K.Add(-1);
                    n++;
                }
            }
            I.Add(i);
            J.Add(j);
            L.Add(H[i]);
            H[i] = L.Count - 1;
        }

        public void RemoveArc(int l)
        {
            I.RemoveAt(l);
            J.RemoveAt(l);   
            H[H.IndexOf(l)] = L[L.Count - 1];
            n = Math.Max(I.Max(), J.Max()) + 1;
            L.RemoveAt(L.Count - 1);
        }

        public void RemoveArc(int i, int j)
        {
            for (int k = 0; k < I.Count; k++)
            {
                if (I[k] == i && J[k] == j)
                {
                    RemoveArc(k);
                    break;
                }
            }
        }

        public void Print()
        {
            string mydocpath =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\Graph.txt"))
            {
                outputFile.WriteLine("digraph test {");
                for (int k = 0; k < I.Count; k++)
                {
                    outputFile.WriteLine("{0} -> {1};", I[k], J[k]);
                }
                outputFile.WriteLine('}');
            }
        }

        private void DFS(int vertex, int currComp)
        {
            var Hn = H;
            var S = new List<int>();
            var w = 0;
            var k = -1;
            var j = -1;
            while (true)
            {
                K[vertex] = currComp;
                for (k = Hn[vertex]; k != -1; k = L[k])
                {
                    j = J[I.IndexOf(vertex)];
                    if (K[j] == -1)
                    {
                        break;
                    }
                }
                if (k != -1)
                {
                    Hn[vertex] = L[k];
                    S.Insert(w, vertex);
                    w++;
                    vertex = j;
                }
                else
                {
                    if (w == 0)
                    {
                        break;
                    }
                    w--;
                    vertex = S[w];
                }
            }
        }

        public void ConnectComponent()
        {
            DFS(0, 0);
            var currComp = 1;
            for (int i = 1; i < n; i++)
            {
                if (K[i] == -1)
                {
                    DFS(i, currComp);
                    currComp++;
                }
            }
        }

        public List<int> BFS(int v)
        {
            var rang = new List<int>();
            var P = new List<int>();
            for (int i = 0; i < I.Count; i++)
            {
                rang.Add(-1);
                P.Add(-1);
            }
            var queue = new List<int>();

            queue.Add(v);

            while (queue.Count != 0)
            {
                var from = queue.Last();
                queue.RemoveAt(0);

                for (int i = 0; i < I.Count; i++)
                {
                    if (I[i] == from)
                    {
                        var to = J[i];
                        if (rang[to] == -1)
                        {
                            queue.Add(to);
                            rang[to] = rang[from] + 1;
                            P[to] = from;
                        }
                    }
                }

            }
            return P;
        }

        public void PrintList(List<int> list)
        {
            foreach (var item in list)
            {
                Console.Write("{0} ", item);
            }
            Console.WriteLine();
        }

        public void Dijksta(int v)
        {
            var R = new List<int>();
            var P = new List<int>();
            var M = C.Max() * I.Count;
            var B = new List<List<int>>();
            for(int i = 0; i < M; i++)
            {
                B.Add(new List<int>());
            }
            for(int i = 0; i < n; i++)
            {
                R.Add(int.MaxValue);
                P.Add(-2);
            }
            R[v] = 0;
            P[v] = -1;

            B[0].Add(v);

            for(int i = 0; i < M; i++)
            {
                int b = Get(B, i);

                while (Get(B, i) != -1)
                {
                    for (int k = H[i]; k != -1; k = L[k])
                    {
                        int j = J[k];
                        int rj = R[j];
                        if (R[b] + C[k] < rj)
                        {
                            R[j] = R[b] + C[k];
                            P[j] = k;

                            if (rj != int.MaxValue)
                            {
                                Remove(B, j, rj);
                            }
                            Insert(B, j, R[j]);
                        }

                    }
                }
            }
        }

        private int Get(List<List<int>> B, int k)
        {
            var i = -1;
            if (B[k].Count != 0)
            {
                i = B[k].First();
                B[k].RemoveAt(0);
            }
            return i;
        }

        private void Insert(List<List<int>> B, int k, int i)
        {
            B[k].Insert(0, i);
        }

        private void Remove(List<List<int>> B, int k, int i)
        {
            B[k].Remove(i);
        }

        private List<int> I;
        private List<int> J;
        private List<int> C;
        private List<int> H;
        private List<int> L;
        private List<int> K;
        private int n;
    }


    class Program
    {
        static void Main(string[] args)
        {
            var graph = new Graph(new []{0, 1, 2, 1, 0}, new [] {1, 2, 3, 3, 2}, new[] {1, 3, 1, 4, 5});
            Console.WriteLine();
            graph.Add(4, 5);
            graph.Add(6, 7);
            graph.Print();

            graph.Dijksta(0);
            
            Console.ReadKey();
        }
    }
}
