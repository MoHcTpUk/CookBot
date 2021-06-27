using Core.Module.MongoDb.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookBot.DAL.Entities
{
    [Table("Polls")]
    public class PollEntity : MongoDBAbstractEntity
    {
        public int MessageId { get; set; }
        public bool isClosed { get; set; }
    }
}