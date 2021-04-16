using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalConverter.Converter
{
    static class Const
    {
        public const Char Sep = ',';
        public const Char zero = '0';
        public const double EPS = 1e-10;

        //Основание системы с. исходного числа. 
        public const int pin = 10;
        //Основание системы с. результата. 
        public const int pout = 16;
        //Число разрядов в дробной части результата. 
        public const int accuracy = 10;

    }
}
