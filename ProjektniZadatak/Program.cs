using LBWorkerLibrary;
using ProjectLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjektniZadatak
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            StartServer();
       //     Logger l
        }
        public static void StartServer()
        {
            //napraviti dictionary sa workerima i proxijima
            LoadBalancer lb = new LoadBalancer();
           // lb.MakeEmptyItemDescription(lb.ItemDSCIndex);
            using (ServiceHost host = new ServiceHost(typeof(LoadBalancer)))
            {
                string address = "net.tcp://localhost:10000/ILoadBalancer";
                NetTcpBinding binding = new NetTcpBinding();

                host.AddServiceEndpoint(typeof(ILoadBalancer), binding, address);

                host.Open();
                Console.WriteLine($"Servis je uspesno pokrenut na adresi : {address}");
                Console.ReadKey();
                host.Close();
            }
            Console.WriteLine("Server start");
           /* IPAddress address = IPAddress.Loopback;
            IPEndPoint endPoint = new IPEndPoint(address, 10000);
            
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
       
            try
            {
                serverSocket.Bind(endPoint);
                serverSocket.Listen(10);
                while (true)
                {
                    Socket socket = serverSocket.Accept();
                    Task<int> task = Task.Factory.StartNew(() => Run(socket));

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            } */
        }
        //Pravi itemDescriptione i nakon sto ih popuni
        //sa item od writera salje ih workeru na obradu serijalizovane
        public static int Run(Socket socket) 
        {
            Console.WriteLine("Receiving data");

            NetworkStream stream = new NetworkStream(socket);
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { NewLine = "\r\n", AutoFlush = true };
            bool izvrsavaj = true;
            

            while (izvrsavaj)
            {
                Console.WriteLine("izvrsavaj");
                string request = reader.ReadLine();
                switch (request)
                {
                   
                  /*  case "SHUTWORKER":
                        int oldWorkerID = Int32.Parse(reader.ReadLine());
                        RemoveWorker(oldWorkerID, IPAddress.Loopback);
                        break; */
                }
               
          
            }
           
            writer.Close();
            reader.Close();
            stream.Close();

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            return 0;
        } 
       

    }
}
