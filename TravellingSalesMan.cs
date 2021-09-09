using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneticAlgorithms_1stTry
{
    class TravellingSalesMan
    {
        public List<City> AllCities = new List<City>();
        public Random r = new Random(1);
        public string alphabet = "abcdefghijklmnopqrstuvwxyz";
        public List<Path> paths = new List<Path>();
        public int cityNum;
        public int generationNum = 0;
        public TravellingSalesMan(int n)
        {
            this.cityNum = n;
            for (int i = 0; i < n; i++)
            {
                string letter = alphabet.Substring(i,1);
                AllCities.Add(new City(r.Next(500), r.Next(500), letter));
            }
            //Initialise
            generateAllpaths();
            //Evaluation
            sortDistance();
           
        }

        public void generateAllpaths()
        {
            List<string> stringPaths = new List<string>();
            do
            {
                string path = ""; 
                for(int i = 0; i < cityNum; i++)
                {
                    int random = r.Next(cityNum);
                    string letter = alphabet.Substring(random,1).ToString();
                    
                    if (path.Contains(letter))
                    {
                        i--;
                        continue;
                    }
                    path = path + letter; ;
                }
                if (!stringPaths.Contains(path) && path.Distinct().Count() == cityNum)
                {
                    stringPaths.Add(path);
                }
               
            } while (stringPaths.Count < 100);

            stringPaths.Sort();
            BuildPaths(stringPaths);
        }

        public int factorial(int n)
        {
            int fact = 1;
            for (int i = 1; i <= n; i++)
            {
                fact = fact * i;
            }
            return fact;
        }

        public void BuildPaths(List<string> AllpossiblePaths)
        {
           foreach(string path in AllpossiblePaths)
            {
                Path p = new Path();
                for(int i = 0; i < cityNum; i++)
                {
                    string City = path.Substring(i,1);
                    p.cities.Add(AllCities.Where(C => C.name == City).FirstOrDefault());
                    p.stringpath = path;
                }
                 p.calDistance();
                paths.Add(p);
            }
        }

        public void sortDistance()
        {
           /*  var order = paths.OrderBy(p => p.distance);
            int i = 0;
            foreach(var p in order)
            {
                paths[i] = p;
                i++;
            } */
          paths =  paths.OrderBy(p => p.distance).ToList();
        }

        public void selectionTruncation(double Spercent)
        {
            int pathsToKeep = (int)(Spercent / 100 * paths.Count());
            int pathsToKill = paths.Count - pathsToKeep;
            for(int i = pathsToKeep; i < paths.Count(); i++)
            {
                paths[i] = null;
            }
            for(int i = 0; i < pathsToKill; i++)
            {
                paths[i + pathsToKeep] = copypath(paths[i]);
            }
            
        }

        public Path copypath(Path pathToCopy)
        {
            Path newPath = new Path();
            newPath.distance = pathToCopy.distance;
            foreach(City c in pathToCopy.cities)
            {
                newPath.cities.Add(c);
            }
            return newPath;
        }
        public Path crossovercycle(Path parentone, Path parenttwo)
        {
            Path childpath = new Path();         
            for (int i = 0; i < parentone.cities.Count; i++)
            {
                childpath.cities.Add(new City(0,0,"empty"));
            }
            childpath.cities[0] = copyCity(parentone.cities[r.Next(cityNum)]);
            string c2 = parenttwo.cities[0].name;
            int x = 0;
            while (childpath.cities.Exists(c => c.name == "empty"))
            {   
              int v =  parentone.cities.IndexOf(parentone.cities.Where(C => C.name == c2).FirstOrDefault());
                if (childpath.cities.Exists(c => c.name == c2))
                {
                    for (int y = 0; y < childpath.cities.Count; y++)
                    {
                        if (childpath.cities[y].name == "empty")
                        {
                            childpath.cities[y] = copyCity(parenttwo.cities[y]);
                        }
                    }                   
                }
                else if (!childpath.cities.Exists(c => c.name == c2))
                {
                    childpath.cities[v] = copyCity(parenttwo.cities[x]);
                }
                c2 = parenttwo.cities[v].name;
                x = v;
            }
            string p0 = string.Join(",", paths[0].cities.Select(c => c.name));
            string p1 = string.Join(",", paths[1].cities.Select(c => c.name));
            string o1 = string.Join(",", childpath.cities.Select(c => c.name));
            bool validtour = childpath.cities.Distinct().Count() == childpath.cities.Count;
         //   bool haschanged = o1 != p0 && o1 != p1;          
            return childpath;
        }
        public City copyCity(City CityToCopy)
        {
            City newcity = new City(CityToCopy.xcor, CityToCopy.ycor, CityToCopy.name);
            return newcity;
        }
        public bool IsValidTour(Path path)
        {
            return path.cities.Distinct().Count() == path.cities.Count;
        }
        public List<Path> crossover()
        {
            List<Path> childrenpaths = new List<Path>();
            childrenpaths.Add(paths[0]);
            for(int i = 0; i < paths.Count - 1; i++)
            {
                Path childpath = crossovercycle( paths[i], paths[paths.Count - i - 1]);
                int r1 = r.Next(cityNum);
                int r2 = r.Next(cityNum);
                City temp = childpath.cities[r1];
                childpath.cities[r1] = childpath.cities[r2];
                childpath.cities[r2] = temp;
                childpath.calDistance();
                childrenpaths.Add(childpath);

            }

            return childrenpaths;
        }
        public void performEvoulution()
        {
            //Selection
            selectionTruncation(10);
            //crossover
            paths = crossover();
            sortDistance();
            generationNum++;
        }
    }
}
