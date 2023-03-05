using DataAccess.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.ChatModels;

[Table("Chat")]
public class Chat : Entity
{
    [Required]
    public User User1 { get; set; }

    [Required] 
    public User User2 { get;set; }
}
