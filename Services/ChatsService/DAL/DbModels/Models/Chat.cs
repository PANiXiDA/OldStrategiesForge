using BaseDAL.Models;
using Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatsService.DAL.DbModels.Models;

public class Chat : BaseDbModel<Guid>
{
    [Column("chat_type")]
    [Required]
    public ChatType ChatType { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    public ICollection<Message> Messages { get; set; } = new List<Message>();
}