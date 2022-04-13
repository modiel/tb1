using System.ComponentModel.DataAnnotations;

namespace tb.Web.ViewModels
{
    
    public class UserLoginViewModel
    {       
        [Required]
        [EmailAddress]
        public string Email { get; set; }
 
        [Required]
        public string Password { get; set; }

        public int Id { get; set; }

    }
}