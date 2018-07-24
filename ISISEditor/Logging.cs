using System;
using System.IO;
using System.Text;
using System.Reflection;
namespace ISISEditor
{
    public class Logging
    {
        private string _logFile;
        private static readonly object sync = new object();

        internal void CreateLogFile()
        {            
            string fileName = "ISISEditor_" + DateTime.Now.ToString().Replace(":", "-") + ".log";
            string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logFile = appPath + @"\" + fileName;
            _logFile = $"{logFile}";
            
            FileStream fs = File.Create(logFile);
            fs.Close();
        }

        public void WriteLine(string log)
        {
            try
            {
                lock (sync)
                {
                    File.AppendAllText(_logFile, log + "\n", Encoding.GetEncoding("Windows-1251"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }
                
    }
}