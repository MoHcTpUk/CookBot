﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CookBot.DAL.Entities
{
    [Table("Polls")]
    public class PollEntity : BaseEntity
    {
        public int MessageId { get; set; }
        public bool isClosed { get; set; }
    }
}