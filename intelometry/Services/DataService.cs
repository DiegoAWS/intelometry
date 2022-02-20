using System;
using intelometry.Models;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace intelometry.Services
{
    public class DataService
    {
        string connectionstring = @"
  Server=127.0.0.1,1433;
  Database=intelometry_test_db;
  User Id=SA;
  Password=MyPass@word";

        public int InsertData(Sheet sheet)
        {
            int insertedRecords = 0;

            try
            {

                SqlConnection dbConnection = new SqlConnection(connectionstring);



                string query = "INSERT INTO electricity_market_data VALUES(" +
                        "@Pricehub," +
                        "@Tradedate," +
                        "@Deliverystartdate," +
                        "@Deliveryenddate," +
                        "@HighpriceMWh," +
                        "@LowpriceMWh," +
                        "@WtdavgpriceMWh," +
                        "@Change," +
                        "@DailyvolumeMWh" +
                        ")";

                //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;



                foreach (Datum item in sheet.sheet)
                {
                    SqlCommand cmd = new SqlCommand(query);



                    cmd.Parameters.AddWithValue("@Pricehub", item.Pricehub);
                    cmd.Parameters.AddWithValue("@Tradedate", item.Tradedate);
                    cmd.Parameters.AddWithValue("@Deliverystartdate", item.Deliverystartdate);
                    cmd.Parameters.AddWithValue("@Deliveryenddate", item.Deliveryenddate);
                    cmd.Parameters.AddWithValue("@HighpriceMWh", item.HighpriceMWh);
                    cmd.Parameters.AddWithValue("@LowpriceMWh", item.LowpriceMWh);
                    cmd.Parameters.AddWithValue("@WtdavgpriceMWh", item.WtdavgpriceMWh);
                    cmd.Parameters.AddWithValue("@Change", item.Change);
                    cmd.Parameters.AddWithValue("@DailyvolumeMWh", item.DailyvolumeMWh);


                    cmd.Connection = dbConnection;
                    dbConnection.Open();

                    cmd.ExecuteScalar();

                    cmd.Dispose();


                    dbConnection.Close();
                    insertedRecords++;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlError: " + ex.Message);

                return 0;

            }

            return insertedRecords;

        }

        public void DeleteAllRows()
        {
            SqlConnection dbConnection = new SqlConnection(connectionstring);

            string query = "TRUNCATE TABLE electricity_market_data";

            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = dbConnection;
            dbConnection.Open();
            cmd.ExecuteScalar();
            cmd.Dispose();
            dbConnection.Close();

        }

        public Sheet GetList()
        {

            List<Datum> list = new List<Datum>();
            SqlConnection dbConnection = new SqlConnection(connectionstring);

            string query = "SELECT * FROM electricity_market_data";

            SqlCommand cmd = new SqlCommand(query);

            cmd.CommandType = CommandType.Text;
            cmd.Connection = dbConnection;
            dbConnection.Open();

            using (SqlDataReader sdr = cmd.ExecuteReader())
            {

                while (sdr.Read())
                {
                    Datum item = new Datum();

                    item.Pricehub = Convert.ToString(sdr["Pricehub"]);
                    item.Tradedate = Convert.ToString(sdr["Tradedate"]);
                    item.Deliverystartdate = Convert.ToString(sdr["Deliverystartdate"]);
                    item.Deliveryenddate = Convert.ToString(sdr["Deliveryenddate"]);
                    item.HighpriceMWh = Convert.ToString(sdr["HighpriceMWh"]);
                    item.LowpriceMWh = Convert.ToString(sdr["LowpriceMWh"]);
                    item.Change = Convert.ToString(sdr["Change"]);
                    item.WtdavgpriceMWh = Convert.ToString(sdr["WtdavgpriceMWh"]);
                    item.DailyvolumeMWh = Convert.ToString(sdr["DailyvolumeMWh"]);

                    list.Add(item);


                }
            }




            cmd.Dispose();


            dbConnection.Close();
            Sheet sheet = new Sheet();

            sheet.sheet = list;
            return sheet;
        }

    }

}