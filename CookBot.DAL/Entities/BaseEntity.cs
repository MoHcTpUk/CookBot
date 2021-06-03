using Core.DAL.Entities;
using System;

namespace CookBot.DAL.Entities
{
    public abstract class BaseEntity : AbstractEntity
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool isDeleted { get; set; }
    }
}
