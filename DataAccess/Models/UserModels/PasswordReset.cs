using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.UserModels;

public class PasswordReset : Entity
{
    [Required]
    public User User { get; set; }

    [Required]
    public int VerificationCode { get; set; }

    [Required]
    public DateTime ValidTillDate { get; set; } = DateTime.Now.AddMinutes(15);
}
