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
namespace Writer
{
    class Program
    {
        static List<int> aktivniWorkeri = new List<int>();
        static void Main(string[] args)
        {
            Console.WriteLine("writer is running");
            SendDataToServer();
        }
        private static void SendDataToServer() // salje serializovane item LoadBalanceru
        {
            Console.WriteLine("Sending data");
            string address = "net.tcp://localhost:10000/ILoadBalancer";
            NetTcpBinding binding = new NetTcpBinding();

            ChannelFactory<ILoadBalancer> channel = new ChannelFactory<ILoadBalancer>(binding, address);

            ILoadBalancer proxy = channel.CreateChannel();
            aktivniWorkeri = proxy.retriveActiveWorkers();
           // Logger.Logger l = new Logger.Logger();
            try
            {
                bool izvrsavaj = true;
                ispis();
                while (izvrsavaj)
                {

                    Thread.Yield();
                    if (Console.KeyAvailable)
                    {

                        ConsoleKeyInfo key = Console.ReadKey(true);
                        switch (key.Key)
                        {
                            
                            case ConsoleKey.V:
                                Console.WriteLine("Pritisli ste  v ");
                                Console.WriteLine("unesite broj workera");
                                int i=Int32.Parse( Console.ReadLine());
                                proxy.AddWorker(i);

                                aktivniWorkeri = proxy.retriveActiveWorkers();
                                ispis();
                                Thread.Sleep(10);
                                break;
                            case ConsoleKey.S:
                                Console.WriteLine("Pritisli ste  S ");
                                Console.WriteLine("unesite broj workera");
                                int j = Int32.Parse(Console.ReadLine());
                                proxy.ShutWorker(j);

                                aktivniWorkeri = proxy.retriveActiveWorkers();
                                ispis();
                                Thread.Sleep(10);
                                break;
                            case ConsoleKey.I:
                                Console.WriteLine("Pritisli ste  I ");
                                ispisCodes();
                                Console.WriteLine("unesite code  itema:");

                                Item itC = new Item();
                                itC.Code = (Codes)Enum.Parse(typeof(Codes), Console.ReadLine());
                                Console.WriteLine("unesite vrednost  code:");
                                itC.Value = Double.Parse(Console.ReadLine());
                                proxy.DoItem(itC);
                                Logger.Program.Log(DateTime.Now + " Writer je poslao datoteku:" + itC);
                                ispis();
                                Thread.Sleep(10);
                                break;
                            default:
                                break;
                        }
                    }                                  
                    Item it = itemGenerator();
                    proxy.DoItem(it);
                     Console.WriteLine(it);
                    //l.Log(DateTime.Now + " Writer je poslao item:" + it);
                    Logger.Program.Log(DateTime.Now + " Writer je poslao datoteku:" + it);
                    Thread.Sleep(2000); 
                    
                }
              
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static Item itemGenerator()  //generise nasumicni item
        {
            Item it=new Item();
            Random r = new Random();
            Random x = new Random();
            Random s = new Random();
            int i = (1+r.Next());
            i = i % 8 + 1;
           
            double ds = s.NextDouble() * 1000;
            switch (i)
            {
                case 1:
                    it = new Item(Codes.CODE_ANALOG, ds);
                    break;
                case 2:
                    it = new Item(Codes.CODE_CONSUMER, ds);
                    break;
                case 3:
                    it = new Item(Codes.CODE_CUSTOM, ds);
                    break;
                case 4:
                    it = new Item(Codes.CODE_DIGITAL, ds);
                    break;
                case 5:
                    it = new Item(Codes.CODE_LIMITSET, ds);
                    break;
                case 6:
                    it = new Item(Codes.CODE_MULTIPLENODE, ds);
                    break;
                case 7:
                    it = new Item(Codes.CODE_SINGLENOE, ds);
                    break;
                case 8:
                    it = new Item(Codes.CODE_SOURCE, ds);
                    break;
                default:
                    it = new Item(Codes.CODE_SOURCE, 404);
                    break;                   
            }
            if (it.Code == Codes.CODE_DIGITAL)
            {
                int binary = (int)it.Value % 2;
                it.Value = binary;
            }
            return it;
        }
        private static void ispis()
        {
            Console.WriteLine("Da dodate novog workera pritisnite V");
            Console.WriteLine("Da uklonite postojeceg workera pritisnite S");
            Console.WriteLine("Da unesete kod rucno pritisnite I");
            Console.WriteLine("Trenutno aktivni workeri su:");
            foreach(int i in aktivniWorkeri)
            {
                Console.WriteLine(i);
            }
        }
        private static void ispisCodes()
        {
            Console.WriteLine("1.CODE_ANALOG");
            Console.WriteLine("2.CODE_DIGITAL");
            Console.WriteLine("3.CODE_CUSTOM");
            Console.WriteLine("4.CODE_LIMITSET");
            Console.WriteLine("5.CODE_SINGLENOE");
            Console.WriteLine("6.CODE_MULTIPLENODE");
            Console.WriteLine("7.CODE_CONSUMER");
            Console.WriteLine("8.CODE_SOURCE");
            foreach (int i in aktivniWorkeri)
            {
                Console.WriteLine(i);
            }
        }

        /* private static async Task<string> GetInputAsync()
         {
             return await Task.Run(() => Console.ReadLine());
         } */

    }
}
