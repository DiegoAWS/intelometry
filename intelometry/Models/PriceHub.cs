namespace intelometry.Models
{
    public class PriceHub
    {
        public int PriceHub_id { get; set; }
        public string PriceHubName { get; set; }

        public PriceHub()
        {
            this.PriceHub_id = -1;
            this.PriceHubName = "";
        }
    }
}
