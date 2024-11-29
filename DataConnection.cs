using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Threading;

namespace sec
{
   public class DataConnection
    { 
        //the instance of the singletone class 
        private static DataConnection _instance;
        //the string of the data base connection =>the (read only) just for not assign the value to this attribute just can read it not edit and change 
        private static readonly string ConnectionString = "Data Source=SHMS;Initial Catalog=tolearn;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //used to interact with the SQL Server => mean like i make an object of the connection to the sql server in general
        //is an object that represents a connection to a SQL Server database
        // This class is part of the System.Data.SqlClient namespace and is used to
        // establish a connection between a C# application and a SQL Server database
        //It's a placeholder for the connection itself but does not yet establish a connection until you instantiate it 
        private SqlConnection _connection;

     
        // the synchronization obje => this mean we make this object to control the access of the threads that will access to make the instance in the same time 
        // now thwe mechanism of the lock => when the multi threads want to access to create the instance in the the same time
        // because we make the functions async tasks to be in parallel not wait for each task to excute to ecute the other one 
        //the lock macke the same mechanism here like the online payment wallet when we transfer money from the first wallet to anothe
        //we lock the first wallet from anothe transfers until the initial transfar done to not conflict the monet transfer from the same wallet
        // ex:w1 transfer 50 to w2 (w1 has 50 in it ) and in the same time w1 want to transfer to w3 30 if we dont lock the function of the transfer
        // and do the tasks parallel the to transaction will excute in the same time the remender will be in the minus status...
        private static readonly object _lock = new object();

        //the private constractor to create only one instance of the database connection
        //because of the huge data in database we shouldnot create instance of it every time this consume the memory space
        //and reduce the performance by increase the runtime (excution time )
        private DataConnection()
        {
            _connection = new SqlConnection(ConnectionString);
        }

        //the function to can create one instance from the singletone class
        public static DataConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    //it tells the program to acquire a lock on the _lock object.
                    //Only one thread can hold the lock at a time,
                    //ensuring that only one thread can execute the code within the lock block.
                    //It is not inherited or derived from another class but is declared directly
                    //within DataConnection to ensure thread safety when accessing or initializing the singleton instance
                    //lock is key word
                      lock (_lock)
                    {
                        //this ensure to create only onee object if the two threads(tasks) excute in the same time
                        //so they enter from the first if statment
                        if (_instance == null)
                        {
                            _instance = new DataConnection();
                        }
                    }
                }
                return _instance;
            }
        }


        //this to open the connection with the sql server
        // the reason that we open and close the connection withe sql(database)
        // is the database server is continous that should  close it after finish the task in it like the web server  

        //return the connection to the server
        public SqlConnection GetConnection()
        {
            //check the connection state 
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }
            return _connection;
        }

        //this to close the connection with the sql server
        //doenot have the return because it only clse the already opened connection
        public void CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }


    }
}
