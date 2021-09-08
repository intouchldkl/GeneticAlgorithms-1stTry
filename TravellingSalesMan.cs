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
            //Selection
            selectionTruncation(10);
            //crossover
    
        Path newpaht =    crossovercycle(paths[0],paths[1]);
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
                childpath.cities.Add(null);
            }

            childpath.cities[0] = parentone.cities[0];
            for (int i = 0; i < parentone.cities.Count; i++)
            {
                string c2 = parenttwo.cities[i].name;

                for (int x = 0; x < parentone.cities.Count; x++)
                {
                    string c1 = parentone.cities[x].name;

                    if (c2 == c1 && !childpath.cities.Contains(parenttwo.cities[i]))
                    {
                        childpath.cities[x] = parenttwo.cities[i];
                    }
                    else if(c1 == c2 && childpath.cities.Contains(parenttwo.cities[i]))
                    {
                        for(int y = 0; y < childpath.cities.Count; y++)
                        {
                            if(childpath.cities[y] == null)
                            {
                                childpath.cities[y] =  parenttwo.cities[y];
                            }
                        }
                    }
                }
                

            }
            return childpath;
        }
    }
}
