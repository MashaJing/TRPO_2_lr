using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalConverter.Converter
{
    class Editor
    {
        //Поле для хранения редактируемого числа.
        string number = "";


        //Свойство для чтения редактируемого числа.
        public string Number
        { get { return number; } }


        //Добавить цифру.
        public string AddDigit(int n)
        {

            String temp = number;
            temp += Conver_10_P.int_to_Char(n);
            number = temp;
            return temp;
        }


        //Точность представления результата.
        /*public int Acc() { 
      }
        */

        //Добавить ноль.
        public string AddZero()
        {
            String temp = number;
            temp += '0';
            number = temp;
            return temp;
        }


        //Добавить разделитель.
        public string AddSep()
        {
            String temp = number;
            temp += Const.Sep;
            number = temp;
            return temp;
        }


        //Удалить символ справа.
        public string Bs()
        {
            if (number.Length != 0)
            {
                String temp = number;
                temp = temp.Remove(temp.Length - 1);
                number = temp;
                return temp;
            }
            else
                return "";
            
        }


        //Очистить редактируемое число.
        public string Clear()
        {

            String temp = "";
            number = temp;
            return temp;
        }

        //Вычислить длину строки
        public int Length()
        {
            return number.Length;
        }

        //Выполнить команду редактирования.
        public string DoEdit(int j)
        {

                if (j <= 15 && j>-1) return AddDigit(j);
                if (j == 16) return AddSep();
                if (j == 17) return Bs();
                if (j == 18) return Clear();   
                return Number;
            
            
        }

    }

}
