using System;

namespace Lab6SPP
{
    
    public class Program
    {
        static void Main()
        {
            LogBuffer log = new LogBuffer("D:\\Третий курс\\СПП\\Lab6SPP\\Log.txt");
            log.CreateFile();
            for(int i = 0; i<40; i++)
            {
                log.Add($"Hello World! {i}");
            }
        }
    }
}
