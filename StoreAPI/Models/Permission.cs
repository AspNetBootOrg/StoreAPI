using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Models;

public class Permission
{
    [Key]
    public int PermissionID { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public string Description { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}