using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalConverter.Calculator
{
    //класс "Редактор p-ичных чисел"
    class TEditor
    {
        public string number { set; get; }

        public TEditor() { number = "0,"; }

        public void changeSign()
        {
            if (number.Length == 0) return;
            if (number.First() == '-')
            {
                number = number.Remove(0, 1);
            }
            else
            {
                number = number.Insert(0, "-");
            }
        }

        public bool isZero()
        {
            if (number.Equals("0,")) return true; 
            else return false;
        }

        public string addDigit(int digit)
        {

            if (number.StartsWith("0") && number.Length == 1)
                addDelimeter();
            if (digit > 9)
                number += ((char)(digit+55)).ToString();
            else
                number += Convert.ToString(digit);
            
            return number;
        
        }

        public void addDelimeter()
        {
            //если запятая уже есть, удаляем её и ставим новую в конце
            if (number.Contains(Const.Sep))
            {
                number = number.Remove(number.IndexOf(Const.Sep), 1);
                while (number.StartsWith("0") && !isZero()) 
                    number = number.Remove(0, 1);
            }
            number += Const.Sep;    
            
        }

        public string Edit(int j)
        {
            if (j < 16)
            {
                number = addDigit(j);
            }
            else
            switch(j)
            {
                case 16:
                    addDelimeter();
                    break;

                case 17:
                    changeSign();
                    break;

                case 19:
                    Clear();
                    break;

                case 20:
                    backSpace();
                    break;
                }

            return number;
        }

        public string AddOprtnSign(int j)
        {
            switch (j)
            {
                case 21:
                    return "+";

                case 22:
                    return "-";

                case 23:
                    return "×";

                case 24:
                    return "/";

                case 25:
                    Clear();
                    return "^(-1)";

                case 26:
                    Clear();
                    return "^2";
            }
            return "";
        }

        public void backSpace()
        {
            if (number.Length>0)
                number = number.Remove(number.Length - 1, 1);
        }

        public string Clear()
        {
            number = "";
            return number;
        }
        
    }
}
