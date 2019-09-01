using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Stashdex {
    class Log {
        public static void log(Exception ex) {
            if (!File.Exists("Log.txt")) {
                File.Create("Log.txt");
            }
            using (StreamWriter sw = File.CreateText($"Log.txt")) {
                sw.WriteLine(ex.Message);
            }
        }
    }
}
