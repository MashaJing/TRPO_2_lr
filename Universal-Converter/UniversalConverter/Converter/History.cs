using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalConverter.Converter
{
    //Структура для записи преобразования
    [Serializable]
    public struct Record
    {
        int p1;
        int p2;
        string number1;
        string number2;
        public Record(int p1, int p2, string n1, string n2)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.number1 = n1;
            this.number2 = n2;
        }

        public override string ToString()
        {
            return String.Format("{0} ({1}) <=> {2} ({3})", number1, p1, number2, p2);
        }
    }

    public class History
    {

        BinaryFormatter formatter = new BinaryFormatter();
        List<Record> L;

        //Чтение истории из файла
        public History()
        {
            
            using (FileStream fs = new 
                FileStream("c:/Users/mdzek/source/repos/Universal-Converter/UniversalConverter/Data/ConverterHistory.dat",
                FileMode.OpenOrCreate))
            {
                try 
                {
                    L = (List<Record>)formatter.Deserialize(fs);
                }
                catch(Exception e)
                {
                    L = new List<Record>();
                }
                //formatter.Serialize(fs, L);
            }

        }

        //Запись истории в файл
        ~History()
        {
            using (FileStream fs = new 
                FileStream("c:/Users/mdzek/source/repos/Universal-Converter/UniversalConverter/Data/ConverterHistory.dat",
                FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, L);
            }
        }

        //Геттер по индексу
        public Record GetRecord(int i)
        {
            if (i < L.Count && i >= 0)
                return L[i];
            else return new Record();
        }

        //Добавление записи
        public void AddRecord(int p1, int p2, string n1, string n2)
        {
            Record newRecord = new Record(p1, p2, n1, n2);
            L.Add(newRecord);
        }

        //Очитска Истории
        public void Clear()
        {
            L.Clear();
        }

        
        public int Count()
        {
            return L.Count;
        }
    }
}
