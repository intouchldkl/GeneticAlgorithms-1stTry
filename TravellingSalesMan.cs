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
            generateAllpaths();
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
                    path = path + random.ToString(); ;
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
                    p.cities.Add(AllCities.Where(C => C.name == alphabet.Substring(Convert.ToInt32(City), 1)).FirstOrDefault());
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
    }
}
