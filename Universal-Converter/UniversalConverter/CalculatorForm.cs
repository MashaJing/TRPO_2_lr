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

    public partial class CalculatorForm : Form
    {
        //объект, через который можно обращаться к процессору, памяти и редактору
        Calculator.TCtrl ctl = new Calculator.TCtrl();

        public CalculatorForm()
        {
            InitializeComponent();
        }


        //инициализация лейблов
        private void CalculatorForm_Load(object sender, EventArgs e)
        {

            ToolTip t = new ToolTip();
            t.SetToolTip(EnterLabel, "Введите выражение");
            
            ToolTip t1 = new ToolTip();
            t1.SetToolTip(NumSystemTrackBar, "Изменить систему счисления");
            
            ToolTip t2 = new ToolTip();
            t2.SetToolTip(PrecisionTrackBar, "Изменить точность вычислений");

            ToolTip tMS = new ToolTip();
            tMS.SetToolTip(button18, "Сохранить в память");

            ToolTip tMR = new ToolTip();
            tMR.SetToolTip(MRButton, "Прочитать из памяти");

            ToolTip tMPlus = new ToolTip();
            tMPlus.SetToolTip(button24, "Прибавить к числу в памяти");

            ToolTip tMC = new ToolTip();
            tMC.SetToolTip(MCbutton, "Очистить память");
            
            ToolTip tC = new ToolTip();
            tC.SetToolTip(rButton, "Стереть всё");

            ToolTip tBS = new ToolTip();
            tBS.SetToolTip(BackSpaceButton, "Удалить один символ");

            ToolTip tCE = new ToolTip();
            tCE.SetToolTip(CEButton, "Очистить операнд");

            EnterLabel.Text = ctl.Proc.Lop_Res.a_str;
            //Основание с.сч. исходного числа р1.
            NumSystemTrackBar.Value = ctl.Proc.Lop_Res.b;
            NumSystemLabel.Text = String.Format("{0}", NumSystemTrackBar.Value);
            //Основание с.сч. результата р2.
            PrecisionTrackBar.Value = ctl.Proc.Lop_Res.c;
            PrecisionLabel.Text = String.Format("{0}", PrecisionTrackBar.Value);
            UpdateButtons();
        }

        
        //деактивация недоступных для данной с.с кнопок
        private void UpdateButtons()
        {
            EnterLabel.Text = ctl.Editor.number;
            //просмотреть все компоненты формы
            foreach (Control i in panel2.Controls)
            {
                if (i is Button)//текущий компонент - командная кнопка 
                {
                    int j = Convert.ToInt16(i.Tag.ToString());
                    if (j < NumSystemTrackBar.Value)
                    {
                        //сделать кнопку доступной
                        i.Enabled = true;
                    }
                    if ((j >= NumSystemTrackBar.Value) && (j <= 15))
                    {
                        //сделать кнопку недоступной
                        i.Enabled = false;
                    }
                }
            }
        }


        //реакция на ввод с клавиатуры
        private void CalcFormKeyPress(object sender, KeyPressEventArgs e)
        {
            int i = -1;
            if (e.KeyChar >= 'A' && e.KeyChar <= 'F') i = (int)e.KeyChar - 'A' + 10;
            if (e.KeyChar >= 'a' && e.KeyChar <= 'f') i = (int)e.KeyChar - 'a' + 10;
            if (e.KeyChar >= '0' && e.KeyChar <= '9') i = (int)e.KeyChar - '0';
            if (e.KeyChar == '.' || e.KeyChar == ',') i = 16;
            if ((int)e.KeyChar == 8) i = 20;
            if ((int)e.KeyChar == 13) i = 27;
            if ((i < ctl.Proc.Lop_Res.b) || (i >= 16)) DoCmnd(i);

        }

        private void DoCmnd(int j)
       {
            try
            {
                if (j < 0) return;

                //обработка команд редактора
                if (j <= 20)
                {
                    if (j < 16 && EnterLabel.Text.Length > 16 && !(ctl.CtrlState == Calculator.TCtrl.TCtrlState.cOpChange))
                    {
                        MessageBox.Show("Достигнута максимальная длина", "Ошибка", MessageBoxButtons.OK);
                    }

                    else
                    {
                        EnterLabel.Text = ctl.RunEditorCommand(j);

                        //если не было установлено никакой операции
                        if (ctl.CtrlState == Calculator.TCtrl.TCtrlState.cEditing || ctl.CtrlState == Calculator.TCtrl.TCtrlState.cStart || ctl.CtrlState == Calculator.TCtrl.TCtrlState.cExpDone)
                        {
                            ctl.Proc.Lop_Res.SetValue(ctl.Editor.number);
                            ctl.CtrlState = Calculator.TCtrl.TCtrlState.cEditing;
                        }

                        //вводится правый операнд
                        else if (ctl.CtrlState == Calculator.TCtrl.TCtrlState.cOpChange || ctl.CtrlState == Calculator.TCtrl.TCtrlState.cValDone)
                        {
                            ctl.Proc.Rop.SetValue(ctl.Editor.number);
                            ctl.CtrlState = Calculator.TCtrl.TCtrlState.cValDone;
                        }
                    }
                    return;
                }

                //обработка команд процессора: если редактирование происходит сейчас || хотим вычислить ф-ию для правого операнда
                else if ((j < 27 && (ctl.CtrlState == Calculator.TCtrl.TCtrlState.cEditing)) || (j>24 && j<27 && (ctl.CtrlState == Calculator.TCtrl.TCtrlState.cValDone)))
                {
                    //устанавливаем соответствующую операцию или функцию в процессоре
                    ctl.SetOprtnFunc(j);
                    //добавляем в строку знак операции
                    EnterLabel.Text += ctl.Editor.AddOprtnSign(j);
                    //очищаем строку числа в редакторе
                    ctl.Editor.Clear();
                    return;
                }

                else if (j == 27)
                {
                    //функция Calculate() обновляет значения операндов и возвращает 
                    //строку результата вычисления установленой функции/операции
                    if (ctl.CtrlState == Calculator.TCtrl.TCtrlState.cOpChange)
                        ctl.Proc.Rop = ctl.Proc.Lop_Res.Copy();
                    // && (ctl.CtrlState == Calculator.TCtrl.TCtrlState.cValDone || ctl.CtrlState == Calculator.TCtrl.TCtrlState.FunDone || ctl.CtrlState == Calculator.TCtrl.TCtrlState.cOpChange)
                    ctl.Editor.number = ctl.Calculate();
                    EnterLabel.Text = ctl.Editor.number;
                    //устанавливаем контроллеру исходный статус
                    ctl.CtrlState = Calculator.TCtrl.TCtrlState.cEditing;
                    return;
                }
                //обработка команд памяти
                else if (j > 27)
                {
                    ctl.Editor.number = ctl.RunMemoryCommand(j);
                    EnterLabel.Text = ctl.Editor.number;
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK);
            }
        }

        ///обработка трекбаров
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            TrackBar tb = (TrackBar)sender;
            NumSystemLabel.Text = String.Format("{0}", tb.Value);

            ctl.Proc.Lop_Res.SetBasement(tb.Value);
            ctl.Proc.Rop.SetBasement(tb.Value);
            ctl.Memory.FNumber.SetBasement(tb.Value);

            ctl.Editor.number = ctl.Proc.Lop_Res.a_str;
            EnterLabel.Text = ctl.Editor.number;
            //ctl.CtrlState = Calculator.TCtrl.TCtrlState.cStart;
            UpdateButtons();
            
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            TrackBar tb = (TrackBar)sender;
            PrecisionLabel.Text = String.Format("{0}", tb.Value);

            ctl.Proc.Lop_Res.SetPrecision(tb.Value);
            ctl.Proc.Rop.SetPrecision(tb.Value);
        }

            //////////////////

            ///обработка всех кнопок
        private void button1_Click(object sender, EventArgs e)
        {

            //ссылка на компонент, на котором кликнули мышью
            Button but = (Button)sender;
            //номер выбранной команды
            int j = Convert.ToInt16(but.Tag.ToString());
            DoCmnd(j);

        }

        ///запуск других форм
        
        private void EnterLabel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.C)
            {
                string s = EnterLabel.Text;
                Clipboard.SetData(DataFormats.StringFormat, s);
            }
        }

        
        private void историяToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            HistoryForm hisForm = new HistoryForm(ctl.his);
            hisForm.Show();
        }

        private void справкаStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ReferenceForm refForm = new ReferenceForm();
            refForm.Show();
        }

        private void правкаToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AppForm convForm = new AppForm();
            convForm.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void копироватьStripMenuItem_Click(object sender, EventArgs e)
        {
            //если вводили левый операнд
            if (ctl.CtrlState != Calculator.TCtrl.TCtrlState.cValDone)
                Calculator.TClipBoard.BUFFER = ctl.Proc.Lop_Res.Copy();
            //если вводили правый операнд
            else
                Calculator.TClipBoard.BUFFER = ctl.Proc.Rop.Copy();
        }

        private void вставитьStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Calculator.TClipBoard.BUFFER.b != 0)
            {
                //если вводили левый операнд ctl.Proc.Oprtn == Calculator.TProc.TOprtn.None && (ctl.CtrlState == Calculator.TCtrl.TCtrlState.cEditing || ctl.CtrlState == Calculator.TCtrl.TCtrlState.cStart)
                if (ctl.CtrlState == Calculator.TCtrl.TCtrlState.cStart || ctl.CtrlState == Calculator.TCtrl.TCtrlState.cEditing)
                {
                    ctl.Proc.Lop_Res = Calculator.TClipBoard.BUFFER.Copy();
                    EnterLabel.Text = ctl.Proc.Lop_Res.a_str;
                    ctl.CtrlState = Calculator.TCtrl.TCtrlState.cEditing;
                }
                //если вводили правый операнд  if (ctl.CtrlState == Calculator.TCtrl.TCtrlState.cValDone || ctl.CtrlState == Calculator.TCtrl.TCtrlState.FunDone)
                else
                {
                    ctl.Proc.Rop = Calculator.TClipBoard.BUFFER.Copy();
                    EnterLabel.Text = ctl.Proc.Rop.a_str;
                    ctl.CtrlState = Calculator.TCtrl.TCtrlState.cValDone;
                }
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
