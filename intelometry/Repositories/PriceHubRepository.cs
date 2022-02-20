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

                    item.PriceHub_id = Convert.ToInt32(reader["PriceHub_id"]);
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
                    response.PriceHub_id = Convert.ToInt32(reader["PriceHub_id"]);
                    response.PriceHubName = Convert.ToString(reader["PriceHubName"]);
                }
            }

            return response;
        }

        public PriceHub Insert(string priceHubName)
        {
            PriceHub response = new PriceHub();

            string query = "INSERT INTO price_hub_table VALUES(@PriceHubName)";

            SqlParameter nameParameter = new SqlParameter("@PriceHubName", priceHubName);

            using (SqlDataReader reader = ConnectionToMSSQL.ExecuteReader(query, nameParameter))
            {
                var data = reader.Read();

                if (data)
                {
                    response.PriceHub_id = Convert.ToInt32(reader["PriceHub_id"]);
                    response.PriceHubName = Convert.ToString(reader["PriceHubName"]);
                }
            }

            return response;
        }


        public int GetOrInsert(string priceHubName)
        {

            PriceHub find = FindOneByName(priceHubName);

            if(find.PriceHub_id == -1)
            {
                PriceHub created = Insert(priceHubName);

                return created.PriceHub_id;
            }

            return find.PriceHub_id;
        }
    }
}
