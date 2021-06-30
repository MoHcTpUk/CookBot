using System.Collections.Generic;
using Core.Module.MongoDb.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookBot.DAL.Entities
{
    [Table("Polls")]
    public class PollEntity : MongoDBAbstractEntity
    {
        public int MessageId { get; set; }
        public bool isClosed { get; set; }
        public string PollId { get; set; }
        public List<UserEntity> VotedYes { get; set; }
        public List<UserEntity> VotedNo { get; set; }
    }

    public class UserEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}