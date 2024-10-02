using System.ComponentModel.DataAnnotations;

namespace RealTimeChatApplication.Models
{
    public class ChatUser
    {

        public int ChatUsser { get; set; }
        [Required(ErrorMessage = "Please enter the UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter the Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter the Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter the ConfirmPassword")]
        public string ConfirmPassword { get; set; }
        public IFormFile ProfilePictureURL { get; set; }
        public bool Status { get; set; }
        public DateTime AuthenticationTime { get; set; }
    }
}
