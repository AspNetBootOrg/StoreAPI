using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models;

public class Order
{
    [Key]
    public int OrderID { get; set; }

    public User User { get; set; }

    [Required]
    public DateTime OrderDate { get; set; } = DateTime.Now;
    
    [Required]
    public string Status { get; set; }

    [Required] [Range(1, int.MaxValue)] public int TotalAmount { get; set; } = 1;
}