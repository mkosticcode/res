using LBWorkerLibrary;
using ProjectLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataSet = LBWorkerLibrary.DataSet;

namespace Worker
{
    
    public class Reader
    {
        public Reader() { }
        public void CreateTable()
        {
            string sql = "CREATE TABLE dataset( id_dataset INTEGER NOT NULL,orderr INTEGER ,first INTEGER NOT NULL,second INTEGER NOT NULL,capacity INTEGER NOT NULL,firstValue NUMBER(10, 2) NOT NULL,secondValue NUMBER(10, 2) NOT NULL) ";
            using (IDbConnection connection = ConnectionUtil_Pooling.GetConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteTable()
        {
            string sql = "DROP TABLE dataset CASCADE CONSTRAINTS;";
            using (IDbConnection connection = ConnectionUtil_Pooling.GetConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void Insert(CollectionDescription cd)
        {
            int cod1 =(int) cd.DescriptionDataSet.First;
            int cod2 = (int)cd.DescriptionDataSet.Second;
            int capacity = (int)cd.DescriptionDataSet.Capacity;

            string query = "insert into dataset(id_dataset, orderr, first, second, capacity, firstValue, secondValue) " +
                "values("+cd.Id+"," + cd.DescriptionDataSet.Order + "," + cod1
                + "," + cod2 + "," + capacity
                + "," + cd.DescriptionDataSet.FirstValue + "," + cd.DescriptionDataSet.SecondValue + ")";
            using (IDbConnection connection = ConnectionUtil_Pooling.GetConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }

        }
        public DataSet Select(CollectionDescription cd)
        {

            LBWorkerLibrary.DataSet ds = new DataSet();
            string query = "select * from dataset where id_dataset=" + cd.Id;
            using (IDbConnection connection = ConnectionUtil_Pooling.GetConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Prepare();
                    using (IDataReader reader = command.ExecuteReader())
                    {
                    if(reader.Read())                        
                        {
                            ds.Order = reader.GetInt32(1);
                            ds.First = (Codes)reader.GetInt32(2);
                            ds.Second = (Codes)reader.GetInt32(3);
                            ds.Capacity = (EnteredData)reader.GetInt32(4);
                            ds.FirstValue = (double)reader.GetValue(5);
                            ds.SecondValue = (double)reader.GetValue(6);
                        }
                    }
                }
            }
            return ds;
        }
        public void Update(CollectionDescription cd)
        {
            DataSet ds = new DataSet();
            ds = Select(cd);
            if (ds.Capacity == 0)
            {
                Insert(cd);
            }
            else
            {
                int cod1 = (int)cd.DescriptionDataSet.First;
                int cod2 = (int)cd.DescriptionDataSet.Second;
                int capacity = (int)cd.DescriptionDataSet.Capacity;
                //srediti
                string query = "update dataset set orderr=" + cd.DescriptionDataSet.Order
                    + ", first=" + cod1 + ", second=" + cod2 + ", capacity=" + capacity + ", firstValue="
                    + cd.DescriptionDataSet.FirstValue + ", secondValue="
                    + cd.DescriptionDataSet.SecondValue + " where id_dataset=" + cd.Id;
                 
                using (IDbConnection connection = ConnectionUtil_Pooling.GetConnection())
                {
                    connection.Open();
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        command.Prepare();
                        command.ExecuteNonQuery();
                    }
                }
            }

        }

    }
}
