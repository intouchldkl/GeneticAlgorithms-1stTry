using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneticAlgorithms_1stTry
{
    public partial class Form1 : Form
    {
       static TravellingSalesMan TSP;
        public Form1()
        {
            InitializeComponent();
            TSP = new TravellingSalesMan(3);

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.BackColor = Color.LightBlue;
            Graphics G = e.Graphics;
            foreach (var c in TSP.AllCities)
            {
                G.FillEllipse(new SolidBrush(Color.Red), c.xcor - 5, c.ycor - 5, 10, 10);
                G.DrawString(c.name, new Font("Arial", 14), new SolidBrush(Color.Black), c.xcor - 3, c.ycor - 3);
            }
        }
    }
}
