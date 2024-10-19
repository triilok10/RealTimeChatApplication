namespace RealTimeChatApplication.Models
{
    public class ChatMessage
    {
        public int? ChatMessageID { get; set; }
        public int? ChatReceiverID { get; set; }
        public string? SearchConnection { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? ProfilePictureURL { get; set; }
    }
}
