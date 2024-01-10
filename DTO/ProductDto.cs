namespace ProductsAPI.DTO
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Price { get; set; }
    }
}