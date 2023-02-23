using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LBWorkerLibrary
{
    [ServiceContract]
    public interface IWorker
    {
        [OperationContract]
        void DoWork(ItemDescription itd);
        [OperationContract]
        void ShutWorker();
    }
}
