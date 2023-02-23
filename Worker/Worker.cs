using LBWorkerLibrary;
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

namespace Worker
{
    public class Worker:IWorker
    {
        IPAddress address;
        int port;
        int workerNo;
        Reader rd;
        ServiceHost host;

        public IPAddress Address { get => address; set => address = value; }
        public int Port { get => port; set => port = value; }
        public int WorkerNo { get => workerNo; set => workerNo = value; }
        public Reader Rd { get => rd; set => rd = value; }

        public Worker()
        {
        }

        public Worker(IPAddress address, int port, int workerNo)
        {
            Address = address;
            Port = port;
            WorkerNo = workerNo;
            rd = new Reader();
        }
        public void StartWorker()
        {

            using (ServiceHost host = new ServiceHost(typeof(Worker)))
            {
                string address = "net.tcp://localhost:"+ port+"/IWorker";
                NetTcpBinding binding = new NetTcpBinding();

                host.AddServiceEndpoint(typeof(IWorker), binding, address);

                host.Open();
                Console.WriteLine($"Servis je uspesno pokrenut na adresi : {address}");
                Console.ReadKey();
                host.Close();
            }
           
        }
        

        public override string ToString()
        {
            string a =address.ToString();
            return a + "-" + port + "-"  + workerNo  ;
        }
        /*public void CloseWorker(Socket socket)
        {
            try
            {
                Console.WriteLine("kraj komunikacije, zatvaram workera");
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }*/

        public void DoWork(ItemDescription itd)
        {
            Console.WriteLine(itd);
            CollectionDescription cd = new CollectionDescription(itd);
            rd = new Reader();
            DataSet ds=rd.Select(cd);
            if (ds == null)
            {
                rd.Insert(cd);
                Logger.Program.Log(DateTime.Now + " Worker je insertovao dataset:" + ds);
            }
            else if (DiffrentUpdate(ds,cd.DescriptionDataSet))
            {
                rd.Update(cd);
                Logger.Program.Log(DateTime.Now + " Worker je updajtovao dataset:" + ds);

            }

        }
        public bool DiffrentUpdate(DataSet newDS,DataSet oldDS)
        {
            if (newDS.Second == ProjectLibrary.Codes.CODE_DIGITAL) { return true; }
            double ratio, ratioSecond;
            if (newDS.FirstValue<oldDS.FirstValue)
            {
               ratio = newDS.FirstValue / oldDS.FirstValue;
            }
            else
            {
                 ratio = oldDS.FirstValue / newDS.FirstValue;
            }
            if (newDS.SecondValue < oldDS.SecondValue)
            {
                 ratioSecond = newDS.SecondValue / oldDS.SecondValue;
            }
            else
            {
                 ratioSecond = oldDS.SecondValue / newDS.SecondValue;
            }

            if (ratio<0.98 || ratioSecond < 0.98)
            {
                return true;
            }
            else { return false; }
        }
        public void ShutWorker()
        {
            System.Environment.Exit(0);
            //host.Close();
        }
       
    }
}
