namespace DemoSonarApp.Models
{
    public class OrderRequest
    {
        public List<decimal> Prices { get; set; }
        public string CustomerType { get; set; }
        public decimal Discount { get; set; }
    }
}
