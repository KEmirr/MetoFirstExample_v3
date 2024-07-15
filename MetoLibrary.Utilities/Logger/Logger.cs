using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MetoLibrary.Utilities.Logger
{
    public static class Logger
    {
        private static readonly string logFilePath = "log.txt"; // Log dosyasının yolu

        public static void Log(string message)
        {
            string logMessage = $"{DateTime.Now}: {message}";
            // Dosyaya yazma
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(logMessage);
            }
        }

        public static void Log(Exception ex)
        {
            string logMessage = $"{DateTime.Now}: {ex.Message}\n{ex.StackTrace}";
            // Dosyaya yazma
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(logMessage);
            }
        }
    }
}
