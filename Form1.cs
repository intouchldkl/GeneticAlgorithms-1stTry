using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneticAlgorithms_1stTry
{
    public partial class Form1 : Form
    {
       static TravellingSalesMan TSP ;
        static List<int> distances = new List<int>();
        public Form1()
        {
            InitializeComponent();
         
            

        }
        /*
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            

            
            panel1.BackColor = Color.LightBlue;
            Graphics G = e.Graphics;
            foreach (var c in TSP.AllCities)
            {
                G.FillEllipse(new SolidBrush(Color.Red), c.xcor - 5, c.ycor - 5, 10, 10);
                G.DrawString(c.name, new Font("Arial", 14), new SolidBrush(Color.Black), c.xcor - 10 , c.ycor - 25);
            }
            var BestPath = TSP.paths[0];

            for (int i = 0; i < TSP.cityNum - 1; i ++)
            {
                G.DrawLine(new Pen(Color.LightPink, 3), BestPath.cities[i].xcor, BestPath.cities[i].ycor, BestPath.cities[i + 1].xcor, BestPath.cities[i+1].ycor);
            }
            string bestpathcity = "";
            foreach(var c in BestPath.cities)
            {
                bestpathcity = bestpathcity + c.name;
            }
            bestPlabel.Text = bestpathcity + " => " + BestPath.distance.ToString() + " meter";
        }
        */
        private void evolebut_Click(object sender, EventArgs e)
        {
            string d = "";
            for(int i = 1;i < 101; i++)
            {
                TSP = new TravellingSalesMan(20, i);
                while (TSP.generationNum < 1000)
                {
                    TSP.performEvoulution();
                    d = TSP.paths[0].distance.ToString();
                    //   panel1.Invalidate();
                    // label1.Text = TSP[i].generationNum.ToString();
                }
                richTextBox1.AppendText(d + "\n");
            }

            //File.WriteAllText("result.txt", richTextBox1.Text);

        }
    }
}
