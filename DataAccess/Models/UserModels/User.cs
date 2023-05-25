using DataAccess.Enumerators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.UserModels;

[Table("User")]
public class User : Entity
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    [Required]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [StringLength(100)]
    public string Password { get; set; }

    [Required]
    public Roles Role { get; set; }

    public ContactInfo? ContactInfo { get; set; }

    [Required]
    public bool UsesImperialSystem { get; set; } = false;

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [StringLength(50)]
    public string Surname { get; set; }

    [StringLength(50)]
    public string? ProfilePictureURI { get; set; }
}
