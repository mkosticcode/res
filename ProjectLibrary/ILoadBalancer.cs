using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibrary
{
    [ServiceContract]
    public interface ILoadBalancer
    {
        [OperationContract]
        void DoItem(Item it);
        [OperationContract]
        void AddWorker(int workerID);
        [OperationContract]
        void ShutWorker(int workerID);
        [OperationContract]
        List<int> retriveActiveWorkers();
    }
}
