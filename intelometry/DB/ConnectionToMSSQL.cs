

using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace intelometry.DB
{
    public class ConnectionToMSSQL
    {

        private readonly IConfiguration _configuration;

        public ConnectionToMSSQL(IConfiguration configuration)
        {
            this._configuration = configuration;
        }


        public  SqlDataReader ExecuteReader (string commandText, params SqlParameter[] parameters)
        {

            string connectionString = _configuration.GetConnectionString("UserDatabase");

            SqlConnection conn = new SqlConnection(connectionString);

            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddRange(parameters);

                conn.Open();
              
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
        }

        public  int ExecuteNonQuery( string commandText, params SqlParameter[] parameters)
        {
            string connectionString = _configuration.GetConnectionString("UserDatabase");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
