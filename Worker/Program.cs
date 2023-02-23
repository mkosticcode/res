using LBWorkerLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Worker
{
    public class Program
    {
        static void Main(string[] args)
        {
            StartServer();
            Console.WriteLine("gg");
            
        }
        // pokrene se novi exe i nakon sto primi podatke koji worker treba da bude ugasi
        //pocetni port da bi mogli da se naprave novi workeri
        private static void StartServer()
        {
           // Worker w = new Worker(IPAddress.Loopback, 10001,1);
          //  w.StartWorker(); 
            IPAddress address = IPAddress.Loopback;
            IPEndPoint endPoint = new IPEndPoint(address, 20000);
            //Task workerTask = Task.Factory.StartNew(() => workers.Add(1, new Worker(address, 10001, 1))); ne radi
            //Task workerTask = Task.Factory.StartNew(() => workers[1].StartWorker()); radi
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                serverSocket.Bind(endPoint);
                serverSocket.Listen(10); 
                Socket socket = serverSocket.Accept();
                serverSocket.Dispose();
                Run(socket);

                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            } 
        }
        private static int Run(Socket socket) //pravi novog workera i gasi pocetni tcp Socket
        {
            Console.WriteLine("Receiving data");

            NetworkStream stream = new NetworkStream(socket);
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { NewLine = "\r\n", AutoFlush = true };
          
            string request = reader.ReadLine();
            string[] tokens = request.Split('-');
            Worker w = new Worker(IPAddress.Loopback, Int32.Parse(tokens[1]), Int32.Parse(tokens[2]));
            Console.WriteLine(request);
            w.StartWorker();
            reader.Close();
            stream.Close();           
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            return 0;
        }

    }
    
}
