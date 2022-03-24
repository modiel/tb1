using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using tb.Core.Models;
namespace tb.Web.ViewModels
{
    public class UserRegisterViewModel
    { 
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
 
        [Required]
        [EmailAddress]
        [Remote(action: "VerifyEmailAvailable", controller: "User")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match. Please try again")]
        public string PasswordConfirm  { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; }

    }
}