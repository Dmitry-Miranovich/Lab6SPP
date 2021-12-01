using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
 /// status : Ready
 /// </summary>
    public class LogBuffer
    {
        private Timer timer;
        private string[] buffer;
        private int position = 0;
        private const int LIMIT = 50;
        private string path;

        public LogBuffer()
        {
            buffer = new string[LIMIT];
            timer = new Timer(new TimerCallback(WriteBufferInTimeAsync), null, 0,5000);
        }
        public void CreateFile(string path)
        {
            this.path = path;
            File.OpenWrite(path);
        }

        public void Add(string item)
        {
            try
            {
                item = string.Format(DateTime.Now + ": " + item);
                buffer[position++] = item;
                if (position >= buffer.Length)
                {
                    string[] newBuffer = (string[])buffer.Clone();
                    WriteBufferAsync(newBuffer);
                    Reset();
                }
            }
            catch(Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine(DateTime.Now + ": "+ ex.Message);
                }
            }
            
        }

        private Semaphore gate = new Semaphore(1,1);
        public async void WriteBufferAsync(string[] buffer)
        {
            Console.WriteLine("Выполнение записи файлов из буфера");
            await Task.Run(() => WriteBuffer(buffer));
            

            Console.WriteLine("Запись выполнена");
        }
        public async void WriteBufferInTimeAsync(object obj)
        {
            gate.WaitOne();
            string[] secBuffer = (string[])buffer.Clone();
            int bufSize = position;
            await Task.Run(() => WriteBufferInTime(secBuffer, bufSize));
            Reset();
        }

        private void WriteBuffer(string [] buffer)
        {
            try
            {
                gate.WaitOne();
                File.AppendAllLines(path, buffer);
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

        private void WriteBufferInTime(string[] buffer, int bufSize)
        {
            try
            {
                if(bufSize > 0)
                {
                    Console.WriteLine("Выполнение записи файлов из буфера по таймеру");
                    File.AppendAllLines(path, buffer);
                    Console.WriteLine("Запись по таймеру выполнена");
                }
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message);
            }
            finally
            {
                gate.Release();
            }
        }
        private void Reset()
        {
            position = 0;
            for(int i = 0; i<buffer.Length; i++)
            {
                buffer[i] = null;
            }
        }
    }
}
