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
    public partial class AppForm : Form
    {
        Converter.Control_ ctl =  new Converter.Control_();

        public AppForm()
        {
            InitializeComponent();
        }

        //инициализация лейблов
        private void AppForm_Load(object sender, EventArgs e)
        {
            label5.Text = ctl.ed.Number;
            //Основание с.сч. исходного числа р1.
            trackBar1.Value = ctl.Pin;
            label7.Text = String.Format("{0}", trackBar1.Value);
            //Основание с.сч. результата р2.
            trackBar2.Value = ctl.Pout;
            label8.Text = String.Format("{0}", trackBar2.Value);

            //Обновить состояние командных кнопок.
            this.UpdateButtons();
        }


        private void DoCmnd(int j)
        {
            if (j == 19) 
            {
                textBox1.Text = ctl.DoCmnd(j);
            }
            else
            {
                if (ctl.St == Converter.Control_.State.Done)
                {
                    //очистить содержимое редактора 
                    label5.Text = ctl.DoCmnd(18);
                }
                //выполнить команду редактирования если не переполнение
                if(ctl.ed.Length() < 16 || j > 16)
                {
                    label5.Text = ctl.DoCmnd(j);
                    //label6.Text = "0";
                    textBox1.Text = "0";
                }
                else
                {
                    MessageBox.Show("Достигнута максимальная длина", "Ошибка", MessageBoxButtons.OK);
                }
                
            }
        }

        //деактивация недоступных для данной с.с кнопок
        private void UpdateButtons()
        {
            //просмотреть все компоненты формы
            foreach (Control i in panel2.Controls)
            {
                if (i is Button)//текущий компонент - командная кнопка 
                {
                    int j = Convert.ToInt16(i.Tag.ToString());
                    if (j < trackBar1.Value)
                    {
                        //сделать кнопку доступной
                        i.Enabled = true;
                    }
                    if ((j >= trackBar1.Value) && (j <= 15))
                    {
                        //сделать кнопку недоступной
                        i.Enabled = false;
                    }
                }
            }
        }


        //реакция на ввод с клавиатуры
        private void AppForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            int i = -1;
            if (e.KeyChar >= 'A' && e.KeyChar <= 'F') i = (int)e.KeyChar - 'A' + 10;
            if (e.KeyChar >= 'a' && e.KeyChar <= 'f') i = (int)e.KeyChar - 'a' + 10;
            if (e.KeyChar >= '0' && e.KeyChar <= '9') i = (int)e.KeyChar - '0';
            if (e.KeyChar == '.' || e.KeyChar == ',') i = 16;
            if ((int)e.KeyChar == 8) i = 17;
            if ((int)e.KeyChar == 13) i = 19;
            if ((i < ctl.Pin) || (i >= 16)) DoCmnd(i);

        }

        ///обработка трекбаров
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            TrackBar tb = (TrackBar)sender;
            label7.Text = String.Format("{0}", tb.Value);
            label5.Text = ctl.DoCmnd(18);
            ctl.Pin = tb.Value;
            this.UpdateButtons();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            TrackBar tb = (TrackBar)sender;
            ctl.Pout = tb.Value;
            label8.Text = String.Format("{0}", tb.Value);
        }
        //////////////////

        ///обработка всех кнопок
        private void button1_Click(object sender, EventArgs e)
        {
            //ссылка на компонент, на котором кликнули мышью
            Button but = (Button)sender;
            //номер выбранной команды
            int j = Convert.ToInt16(but.Tag.ToString());
            trackBar1.Select();
            DoCmnd(j);

        }

        ///запуск других форм
        private void историяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryForm hisForm = new HistoryForm(ctl.his);
            hisForm.Show();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReferenceForm refForm = new ReferenceForm();
            refForm.Show();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.C)
            {
                string s = textBox1.Text;
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
        }

        private void калькуляторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CalculatorForm calcForm = new CalculatorForm();
            calcForm.Show();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
