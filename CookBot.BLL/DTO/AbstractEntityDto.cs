using System;

namespace CookBot.BLL.DTO
{
    public abstract record AbstractEntityDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool isDeleted { get; set; }
    }
}
