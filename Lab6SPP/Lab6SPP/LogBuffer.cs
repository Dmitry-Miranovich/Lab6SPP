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
        private string[] buffer, rsplBuffer;
        private int position = 0;
        private bool isFileInUse = true;
        private TimerCallback tm;
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
            //tm = new TimerCallback(WriteBuffer);
            //Timer timer = new Timer(tm, null, 0, 2000);
        }
        public void CreateFile()
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
                rsplBuffer = (string[])buffer.Clone();
                Reset();
                Thread thread = new Thread(WriteBuffer);
                thread.Start();
            }
        }

        public void WriteBuffer(object s)
        {
            while (isFileInUse)
            {
                try
                {
                    File.AppendAllLines(path, rsplBuffer);
                    isFileInUse = false;
                }
                catch (IOException)
                {
                    //File is being used by another process
                    //To prevent crushing our programm we evade our exeption by catch and in infinite loop wait, until appending is done
                }
            }
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
