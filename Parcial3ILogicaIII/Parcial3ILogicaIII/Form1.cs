using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parcial3ILogicaIII
{
    public partial class Form1 : Form
    {
        static public int nodes;
        static public bool dirigido=false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nodes = (int)numericUpDown1.Value;
            if (checkBox1.Checked) dirigido = true;
            Form2 fm2 = new Form2(nodes,dirigido);
            fm2.Show();
            //Console.WriteLine(nodes);
        }

    }
}
