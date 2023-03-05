using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.UserModels;

[Table("ContactInfo")]
public class ContactInfo : Entity
{
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [Required]
    [StringLength(50)]
    public string EmailAddress { get; set; }
}
