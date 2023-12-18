namespace Product.API.Models
{
    public class Product
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
