using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAPI.Models;

public class User
{
    [Key]
    public int UserID { get; set; }

    [Required] [StringLength(100)] public string Username { get; set; }

    [Required]
    [StringLength(255)]
    [EmailAddress]
    public string Email { get; set; }

    [Required] public string PasswordHash { get; set; }

    [Required] public DateTime RegistrationDate { get; set; } = DateTime.Now;
    
    public DateTime? LastLogin { get; set; }

    public ICollection<Group> Groups { get; set; } = new List<Group>();
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public Cart? Cart { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}