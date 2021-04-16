using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalConverter.Calculator
{

    class TMemory
    {

        public TPNumber FNumber;

        //текущее состояние памяти
        public enum State { _Off, _On };

        public State St { set; get; }

        public TMemory()
        {
            FNumber = new TPNumber("0,", "10", "1");
            St = State._Off;
        }


        public void Store(TPNumber E)
        {
            FNumber.SetValue(E.a_str, E.b_str, E.c_str);
            St = State._On;
        }

        public TPNumber GetNumberCopy()
        {
            St = State._On;
            return FNumber.Copy();
        }

        public void Add(TPNumber E)
        {
            if (E.b != 10)
            {
                FNumber.SetValue(Converter.Conver_10_P.Do(FNumber.Sum(E).a, E.b, E.c));
                FNumber.a_str = Converter.Conver_10_P.Do(FNumber.a, E.b, E.c);
            }
            else
                FNumber.SetValue(FNumber.Sum(E).a_str);

            St = State._On;
        }

        public string GetMemoryState()
        {
            return Convert.ToString(St);
        }

        public void Clear()
        {
            FNumber = new TPNumber("0,", "10", "1");
            St = State._Off;
        }
    }
}
