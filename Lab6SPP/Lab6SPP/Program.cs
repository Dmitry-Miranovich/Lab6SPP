using System;
using System.Threading;

namespace Lab6SPP
{
    
    public class Program
    {
        static void Main()
        {
            LogBuffer log = new LogBuffer("D:\\Третий курс\\СПП\\Lab6SPP\\Log.txt");
            for(int i = 0; i<20; i++)
            {
                log.Add($"Hello World! {i}");
            }
        }
    }
}
