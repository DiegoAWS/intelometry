
using System;
using System.Data;
using System.Data.SqlClient;

//using System;
//using intelometry.Models;

//using System.Collections.Generic;

namespace intelometry.DB
{
    public static class ConnectionToMSSQL
    {
        static string connectionString = @"
  Server=127.0.0.1,1433;
  Database=intelometry_test_db;
  User Id=SA;
  Password=MyPass@word";

        // Set the connection, command, and then execute the command with query and return the reader.  
        public static SqlDataReader ExecuteReader (string commandText, params SqlParameter[] parameters)
        {
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

        public static int ExecuteNonQuery( string commandText, params SqlParameter[] parameters)
        {
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
