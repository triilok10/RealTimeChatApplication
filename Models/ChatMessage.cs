namespace RealTimeChatApplication.Models
{
    public class ChatMessage
    {
        public int? ChatMessageID { get; set; } = 0;
        public int? ChatReceiverID { get; set; } = 0;
        public string? SearchConnection { get; set; } = "";
        public string? Email { get; set; } = "";
        public string? UserName { get; set; } = "";
        public string? FullName { get; set; } = "";
        public string? ProfilePictureURL { get; set; } = "";
        public string? ChatMessageData { get; set; } = "";
        public DateTime? TimeStamp { get; set; }
        public bool? IsRequestAccepted { get; set; } = false;
        public int? RequestID { get; set; } = 0;
        public int? AcceptID { get; set; } = 0;
        public int? ChatUserID { get; set; } = 0;
        public IFormFile? ChatPhoto { get; set; } 
        public GenderType? Gender { get; set; }
        public enum GenderType
        {
            Male,
            Female,
            Other
        }
    }
}
