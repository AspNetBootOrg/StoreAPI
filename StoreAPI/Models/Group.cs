using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models;

public class Group
{
    [Key]
    public int GroupID { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public ICollection<User> Users { get; set; }
}