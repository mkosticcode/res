using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace Worker
{
    public class ConnectionUtil_Pooling : IDisposable
    {
        private static IDbConnection instance = null;
        public static readonly string VM_DATA_SOURCE = "//192.168.8.104:1521/xe";

        public static readonly string USER_ID = "coa";
        public static readonly string PASSWORD = "ftn";
        public static IDbConnection GetConnection()
        {
            if (instance == null || instance.State == System.Data.ConnectionState.Closed)
            {
                OracleConnectionStringBuilder ocsb = new OracleConnectionStringBuilder();
                ocsb.DataSource = VM_DATA_SOURCE;
                ocsb.UserID = USER_ID;
                ocsb.Password = PASSWORD;
                ocsb.Pooling = true;
                ocsb.MinPoolSize = 1;
                ocsb.MaxPoolSize = 10;
                ocsb.IncrPoolSize = 3;
                ocsb.ConnectionLifeTime = 5;
                ocsb.ConnectionTimeout = 5;
                instance = new OracleConnection(ocsb.ConnectionString);

            }
            return instance;
        }

        public void Dispose()
        {
            if (instance != null)
            {
                instance.Close();
                instance.Dispose();
            }

        }
    }
}
