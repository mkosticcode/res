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
using Worker;

namespace ProjektniZadatak
{
    public class LoadBalancer : ILoadBalancer
    {
         private Dictionary<int, Worker.Worker> workers;
         private List<ItemDescription> itemsDSC;
         private int itemDSCIndex;
        private Dictionary<int, IWorker> workerProxys;
        private List<int> aktivniWorkeri;
        private int roundRobinIterator;
        public Dictionary<int, Worker.Worker> Workers { get => workers; set => workers = value; }
        public List<ItemDescription> ItemsDSC { get => itemsDSC; set => itemsDSC = value; }
        public int ItemDSCIndex { get => itemDSCIndex; set => itemDSCIndex = value; }
        public Dictionary<int, IWorker> WorkerProxys { get => workerProxys; set => workerProxys = value; }
        public List<int> AktivniWorkeri { get => aktivniWorkeri; set => aktivniWorkeri = value; }
        public int RoundRobinIterator { get => roundRobinIterator; set => roundRobinIterator = value; }

        public LoadBalancer()
        {
            ItemDSCIndex = 1;
            Workers = new Dictionary<int, Worker.Worker>();
            ItemsDSC = new List<ItemDescription>();
            workerProxys = new Dictionary<int, IWorker>();
            AktivniWorkeri = new List<int>();
            RoundRobinIterator = 0;
            try
            {
                // MakeEmptyItemDescription(ItemDSCIndex);
                itemsDSC = UcitajItemDescriptione();
            }
            catch(Exception e) {
                 MakeEmptyItemDescription(ItemDSCIndex);
            }
            IspisiItemDescriptin(ItemsDSC);

            /*   AddWorkerProxy(1);
               while (true)
               {
                   SendDataToWorker(ItemsDSC[3], 1);
                   Thread.Sleep(2000);

               } */
        }
        public void AddWorker(int workerID)
        {
            AddWorker(workerID, IPAddress.Loopback);
            AddWorkerProxy(workerID);
            //Logger.Logger l = new Logger.Logger();
            // l.Log(DateTime.Now + " LoadBalanser je pokrenuo workera:" + workerID);
            Logger.Program.Log(DateTime.Now + " LoadBalanser je pokrenuo workera:" + workerID);

        }
        public void AddWorkerProxy(int workerID)
        {
            if (workerProxys.ContainsKey(workerID) || workerID <= 0)
            {

                return;
            }
            int port = 10000 + workerID;
            string address = "net.tcp://localhost:"+port+"/IWorker";
            NetTcpBinding binding = new NetTcpBinding();

            ChannelFactory<IWorker> channel = new ChannelFactory<IWorker>(binding, address);

            IWorker proxy = channel.CreateChannel();
            WorkerProxys.Add(workerID, proxy);
        }

        public void DoItem(Item it)
        {
            Console.WriteLine(it);
            int index = indexCalculator(it);
            FillItemDescription(it, index);        
        }

        public void ShutWorker(int workerID)
        {
            IWorker proxy = workerProxys[workerID];
            try
            {
                proxy.ShutWorker();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            WorkerProxys.Remove(workerID);
            AktivniWorkeri.Remove(workerID);
            ItemDSCIndex--;
            Workers.Remove(workerID);
            Logger.Program.Log(DateTime.Now + " LoadBalanser je ugasio workera:" + workerID);
            if (roundRobinIterator >= AktivniWorkeri.Count) { roundRobinIterator = 0; }
        }
        public void SendDataToWorker(ItemDescription idc, int i)// salje objekte workeru trenutno harkodovano ka jednom bez rasporedjivanja
        {
           // Worker.Worker w = Workers[i];
            IWorker proxy = workerProxys[i];
            Logger.Program.Log(DateTime.Now + " LoadBalanser je poslao ItemDescription:" + idc);
            try
            {
                proxy.DoWork(idc);
                cleanIdc(idc);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //Logger.Logger l = new Logger.Logger();
            //l.Log(DateTime.Now + " LoadBalanser je poslao ItemDescription:" + idc);

        }
        public void cleanIdc(ItemDescription idc)
        {
            idc.ItemsList = new List<Item>();
            idc.DescriptionDataSet.Capacity = EnteredData.empty;
        }
        public  void StartWorkerExe(Worker.Worker w)// pokrece workera kao posebni exe proces
        {
            System.Diagnostics.Process.Start("worker.exe");
            IPAddress address = IPAddress.Loopback;
            IPEndPoint endPoint = new IPEndPoint(address, 20000);
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Connect(endPoint);
            NetworkStream stream = new NetworkStream(serverSocket);
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { NewLine = "\r\n", AutoFlush = true };
            writer.WriteLine(w);

            Console.WriteLine("Napravljen je worker:"+w.WorkerNo);
            /* writer.Close();
             reader.Close();
             stream.Close();

             serverSocket.Shutdown(SocketShutdown.Both);
             serverSocket.Close(); */
        }
        public void MakeEmptyItemDescription(int id)// pravi prazne itemDescrtiption koje dodaje u listu 
        {
            DataSet first = new DataSet(1, Codes.CODE_ANALOG, Codes.CODE_DIGITAL, 0, 0, EnteredData.empty);
            DataSet second = new DataSet(2, Codes.CODE_CUSTOM, Codes.CODE_LIMITSET, 0, 0, EnteredData.empty);
            DataSet third = new DataSet(3, Codes.CODE_SINGLENOE, Codes.CODE_MULTIPLENODE, 0, 0, EnteredData.empty);
            DataSet fourth = new DataSet(4, Codes.CODE_CONSUMER, Codes.CODE_SOURCE, 0, 0, EnteredData.empty);
            ItemDescription i1 = new ItemDescription(id);
            ItemDescription i2 = new ItemDescription(id + 1);
            ItemDescription i3 = new ItemDescription(id + 2);
            ItemDescription i4 = new ItemDescription(id + 3);
            i1.DescriptionDataSet = first;
            i2.DescriptionDataSet = second;
            i3.DescriptionDataSet = third;
            i4.DescriptionDataSet = fourth;

            ItemsDSC.Add(i1);
            ItemsDSC.Add(i2);
            ItemsDSC.Add(i3);
            ItemsDSC.Add(i4);

        }
            //racuna koji ce index biti u List itemdescriptin
            // na osnovu item code zato sto se datasetovi ponavljaju nakon svakog cetvrtog 
        public int indexCalculator(Item i)
        {
            int index = -1;
            switch (i.Code)
            {
                case Codes.CODE_ANALOG:
                case Codes.CODE_DIGITAL:
                    index = 1;//ItemDSCIndex;
                    break;
                case Codes.CODE_CUSTOM:
                case Codes.CODE_LIMITSET:
                    index = 2;// ItemDSCIndex + 1;
                    break;
                case Codes.CODE_SINGLENOE:
                case Codes.CODE_MULTIPLENODE:
                    index = 3;// ItemDSCIndex + 2;
                    break;
                case Codes.CODE_CONSUMER:
                case Codes.CODE_SOURCE:
                    index = 4;// ItemDSCIndex + 3;
                    break;

            }
            return index;
        }
        public void FillItemDescription(Item i, int id)//popunjava ItemDescriptione i one koje su popunjene salje ka workeru
        {
            int idRobin= id + 4 * RoundRobinIterator;
          //  id += 4 * RoundRobinIterator;
            foreach (ItemDescription IDC in ItemsDSC)
            {
                if (idRobin == IDC.Id)
                {
                    IDC.ItemsList.Add(i);
                    if (IDC.DescriptionDataSet.First == i.Code)
                    {
                        IDC.DescriptionDataSet.FirstValue = i.Value;
                        if (IDC.DescriptionDataSet.Capacity == EnteredData.empty || IDC.DescriptionDataSet.Capacity == EnteredData.left)
                        {
                            IDC.DescriptionDataSet.Capacity = EnteredData.left;
                        }
                        else
                        {
                            IDC.DescriptionDataSet.Capacity = EnteredData.full;
                            if (AktivniWorkeri.Count > 0)
                            {
                                int WorkerID=AktivniWorkeri[roundRobinIterator];
                                roundRobinIterator++;
                                if(roundRobinIterator>= AktivniWorkeri.Count) { roundRobinIterator = 0; }
                                SendDataToWorker(IDC, WorkerID);
                            }
                        }
                    }
                    else
                    {
                        IDC.DescriptionDataSet.SecondValue = i.Value;
                        if (IDC.DescriptionDataSet.Capacity == EnteredData.empty || IDC.DescriptionDataSet.Capacity == EnteredData.right)
                        {
                            IDC.DescriptionDataSet.Capacity = EnteredData.right;
                        }
                        else
                        {
                            IDC.DescriptionDataSet.Capacity = EnteredData.full;
                            if (AktivniWorkeri.Count > 0)
                            {
                                int WorkerID = AktivniWorkeri[roundRobinIterator];
                                roundRobinIterator++;
                                if (roundRobinIterator >= AktivniWorkeri.Count) { roundRobinIterator = 0; }
                                SendDataToWorker(IDC, WorkerID);
                            }
                        }

                    }
                    IspisiItemDescriptin(ItemsDSC);
                    break;
                }
            }

        }
        public void AddWorker(int i, IPAddress address)
        {
            if (Workers.ContainsKey(i)||i<=0)
            {

                return;
            }
            Workers.Add(i, new Worker.Worker(address, 10000 + i, i));
            AktivniWorkeri.Add(i);
            if (AktivniWorkeri.Count > ItemDSCIndex)
            {
                int uslov = (ItemDSCIndex + 1) * 4;
                if (ItemsDSC.Count < uslov)
                {
                    MakeEmptyItemDescription(1 + ItemDSCIndex * 4);
                }
                ItemDSCIndex++;
            }
            StartWorkerExe(Workers[i]);

        }
        public void RemoveWorker(int i, IPAddress address)
        {
            if (Workers.ContainsKey(i))
            {

                Worker.Worker w = Workers[i];
                IPEndPoint endPoint = new IPEndPoint(w.Address, w.Port);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    socket.Connect(endPoint);
                    NetworkStream stream = new NetworkStream(socket);
                    StreamWriter writer = new StreamWriter(stream) { NewLine = "\r\n", AutoFlush = true };

                    writer.WriteLine("SHUTDOWN");
                    writer.Close();
                    stream.Close();
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    Workers.Remove(i);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }
        public string ispisCuvanjaItemDescriptiona(ItemDescription idc)
        {
            string isp = idc.Id + ";" + idc.DescriptionDataSet.Order + ";"
                + idc.DescriptionDataSet.First + ";" + idc.DescriptionDataSet.Second + ";"
                + idc.DescriptionDataSet.FirstValue + ";" + idc.DescriptionDataSet.SecondValue + ";"
                + idc.DescriptionDataSet.Capacity + ";";
           
            return isp;
        }
        public  void IspisiItemDescriptin(List<ItemDescription> ItemsDSC)
        {
            var path = System.IO.Path.GetDirectoryName(
      System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = path.Substring(6);
            path += "items.txt";
            //path = path.Substring(0,path.Length-6);

            FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(stream);
            foreach (ItemDescription idc in ItemsDSC)
            {
                sw.WriteLine(ispisCuvanjaItemDescriptiona(idc));
            }
            sw.Close();
            stream.Close();

        }
        public List<ItemDescription> UcitajItemDescriptione()
        {
            List<ItemDescription> items = new List<ItemDescription>();
            var path = System.IO.Path.GetDirectoryName(
      System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = path.Substring(6);
            path += "items.txt";
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                if (tokens[0].Equals("")) { break; } ;
                ItemDescription idc = new ItemDescription(Int32.Parse(tokens[0]));
                idc.DescriptionDataSet.Order = Int32.Parse(tokens[1]);
                idc.DescriptionDataSet.First = (Codes)Enum.Parse(typeof(Codes), tokens[2]);
                idc.DescriptionDataSet.Second = (Codes)Enum.Parse(typeof(Codes), tokens[3]);
                idc.DescriptionDataSet.FirstValue = Double.Parse(tokens[4]);
                idc.DescriptionDataSet.SecondValue = Double.Parse(tokens[5]);
                idc.DescriptionDataSet.Capacity = (EnteredData)Enum.Parse(typeof(EnteredData), tokens[6]);

                items.Add(idc);
            }
            sr.Close();
            stream.Close();

            return items;
        }

        public List<int> retriveActiveWorkers()
        {
            return AktivniWorkeri;
        }
    }
}
