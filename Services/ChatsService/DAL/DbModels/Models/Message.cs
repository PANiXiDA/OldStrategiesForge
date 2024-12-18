﻿using BaseDAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatsService.DAL.DbModels.Models;

public class Message : BaseDbModel<Guid>
{
    [Column("chat_id")]
    [Required]
    public Guid ChatId { get; set; }

    [Column("sender_id")]
    [Required]
    public int SenderId { get; set; }

    [Column("content")]
    [Required]
    public string Content { get; set; } = string.Empty;

    [Column("is_read")]
    [Required]
    public bool IsRead { get; set; } = false;

    public Chat Chat { get; set; } = null!;
}