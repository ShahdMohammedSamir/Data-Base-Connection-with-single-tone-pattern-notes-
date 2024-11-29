using System.Data.SqlClient;
using System.Threading.Channels;

namespace sec
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Visitors S1 = Visitors.GetInStance();
            Visitors S2 = Visitors.GetInStance();
            Visitors S3 = Visitors.GetInStance();
            //test the function of the visitors calculation..
            Console.WriteLine(Visitors.currentVisitors());

            //make the only object of the singletone class
            DataConnection dbConnection = DataConnection.Instance;

            try
            {
                //open the connection with the sql server (data base in general)
                SqlConnection connection = dbConnection.GetConnection();
                //to make sure the connection is opened
                Console.WriteLine("Database connection opened successfully.");
                //write the sql commend that we want to excute in the following block ...
                using (SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM Courses", connection))
                {
                    //ecute the commend from data base and read the result..
                    SqlDataReader reader = command.ExecuteReader();
                    //the loop to show the result becuse the data from data base is retrive in the list (set)
                    //for each raw for this reason we should make the neasted loop
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.WriteLine(reader[i]); 
                        }
                    }
                    //i think it mean stop reading the retreval data
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                //this exception to catch the errors in the connection if some thing wrong occaure 
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                //finally we close the connection to the server of database 
                dbConnection.CloseConnection();
                //check the server closed successfully...
                Console.WriteLine("Database connection closed.");
            }
        }
    }
}
