namespace DataAccess.Models.FormsModels;

using DataAccess.Enumerators;
using DataAccess.Models.SportsClubModels;
using DataAccess.Models.UserModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TrainerInvite")]
public class TrainerInvite : Entity
{
    [Required]
    public SportsClub InvitedBy { get; set; }

    [Required]
    public User Trainer { get; set; }

    [Required]
    public OfferStatus Status { get; set; }

    [Required]
    public DateTime Date { get; set; }
}
