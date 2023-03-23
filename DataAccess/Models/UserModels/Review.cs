using DataAccess.Models.SportsClubModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.UserModels;

[Table("Review")]
public class Review : Entity
{
    [Required]
    public User CreatedBy { get; set; }

    public SportsClub? SportsClub { get; set; }

    public User? Trainer { get; set; }

    [Required]
    public int Rating { get; set; }

    [StringLength(100)]
    public string? ReviewText { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }
}
