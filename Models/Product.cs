namespace ProductsAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Price { get; set; }
        public bool IsActive { get; set; }
    }
}