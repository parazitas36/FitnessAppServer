using DataAccess.Enumerators;
using DataAccess.Models.TrainingPlanModels;
using DataAccess.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.ChatModels;

[Table("Message")]
public class Message : Entity
{
    [Required]
    public Chat Chat { get; set; }

    [Required]
    public User SentBy { get; set; }

    [Required]
    public DateTime SentDate { get; set; } = DateTime.Now;

    public ExerciseProgress? ExerciseProgress { get; set; }

    [Required]
    [StringLength(300)]
    public string Text { get; set; }

    [Required]
    public MessageType MessageType { get; set; }
}
