using System;
using intelometry.DB;
using intelometry.Models;
using System.Data.SqlClient;
using System.Collections.Generic;


namespace intelometry.Repositories
{
    public class ElectricityMarketRepository
    {
        PriceHubRepository  _priceHubRepository;


        public ElectricityMarketRepository(PriceHubRepository priceHubRepository)
        {
            _priceHubRepository = priceHubRepository;
        }


        public List<ElectricityMarket> ListAll(params Filter[] filters)
        {
            List<ElectricityMarket> response = new List<ElectricityMarket>();


            string query = "SELECT * FROM electricity_market_table";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (filters.Length > 0)
            {
                query += " WHERE ";

                List<string> filterText = new List<string>();

                for (int i = 0; i < filters.Length; i++)
                {
                    Filter filter = filters[i];

                    filterText.Add($"@field{i} @comparator{1} @value{i}");

                    SqlParameter fieldParam = new SqlParameter($"field{i}", filter.Field);
                    parameters.Add(fieldParam);

                    SqlParameter comparatorParam = new SqlParameter($"comparator{i}", filter.Comparator);
                    parameters.Add(comparatorParam);

                    SqlParameter valueParam = new SqlParameter($"value{i}", filter.Value);
                    parameters.Add(valueParam);
                }

                query += string.Join(" AND ", filterText);
            }

            query += "NATURAL JOIN price_hub_table ";

            Console.WriteLine($"\"{query}\"");

            using (SqlDataReader reader = ConnectionToMSSQL.ExecuteReader(query, parameters.ToArray()))
            {

                while (reader.Read())
                {
                    ElectricityMarket item = new ElectricityMarket();

                    item.PriceHubName = Convert.ToString(reader["PriceHubName"]);
                    item.Tradedate = Convert.ToDateTime(reader["Tradedate"]);
                    item.Deliverystartdate = Convert.ToDateTime(reader["Deliverystartdate"]);
                    item.Deliveryenddate = Convert.ToDateTime(reader["Deliveryenddate"]);
                    item.HighpriceMWh = Convert.ToString(reader["HighpriceMWh"]);
                    item.LowpriceMWh = Convert.ToString(reader["LowpriceMWh"]);
                    item.Change = Convert.ToString(reader["Change"]);
                    item.WtdavgpriceMWh = Convert.ToString(reader["WtdavgpriceMWh"]);
                    item.DailyvolumeMWh = Convert.ToString(reader["DailyvolumeMWh"]);


                    response.Add(item);
                }
            }

            return response;
        }


        public int InsertMany(List<ElectricityMarket> data)
        {
            int insertedRows = 0;

            string query = "INSERT INTO electricity_market_data VALUES(" +
                         "@PriceHub_id," +
                         "@Tradedate," +
                         "@Deliverystartdate," +
                         "@Deliveryenddate," +
                         "@HighpriceMWh," +
                         "@LowpriceMWh," +
                         "@WtdavgpriceMWh," +
                         "@Change," +
                         "@DailyvolumeMWh" +
                         ")";


            foreach (ElectricityMarket item in data)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                int PriceHub_id = _priceHubRepository.GetOrInsert(item.PriceHubName);


                parameters.Add(new SqlParameter("@PriceHub_id", PriceHub_id));

                //SqlParameter tradedate = new SqlParameter();

                parameters.Add(new SqlParameter("@Tradedate", item.Tradedate));
                parameters.Add(new SqlParameter("@Deliverystartdate", item.Deliverystartdate));
                parameters.Add(new SqlParameter("@Deliveryenddate", item.Deliveryenddate));
                parameters.Add(new SqlParameter("@HighpriceMWh", item.HighpriceMWh));
                parameters.Add(new SqlParameter("@LowpriceMWh", item.LowpriceMWh));
                parameters.Add(new SqlParameter("@WtdavgpriceMWh", item.WtdavgpriceMWh));
                parameters.Add(new SqlParameter("@Change", item.Change));
                parameters.Add(new SqlParameter("@DailyvolumeMWh", item.DailyvolumeMWh));


                int rowAffected = ConnectionToMSSQL.ExecuteNonQuery(query, parameters.ToArray());

                insertedRows += rowAffected;
            }

            return insertedRows;
        }

    }
}
