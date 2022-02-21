using System;
using intelometry.Models;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using intelometry.Repositories;
using System.Web;

namespace intelometry.Services
{
    public class DataService
    {

        ElectricityMarketRepository _electricityMarketRepository;
        PriceHubRepository _priceHubRepository;

        public DataService(ElectricityMarketRepository electricityMarketRepository, PriceHubRepository priceHubRepository)
        {
            _electricityMarketRepository = electricityMarketRepository;
            _priceHubRepository = priceHubRepository;
        }

        public void StoreNewData(List<Datum> sheet)
        {
            List<ElectricityMarket> transformedData = TransformDataList(sheet);

            _electricityMarketRepository.InsertMany(transformedData);
        }

        public List<PriceHub> ListAllPriceHubs()
        {
            return _priceHubRepository.ListAll();
        }

        public List<ElectricityMarket> ListAllData()
        {
            return _electricityMarketRepository.ListAll();
        }

        public List<ElectricityMarket> ListAllData(List<int> priceHub_ids)
        {
            return ListAllData(priceHub_ids, "", DateTime.Now, DateTime.Now);
        }

        public string formatdateToSQL(DateTime date)
        {
            return $"'{date.ToString("yyyy-MM-dd")}'";
        }
        public List<ElectricityMarket> ListAllData(List<int> priceHub_ids, string filterBy,
            DateTime timeStart, DateTime timeEnd)
        {
            List<Filter> filtersPriceHub = new List<Filter>();

            foreach (var priceHub_id in priceHub_ids)
            {
                filtersPriceHub.Add(new Filter("PriceHub_id", "=", priceHub_id.ToString()));
            }


            List<Filter> filtersDate = new List<Filter>();

            if (filterBy != "")
            {
                filtersDate.Add(new Filter(filterBy, ">=", formatdateToSQL(timeStart)));
                filtersDate.Add(new Filter(filterBy, "<", formatdateToSQL(timeEnd)));
            }
           

            return _electricityMarketRepository.ListAll(filtersPriceHub, filtersDate);
        }

        public ElectricityMarket transformData(Datum item)
        {
            ElectricityMarket transformedItem = new ElectricityMarket();

            transformedItem.PriceHubName = item.Pricehub;
            transformedItem.HighpriceMWh = item.HighpriceMWh;
            transformedItem.LowpriceMWh = item.LowpriceMWh;
            transformedItem.WtdavgpriceMWh = item.WtdavgpriceMWh;
            transformedItem.Change = item.Change;
            transformedItem.DailyvolumeMWh = item.DailyvolumeMWh;


            transformedItem.Tradedate = Convert.ToDateTime(item.Tradedate);
            transformedItem.Deliverystartdate = Convert.ToDateTime(item.Deliverystartdate);
            transformedItem.Deliveryenddate = Convert.ToDateTime(item.Deliveryenddate);

            return transformedItem;
        }

        public List<ElectricityMarket> TransformDataList(List<Datum> data)
        {
            List<ElectricityMarket> transformedDataList = new List<ElectricityMarket>();

            Array.ForEach(data.ToArray(), (item) =>
            {
                ElectricityMarket transformedData = transformData(item);
                transformedDataList.Add(transformedData);
            });

            return transformedDataList;
        }


        public int DeleteAllTheData()
        {

            int deletedElectricityData = _electricityMarketRepository.DeleteAll();
            int deletedPriceHub = _priceHubRepository.DeleteAll();

            return deletedElectricityData + deletedPriceHub;
        }
    }

}