using System;
namespace intelometry.Models
{
    public class ElectricityMarket
    {
        public string PriceHubName { get; set; }
        public DateTime Tradedate { get; set; }
        public DateTime Deliverystartdate { get; set; }
        public DateTime Deliveryenddate { get; set; }
        public string HighpriceMWh { get; set; }
        public string LowpriceMWh { get; set; }
        public string WtdavgpriceMWh { get; set; }
        public string Change { get; set; }
        public string DailyvolumeMWh { get; set; } //
    }
}
