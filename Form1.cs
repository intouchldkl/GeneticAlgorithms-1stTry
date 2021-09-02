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
       static TravellingSalesMan TSV;
        public Form1()
        {
            InitializeComponent();
            TSV = new TravellingSalesMan(3);        
        }
        
        

    }
}
