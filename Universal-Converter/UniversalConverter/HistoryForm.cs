using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniversalConverter
{
    public partial class HistoryForm : Form
    {
        Converter.History his;
        Calculator.History hisCalc;
        public HistoryForm()
        {
            InitializeComponent();
        }

        public HistoryForm(Converter.History his)
        {
            InitializeComponent();
            this.his = his;
            for (int i = 0; i < this.his.Count(); i++)
            {
                listBox1.Items.Add(this.his.GetRecord(i).ToString());
            }
        }


        public HistoryForm(Calculator.History his)
        {
            InitializeComponent();
            this.hisCalc = his;
            for (int i = 0; i < this.hisCalc.Count(); i++)
            {
                listBox1.Items.Add(this.hisCalc.GetRecord(i).ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            hisCalc.Clear();
            listBox1.Items.Clear();
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.C)
            {
                string s = listBox1.SelectedItem.ToString();
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
        }

        private void HistoryForm_Load(object sender, EventArgs e)
        {

        }
    }
}
