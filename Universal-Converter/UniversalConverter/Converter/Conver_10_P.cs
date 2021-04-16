using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalConverter.Converter
{
    abstract class Conver_10_P
    {
        //Преобразовать целое в символ.
        public static char int_to_Char(long n) 
        {   
            switch(n)
            {
                case 0:
                    return '0';
                    break;
                case 1:
                    return '1';
                    break;
                case 2:
                    return '2';
                    break;
                case 3:
                    return '3';
                    break;
                case 4:
                    return '4';
                    break;
                case 5:
                    return '5';
                    break;
                case 6:
                    return '6';
                    break;
                case 7:
                    return '7';
                    break;
                case 8:
                    return '8';
                    break;
                case 9:
                    return '9';
                    break;
                case 10:
                    return 'A';
                    break;
                case 11:
                    return 'B';
                    break;
                case 12:
                    return 'C';
                    break;
                case 13:
                    return 'D';
                    break;
                case 14:
                    return 'E';
                    break;
                case 15:
                    return 'F';
                    break;
                default:
                    throw new Exception("Недопустимый остаток");


            }
        }



        //Преобразовать десятичное целое в с.с. с основанием р.
        public static string int_to_P(long n, int p) 
        {
            String result = "";
            long temp = System.Math.Abs(n);
            Stack<char> stack = new Stack<char>();

            if (n < 0)
            {
                result = "-";
            }
            
            if(temp==0)
            {
                result += '0';
            }
               
            while(temp != 0)
            {
                try
                {
                    stack.Push(int_to_Char(temp % p));
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                temp /= p;


            }
            while(stack.Count !=0)
            {
                result += stack.Pop();
            }

            return result;
        }



        //Преобразовать десятичную дробь в с.с. с основанием р.
        public static string flt_to_P(double n, int p, int c)
        {
            double temp = System.Math.Abs(n);
            double mult;
            String result = "";
            if (temp == 0)
            {
                result += '0';
            }

            while (c != 0 && temp % 1 > Const.EPS)
            {
                mult = temp * p;
                mult = Math.Round(mult, 10);

                try 
                { 
                    result += int_to_Char((int)mult);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                temp = mult % 1;
                c--;
            }
            return result;
        }



        //Преобразовать десятичное действительное число
        //в с.с. с основанием р и точностью с знаков.
        public static string Do(double n, int p, int c)
        {
            String v1 = int_to_P((long)n, p);
            String v2 = flt_to_P(n%1, p, c);
            return v1 + Const.Sep + v2;

        }
    }

}
