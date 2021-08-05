using ContosoGamingAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoGamingAPI
{
    public class RouteEngine
    {
        List<LandMark> LandMarks = new List<LandMark>();
        private int vertices;
        private List<LandMark>[] routeList;
        public int MaxStop = 0;

        public RouteEngine(int _vertices)
        {
            this.vertices = _vertices;
            init();
        }

        public RouteEngine(List<LandMark> _landMarks)
        {
            LandMarks = _landMarks;
            this.vertices = _landMarks.Count();
            init();
        }

        private void init()
        {
            routeList = new List<LandMark>[vertices];

            for (int i = 0; i < vertices; i++)
            {
                routeList[i] = new List<LandMark>();
            }
        }

        public void AddRoutePath(LandMark u, LandMark v)
        {
            routeList[u.Index].Add(v);
        }

        public int GetAllRoutePaths(int s, int d)
        {
            LandMark[] visitedLandmarks = LandMarks.ToArray();
            List<int> pathList = new List<int>();

            pathList.Add(s);
            int routeCount = 0;
            CalculateRoutes(s, d, visitedLandmarks, pathList, ref routeCount);

            return routeCount;
        }

        private void CalculateRoutes(int u, int d, LandMark[] visitedLandmarks, List<int> localPathList, ref int routeCount)
        {
            if (u.Equals(d))
            {
                if (localPathList.Count() - 2 <= MaxStop)
                    routeCount++;
                List<LandMark> paths = LandMarks.FindAll(l => localPathList.Contains(l.Index));

                foreach (var p in localPathList)
                    Console.Write(LandMarks.Find(x => x.Index == p).Name);
                Console.WriteLine();
                Console.WriteLine(string.Join(" ", localPathList));
                return;
            }

            visitedLandmarks[u].Visited = true;

            foreach (LandMark i in routeList[u])
            {
                if (!visitedLandmarks[i.Index].Visited)
                {
                    localPathList.Add(i.Index);
                    CalculateRoutes(i.Index, d, visitedLandmarks, localPathList, ref routeCount);

                    localPathList.Remove(i.Index);
                }
            }

            visitedLandmarks[u].Visited = false;
        }

        public void CallMain()
        {
            RouteEngine g = new RouteEngine(5);

            LandMark A = new LandMark(0, "A");
            LandMark B = new LandMark(1, "B");
            LandMark C = new LandMark(2, "C");
            LandMark D = new LandMark(3, "D");
            LandMark E = new LandMark(4, "E");

            LandMarks.Add(A);
            LandMarks.Add(B);
            LandMarks.Add(C);
            LandMarks.Add(D);
            LandMarks.Add(E);


            //g.addEdge(0, 1);
            //g.addEdge(1, 2);
            //g.addEdge(2, 3);
            //g.addEdge(3, 4);
            //g.addEdge(0, 3);
            //g.addEdge(3, 0);
            //g.addEdge(2, 4);
            //g.addEdge(0, 4);
            //g.addEdge(4, 1);

            g.AddRoutePath(A, B);
            g.AddRoutePath(B, C);
            g.AddRoutePath(C, D);
            g.AddRoutePath(D, E);
            g.AddRoutePath(A, D);
            g.AddRoutePath(D, A);
            g.AddRoutePath(C, E);
            g.AddRoutePath(A, E);
            g.AddRoutePath(E, B);
            // arbitrary source
            int s = 0;

            // arbitrary destination
            int d = 2;

            Console.WriteLine("Following are all different"
                            + " paths from " + s + " to " + d);
            g.GetAllRoutePaths(s, d);
        }
    }
}
