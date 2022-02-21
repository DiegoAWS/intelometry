using System;
using intelometry.DB;
using intelometry.Models;
using System.Data.SqlClient;
using System.Collections.Generic;


namespace intelometry.Repositories
{
    public class ElectricityMarketRepository
    {
        PriceHubRepository _priceHubRepository;
        ConnectionToMSSQL _connectionToMSSQL;

        public ElectricityMarketRepository(PriceHubRepository priceHubRepository, ConnectionToMSSQL connectionToMSSQL)
        {
            _priceHubRepository = priceHubRepository;
            _connectionToMSSQL = connectionToMSSQL;
        }

        static string WriteFilters(List<Filter> filters, bool operatorOr = false)
        {
            if (filters.Count > 0)
            {

                List<string> filterText = new List<string>();

                foreach (Filter filter in filters)
                {

                    filterText.Add($"{filter.Field} {filter.Comparator} {filter.Value}");
                }

                string op = operatorOr ? "OR" : "AND";

                return string.Join($" {op} ", filterText);
            }

            return "";
        }

        public List<ElectricityMarket> ListAll()
        {
            List<Filter> emptyFilter = new List<Filter>();

            return ListAll(emptyFilter);
        }

        public List<ElectricityMarket> ListAll(List<Filter> filtersPriceHub)
        {
            List<Filter> emptyFilter = new List<Filter>();

            return ListAll(filtersPriceHub, emptyFilter);
        }

        public List<ElectricityMarket> ListAll(List<Filter> filtersPriceHub, List<Filter> filtersDate)
        {
            List<ElectricityMarket> response = new List<ElectricityMarket>();


            string query = "SELECT * FROM electricity_market_table " +
              "JOIN price_hub_table " +
              "ON electricity_market_table.PriceHub_id = price_hub_table.id ";

            if (filtersPriceHub.Count > 0 || filtersDate.Count > 0)
            {
                query += " WHERE ";
            }

            if (filtersPriceHub.Count > 0)
            {
                string filtersPriceHubString =WriteFilters(filtersPriceHub, true);
                query += $"({filtersPriceHubString})";
            }
            if (filtersDate.Count > 0)
            {
                string filterDateString= WriteFilters(filtersDate);

                if(filtersPriceHub.Count>0)
                {
                    query += $" AND ({filterDateString})";
                }
                else
                {
                    query += $"({filterDateString})";
                }
            }

            query += " ORDER BY Deliverystartdate";

            using (SqlDataReader reader = _connectionToMSSQL.ExecuteReader(query))
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

            string query = "INSERT INTO electricity_market_table VALUES(" +
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


                int rowAffected = _connectionToMSSQL.ExecuteNonQuery(query, parameters.ToArray());


                insertedRows += rowAffected;
            }

            return insertedRows;
        }

        public int DeleteAll()
        {
            string query = "DELETE FROM electricity_market_table"; // Very nice statement :)


            return _connectionToMSSQL.ExecuteNonQuery(query);
        }
    }
}
