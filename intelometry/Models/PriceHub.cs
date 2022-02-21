namespace intelometry.Models
{
    public class PriceHub
    {
        public int id { get; set; }
        public string PriceHubName { get; set; }

        public PriceHub()
        {
            this.id = -1;
            this.PriceHubName = "";
        }
    }
}
