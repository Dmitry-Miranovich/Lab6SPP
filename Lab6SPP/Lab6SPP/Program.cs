using System;
using System.Threading;

namespace Lab6SPP
{
    
    public class Program
    {
        static void Main()
        {
            string path = "D:\\Третий курс\\СПП\\Lab6SPP\\Log.txt";
            LogBuffer log = new LogBuffer();
            log.CreateFile(path);
            for(int i = 0; i<135; i++)
            {
                log.Add($"Hello World! {i}");
            }
            Console.ReadLine();
        }
    }
}
