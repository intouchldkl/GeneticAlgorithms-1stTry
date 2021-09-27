using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneticAlgorithms_1stTry
{
    class TravellingSalesMan
    {
        public List<City> AllCities = new List<City>();
        public Random r;
        public string alphabet = "abcdefghijklmnopqrstuvwxyz";
        public List<Path> paths = new List<Path>();
        public int cityNum;
        public int generationNum = 0;
        public TravellingSalesMan(int n,int seed)
        {
            this.r = new Random(seed);
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
                p.calFitness();
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
        public void RouletteSelection(double point)
        {
            List<Path> keptPaths = new List<Path>();
            keptPaths.Add(paths[0]);

            int i = 0;
            while (getfitnesssum(i) < point)
            {
                i++;
                keptPaths.Add(paths[i]);

            }

            int f = 0; //count for keptpaths
            for (int z = 0; z < paths.Count; z++)
            {
                if (f < keptPaths.Count)
                {
                    paths[z] = copypath(keptPaths[f]);
                    f++;
                }
                else
                {
                    f = 0;
                }
            }

            string p0 = "";
            string p1 = "";
            int x;

            for (x = 0; x < paths.Count; x++)
            {
                p1 = p1 + string.Join(",", paths[x].distance + "\n");
            }

            int c = keptPaths.Distinct().Count();
            for (x = 0; x < keptPaths.Count; x++)
            {
                p0 = p0 + string.Join(",", keptPaths[x].distance + "\n");

            }

            x = 0;





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
        
        // This one wrote myself
        public void selectionStochastic()
        {

            double total = 0;
            List<Path> temp = new List<Path>() ;
            temp.Add(copypath(paths[0]));
            paths = paths.OrderByDescending(p => p.distance).ToList();
            /* 
             for (int i = 0; i < paths.Count; i++)
             {
                 temp.Add(new Path());
             }*/
            foreach (var p in paths)
            {
                total += p.fitness;
            }
            double sum = 0;
       //     int j = 0;
            Random rr = new Random(10);
            double r1 = r.NextDouble();
            double ReproductionRate;
            for (int i = 0 ;i < paths.Count; i++)
            {
               ReproductionRate = ((paths[i].fitness / total));
                sum = sum +ReproductionRate;
                while (sum > r1)
                {
                    temp.Add(copypath(paths[i]));
                 //   j++;
                    r1 += 0.01;
                }
            }
            string p0 = "";
            string p1 = "";
            temp = temp.OrderBy(p => p.distance).ToList();
            int c = temp.Count;

            int x = 0;
            for (int i = c; i < paths.Count; i++)
            {
                if (x == temp.Count) x = 0;
                temp.Add(copypath(temp[x]));
                x++;
            }
           
            for (int i = 0; i < paths.Count; i++)
            {
               
                p1 = p1 + string.Join(",", paths[i].distance + "\n");
            }
            for (int i = 0; i < temp.Count; i++)
            {
                p0 = p0 + string.Join(",", temp[i].distance + "\n");

            }
        
            paths = temp;
        }

        //This one refer to the pseudocode from https://en.wikipedia.org/wiki/Stochastic_universal_sampling
        public void selectionStochastic2(double OfftoKeep)
        {
            int pathsToKeep = (int)(OfftoKeep / 100 * paths.Count());
            List<Path> keptPaths = new List<Path>();
            keptPaths.Add(paths[0]);
            List<double> pointers = new List<double>();
            double total = 0;
            int z = 0;
            foreach (var path in paths)
            {
                if(z == pathsToKeep)
                {
                    break;
                }
                total += path.fitness;
                z++;
            }
            Random rr = new Random();
            double p = (total / OfftoKeep);
            int start = rr.Next(0, (int)p);
            for(int i = 0; i < OfftoKeep ; i++)
            {
                pointers.Add(start + (i * p));
            }
            foreach(int point in pointers)
            {
                int i = 0;
                double sum = 0;
                for(int y = 0; y < i; y++)
                {
                    sum += paths[y].fitness;
                }
                while (getfitnesssum(i) < point )
                {
                    i++;
                    keptPaths.Add(paths[i]);
                   
                }
            }
            
            string p0 = "";
            string p1 = "";
            
            for (int i = 0; i < paths.Count; i++)
            {
                p1 = p1 + string.Join(",", paths[i].distance + "\n");
            }
            
            int c = keptPaths.Distinct().Count();
            List<Path> temp = keptPaths.Distinct().ToList();
            for (int i = 0; i < temp.Count; i++)
            {
                p0 = p0 + string.Join(",", temp[i].distance + "\n");

            }
            int x = 0;
            for(int i = c; i < paths.Count; i++)
            {
                paths[i] = temp[x];
                x++;
            }
        }
        public double getfitnesssum(int index)
        {
            double sum = 0;
            for (int y = 0; y < index; y++)
            {
                sum += paths[y].fitness;
            }
            return sum;
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
            int r1 = r.Next(cityNum);
            childpath.cities[r1] = copyCity(parentone.cities[r1]);
            string c2 = parenttwo.cities[r1].name;
            int x = r1;
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
                Path childpath =   crossovercycle( paths[i], paths[paths.Count - i - 1]); //partiallymappedcrossover(paths[i], paths[paths.Count - i - 1]);
                int r1 = r.Next(cityNum);
                int r2 = r.Next(cityNum);
                //mutation
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
            //    while (paths[0].distance > 2300)
            //   {
            //Selection
           //  selectionTruncation(90);
            //   selectionStochastic();
            RouletteSelection(5);
            //crossover

            paths = crossover();
                sortDistance();
                generationNum++;
          //  }
        }
        public Path partiallymappedcrossover(Path p1,Path p2)
        {
            Path childpath = new Path();
           
            for (int i = 0; i < cityNum; i++)
            {
                childpath.cities.Add(new City(0, 0, "empty"));
            }
            int r1 = r.Next(1,cityNum/2)-1 ;
            int r2 = r.Next(cityNum/2, cityNum)-1;
            int legnth = r2 - r1;
            while(legnth <= 2 || legnth > cityNum-(cityNum*0.25))
            {
                r1 = r.Next(0, cityNum / 2);
                r2 = r.Next(cityNum / 2, cityNum);
                legnth = r2 - r1;
            }
            List<int> numbers = new List<int>();
            for(int i = r1; i < r2+1; i++)
            {
                childpath.cities[i] = copyCity(p1.cities[i]);
            }
            int n = 0;
            for (int z = r1; z < r2+1; z++)
            {
                numbers.Add(z);
            }
                for (int i = 0; i < cityNum; i++)
                {
                    City city = copyCity(p2.cities[i]);
                    if (!childpath.cities.Exists(c => c.name == city.name)  && !numbers.Exists(n => n == i))
                    {
                        childpath.cities[i] = city;                    
                    }
                }    
            
            int x = 0;
            for (int y = r1; y < r2+1; y++)
            {
                City ct = copyCity(p2.cities[y]);
                for (int i = x; i < cityNum; i++)
                {
                    if(!childpath.cities.Exists(c => c.name == ct.name) && childpath.cities[i].name == "empty")
                    {
                        childpath.cities[i] = ct;
                        x++;
                       break;
                    }
                }
            }
            string P1 = string.Join(",", p1.cities.Select(c => c.name));
            string P2 = string.Join(",", p2.cities.Select(c => c.name));
            string of1 = string.Join(",", childpath.cities.Select(c => c.name));
            int nu = childpath.cities.Distinct().Count();
            
            return childpath;
        }

    
    }
}
