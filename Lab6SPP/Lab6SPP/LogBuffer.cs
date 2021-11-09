using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Lab6SPP
{
 /// <summary>
 /// Author Dmitry Miranovich
 /// Class LogBuffer
 /// status : Not Ready
 /// </summary>
    public class LogBuffer
    {
        private List<string> log;
        private TimerCallback tm;
        private Timer timer;
        private object timerBlock = new object();
        private string[] buffer, rsplBuffer;
        private int position = 0;
        private bool isFileInUse = false;
        private const int LIMIT = 20;
        private string path;
        public int Size
        {
            get
            {
                return log.Count;
            }
        }

        public LogBuffer(string path)
        {
            this.path = path;
            log = new List<string>();
            buffer = new string[LIMIT];
            rsplBuffer = new string[LIMIT];
            CreateFile();
            //tm = new TimerCallback(WriteBufferInTime);
            //timer = new Timer(tm, null, 0, 20); 
        }
        private void CreateFile()
        {
            try
            {
                using (FileStream stream = File.Create(path))
                {
                    stream.Close();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Add(string item)
        {
            log.Add(item);
            buffer[position++] = item;
            if (position >= buffer.Length)
            {
                string[] newBuffer = (string[])buffer.Clone();
                WriteBufferAsync(newBuffer);
                Reset();
            }
        }

        private Semaphore gate = new Semaphore(1,1);
        public async void WriteBufferAsync(string[] buffer)
        {
            Console.WriteLine("Выполнение записи файлов из буфера");
            await Task.Run(() => WriteBuffer(buffer));
            Console.WriteLine("Запись выполнена");
        }

        private void WriteBuffer(string [] buffer)
        {
            gate.WaitOne();
            try
            {
                File.AppendAllLines(path, buffer);
                Thread.Sleep(6000);
            }
            catch (IOException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                gate.Release();
            }
        }

        private void WriteBufferInTime(object s)
        {
           
        }
        private void Reset()
        {
            position = 0;
            isFileInUse = true;
            for(int i = 0; i<buffer.Length; i++)
            {
                buffer[i] = null;
            }
        }
    }
}
