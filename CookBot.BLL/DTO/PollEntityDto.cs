namespace CookBot.BLL.DTO
{
    public record PollEntityDto : AbstractEntityDto
    {
        public int MessageId { get; set; }
    }
}