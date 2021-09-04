using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneticAlgorithms_1stTry
{
    class Path
    {
       public int distance = 0;
       public string stringpath;
        public List<City> cities = new List<City>();
        public Path()
        {
            //loop through all cities
            //use x and y cor to figure out the distance, Add them together to get total distance(Pythagoras)
            
        }

        public int calDistance()
        {
            
            for(int i = 1;i < cities.Count; i++)
            {
                distance = (int)(distance + Math.Sqrt(Math.Pow((cities.ElementAt(i).xcor - cities.ElementAt(i-1).xcor), 2) + Math.Pow((cities.ElementAt(i).ycor - cities.ElementAt(i-1).ycor), 2)));
            }
            return distance;
        }



    }
}
