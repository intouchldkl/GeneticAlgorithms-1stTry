﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneticAlgorithms_1stTry
{
    class Path
    {
       public double distance = 0;
       public string stringpath;
        public List<City> cities = new List<City>();
        public double fitness;
        public Path()
        {
            //loop through all cities
            //use x and y cor to figure out the distance, Add them together to get total distance(Pythagoras)
            
        }

        public void calDistance()
        {
            
            for(int i = 1;i < cities.Count; i++)
            {
                distance = (distance + Math.Sqrt(Math.Pow((cities[i].xcor - cities[i-1].xcor), 2) + Math.Pow((cities[i].ycor - cities[i-1].ycor), 2)));
            }
        }

        public void calFitness()
        {
            fitness = (1 / distance) *10000;
        }



    }
}
