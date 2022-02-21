using System;
using intelometry.DB;
using intelometry.Models;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace intelometry.Repositories
{
    public class PriceHubRepository
    {

        public List<PriceHub> ListAll()
        {
            List<PriceHub> response = new List<PriceHub>();

            string query = "SELECT * FROM price_hub_table";

            using (SqlDataReader reader = ConnectionToMSSQL.ExecuteReader(query))
            {

                while (reader.Read())
                {
                    PriceHub item = new PriceHub();

                    item.id = Convert.ToInt32(reader["id"]);
                    item.PriceHubName = Convert.ToString(reader["PriceHubName"]);

                    response.Add(item);
                }
            }

            return response;
        }

        public PriceHub FindOneByName(string name)
        {
            PriceHub response = new PriceHub();

            string query = "SELECT * FROM price_hub_table WHERE PriceHubName=@name";

            SqlParameter nameParameter = new SqlParameter("@name", name);

            using (SqlDataReader reader = ConnectionToMSSQL.ExecuteReader(query, nameParameter))
            {
                var data = reader.Read();

                if (data)
                {
                    response.id = Convert.ToInt32(reader["id"]);
                    response.PriceHubName = Convert.ToString(reader["PriceHubName"]);
                }
            }

            return response;
        }

        public int Insert(string priceHubName)
        {
            string query = "INSERT INTO price_hub_table VALUES(@PriceHubName)";

            SqlParameter nameParameter = new SqlParameter("@PriceHubName", priceHubName);

            return ConnectionToMSSQL.ExecuteNonQuery(query, nameParameter);
        }

        public int DeleteAll()
        {
            string query = "DELETE FROM price_hub_table"; // Very nice statement :)


            return ConnectionToMSSQL.ExecuteNonQuery(query);
        }


        public int GetOrInsert(string priceHubName)
        {

            PriceHub find = FindOneByName(priceHubName);

            if (find.id == -1)
            {
                Insert(priceHubName);


                return FindOneByName(priceHubName).id;
            }

            return find.id;
        }
    }
}
