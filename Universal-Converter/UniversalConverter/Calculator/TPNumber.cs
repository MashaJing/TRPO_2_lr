using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalConverter.Calculator
{
    class TPNumber
    {

        //значение числа
        public double a { get; set; }

        //основание системы
        public int b { get; set; }

        //точность вычислений
        public int c { get; set; }

        //=========================================================

        //значение числа
        public string a_str { get; set; }

        //точность вычислений
        public string c_str { get; set; }

        //основание системы
        public string b_str { get; set; }

        public TPNumber() { }

        public TPNumber(double number, int basement, int accuracy)
        {
            a = number;
            b = basement;
            c = accuracy;
            a_str = Convert.ToString(a);
            b_str = Convert.ToString(b);
            c_str = Convert.ToString(c);
        }

        public TPNumber(string number, string basement, string accuracy)
        {
            a_str = number;
            b_str = basement;
            c_str = accuracy;
            updateNumberFields();
        }

        public void updateNumberFields()
        {
            b = Convert.ToInt32(b_str);
            c = Convert.ToInt32(c_str);
            if (b == 10)
                a = Convert.ToDouble(a_str);
            else
            {
                int sign;
                string withoutSign;
                //обработка отрицательных чисел
                if (a_str.StartsWith("-"))
                {
                    sign = -1;
                    withoutSign = a_str.Remove(0, 1);
                }
                else
                {
                    sign = 1;
                    withoutSign = a_str;
                }

                a = sign * Converter.Conver_P_10.Do(withoutSign, b, 10);
            }


        }

        public void SetValue(string newA)
        {
            if (newA.Length != 0 && newA != ",")
            {
                a_str = newA;
                updateNumberFields();
            }
        }


        public void SetValue(string newA, string newB, string newC)
        {
            if (newA.Length != 0 && newA != ",")
            {
                a_str = newA;
                b_str = newB;
                c_str = newC;
                updateNumberFields();
            }

        }

        public void SetBasement(int newB)
        {
            b_str = newB.ToString();
            b = newB;
            if (b == 10)
                a_str = Convert.ToString(a);
            else
            {
                    //обработка отрицательных чисел
                if (a < 0)
                    a_str = "-";
                else
                    a_str = "";

                    a_str += Converter.Conver_10_P.Do(Math.Abs(a), b, c+1);

            }
        }

        public void SetBasement(string newB)
        {
            b_str = newB;
            b = Convert.ToInt32(newB);

            if (b == 10)
                a_str = Convert.ToString(a);
            else
            {
                //обработка отрицательных чисел
                if (a < 0)
                    a_str = "-";
                else
                    a_str = "";

                a_str += Converter.Conver_10_P.Do(Math.Abs(a), b, c + 1);

            }
        }

        public void SetPrecision(int newC)
        {
            c_str = newC.ToString();
            c = newC;
        }

        public void SetPrecision(string newC)
        {
            c_str = newC;
            c = Convert.ToInt32(newC);
        }


        public TPNumber Copy()
        {
            TPNumber n = new TPNumber(a_str, b_str, c_str);
            n.updateNumberFields();
            return n;
        }

        public TPNumber Sum(TPNumber Num2)
        {
            if (b != Num2.b)  throw new Exception("С.с. операндов должны быть равны!");
            TPNumber Num3 = new TPNumber(Math.Round(a + Num2.a,c), b, c);
            return Num3;
        }

        public TPNumber Substract(TPNumber Num2)
        {
            if (b != Num2.b) throw new Exception("С.с. операндов должны быть равны!");
            TPNumber Num3 = new TPNumber(Math.Round(a - Num2.a,c), b, c);
            return Num3;
        }

        public TPNumber Multiply(TPNumber Num2)
        {
            if (b != Num2.b) throw new Exception("С.с. операндов должны быть равны!");
            TPNumber Num3 = new TPNumber(Math.Round(a * Num2.a, c), b, c);
            return Num3;
        }

        public TPNumber Devide(TPNumber Num2)
        {
            if (b != Num2.b) throw new Exception("С.с. операндов должны быть равны!");
            if (Num2.a == 0) throw new Exception("Деление на ноль тут запрещено!");
            TPNumber Num3 = new TPNumber(Math.Round(a / Num2.a, c), b, c);
            return Num3;
        }

        public TPNumber Inverse()
        {
            if (a == 0) throw new Exception("Для 0 обращение невозможно!");
            TPNumber Num3 = new TPNumber(Math.Round(1 /a, c), b, c);
            return Num3;
        }

        public TPNumber Squared()
        {
            TPNumber Num3 = new TPNumber(Math.Round(Math.Pow(a, 2), c), b, c);
            return Num3;
        }


    }
}
