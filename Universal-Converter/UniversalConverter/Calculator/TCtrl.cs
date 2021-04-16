using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalConverter.Calculator
{
    //управление выполнением команд калькулятора
    class TCtrl
    {
        public enum TCtrlState { cStart, cEditing, FunDone, cValDone, cExpDone, cOpChange, cError };
        /* cStart (Начальное), 
         * cEditing (Ввод и редактирование), 
         * cExpDone (Выражение вычислено), 
         * cOpDone (Операция выполнена), 
         * cValDone (Значение введено), 
         * cOpChange (Операция изменена),
         * cError (Ошибка)
         */

        public TCtrlState CtrlState { get; set; }
        public TEditor Editor;
        public TProc Proc;
        public TMemory Memory;
        public History his;
       // public TPNumber Number;

        public TCtrl()
        {
            CtrlState = TCtrlState.cStart;
            Editor = new TEditor();
            Proc = new TProc();
            Memory = new TMemory();
            his = new History();
        }

        ~TCtrl() {
            CtrlState = TCtrlState.cStart;
            Editor = null;
            Proc = null;
            Memory = null;

        }

        public string RunEditorCommand(int a)
        {
            if (a == 18)
            {
                SetDefaultState();
                Editor.number = "0";
                return Editor.number;
            }
            else if (a == 20 && CtrlState == TCtrlState.cOpChange || CtrlState == TCtrlState.FunDone)
            {
                //если хотят стереть знак операции
                Proc.Oprtn = TProc.TOprtn.None;
                return Proc.Lop_Res.a_str;
            }
            else
            {
                return Editor.Edit(a);
            }
        }
        
        public string RunMemoryCommand(int j)
        {
            switch (j)
            {
                case 28:
                    Memory.Clear();
                    return Editor.number;

                case 29:
                    if (CtrlState == TCtrlState.cValDone || CtrlState == TCtrlState.cOpChange)
                    {
                        Proc.Rop.SetValue(Memory.FNumber.a_str, Memory.FNumber.b_str, Memory.FNumber.c_str);
                        CtrlState = TCtrlState.cValDone;
                    }
                    else
                    {
                        Proc.Lop_Res.SetValue(Memory.FNumber.a_str, Memory.FNumber.b_str, Memory.FNumber.c_str);
                        CtrlState = TCtrlState.cEditing;
                    }
                    break;

                case 30:
                    {
                        Memory.Store(Proc.Lop_Res);
                        return Editor.number;
                    }

                case 31:
                    Memory.Add(Proc.Lop_Res);
                    Proc.Lop_Res.SetValue(Memory.FNumber.a_str);
                    break;
            }

            return Memory.FNumber.a_str;
        }

        /*public string RunClipBoardCommand(int a, string b)
        {

        }*/

        public string Calculate()
        {
            //если выполнили операцию
            if (Proc.Oprtn != TProc.TOprtn.None)
            {
                //создаем временные переменные для записи в историю левого операнда и id операции
                string Lop_temp = Converter.Conver_10_P.Do(Proc.Lop_Res.a, Proc.Lop_Res.b, Proc.Lop_Res.c);
                int operation_id = (int)Proc.Oprtn;

                //производим операцию
                Proc.OprtnRun();

                //записываем операцию в историю
                his.AddRecord(Proc.Lop_Res.b, operation_id, Lop_temp, Converter.Conver_10_P.Do(Proc.Rop.a,Proc.Rop.b,Proc.Rop.c),
                     Converter.Conver_10_P.Do(Proc.Lop_Res.a, Proc.Lop_Res.b, Proc.Lop_Res.c));
                
                CtrlState = TCtrlState.cExpDone;
                //Proc.Rop.SetValue("0");
            }
            //если выполнили функцию
            else
            {
                //создаем временные переменные для записи в историю левого операнда и id функции
                string Lop_temp = Converter.Conver_10_P.Do(Proc.Lop_Res.a, Proc.Lop_Res.b, Proc.Lop_Res.c);

                //выисляем функцию
                Proc.FuncRun();

                //записываем операцию в историю
                his.AddRecord(Proc.Lop_Res.b, (int)Proc.Func, Lop_temp, Proc.Rop.a_str, 
                    Converter.Conver_10_P.Do(Proc.Lop_Res.a, Proc.Lop_Res.b, Proc.Lop_Res.c));

                CtrlState = TCtrlState.FunDone;
                Proc.OprtnClean();
            }

            //результаты вычислений переводим в заданную с.с.
            if (Proc.Lop_Res.b != 10)
            {
                //обработка отрицательных чисел
                if (Proc.Lop_Res.a < 0)
                    Proc.Lop_Res.a_str = "-";
                else
                    Proc.Lop_Res.a_str = "";

                Proc.Lop_Res.a_str += Converter.Conver_10_P.Do(Math.Abs(Proc.Lop_Res.a), Proc.Lop_Res.b, Proc.Lop_Res.c);
                
            }

            return Proc.GetLopCopy().a_str;
        }

        public void SetDefaultState() 
        {
            //TCtrl
            CtrlState = TCtrlState.cEditing;
            //TProc
            Proc.Oprtn = TProc.TOprtn.None;
            Proc.Lop_Res.SetValue("0,");
            Proc.Rop.SetValue("0");
            
        }

        public void SetOprtnFunc(int i)
        {
            
            if (i <= 24)
            {
                Proc.Oprtn = (TProc.TOprtn)i;
                //устанавливаем контроллеру статус "смена операции"
                CtrlState = TCtrlState.cOpChange;
            }
            else if (i <= 26)
            { 
                Proc.Func = (TProc.TFunc)i;

                //если хотим поменять в правом операнде
                if (CtrlState == TCtrlState.cValDone)
                {
                    switch (i)
                    {
                        case 25:
                            Proc.Rop = Proc.Rop.Inverse();
                            break;

                        case 26:
                            Proc.Rop = Proc.Rop.Squared();
                            break;
                    }
                }
                //устанавливаем контроллеру статус "функция установлена"
                CtrlState = TCtrlState.FunDone;

            }
        }
    }
}
