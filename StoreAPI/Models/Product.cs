namespace StoreAPI.Models;

public class Product
{
    public int ProductID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; } = 0;

    public ICollection<Cart> Carts { get; set; } = new List<Cart>();
}