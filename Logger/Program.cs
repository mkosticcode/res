using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logger
{
    public class Program
    {
        static string path;
        static void Main(string[] args)
        {
            Console.ReadLine();
        }
        public static int Log(string ispis)
        {
            if (path == null)
            {
                path = System.IO.Path.GetDirectoryName(
      System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                path = path.Substring(6);
                path += "log.txt";

            }
            // string path = System.IO.Path.GetDirectoryName(
            //   System.Reflection.Assembly.GetAssembly(typeof(Logger)).Location);
            // path = Path.GetDirectoryName(path);
            // path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            // Console.WriteLine(ispis);
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);
            sw.WriteLine(ispis);
            sw.Close();
            stream.Close();
            return 0;
        }
    }
}
