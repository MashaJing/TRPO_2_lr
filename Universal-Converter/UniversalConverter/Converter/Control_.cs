using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalConverter.Converter
{
    class Control_
    {
        public History his = new History();
        public enum State { In_progress, Done }

        //Свойство для чтения и записи состояния Конвертера.
        public State St { get; set; }

        //Конструктор.
        public Control_()
        {
            St = State.In_progress;
            Pin = Const.pin;
            Pout = Const.pout;
        }
        //объект редактор
        public Editor ed = new Editor();

        //Свойство для чтения и записи основания системы с. р1.
        public int Pin { get; set; }

        //Свойство для чтения и записи основания системы с. р2.
        public int Pout { get; set; }

        //Выполнить команду конвертера.
        public string DoCmnd(int j)
        {
            if (j == 19)
            {
                double r = Conver_P_10.Do(ed.Number, (Int16)Pin, Const.accuracy);
                string res = Conver_10_P.Do(r, (Int32)Pout, acc());
                St = State.Done;
                his.AddRecord(Pin, Pout, ed.Number, res);
                return res;
            }
            else
            {
                St = State.In_progress;
                return ed.DoEdit(j);
            }

        }
        //Точность представления результата.
        private int acc()
        {
            return (int)Math.Round(Const.accuracy * Math.Log(Pin) / Math.Log(Pout) + 0.5);
        }
    }


}

