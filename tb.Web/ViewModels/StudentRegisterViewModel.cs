using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using tb.Core.Models;
namespace tb.Web.ViewModels
{
    public class StudentRegisterViewModel//May delete
    { 
        public int Id { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match. Please try again")]
        public string PasswordConfirm  { get; set; }


        // Navigation property
        public Student Student { get; set; }

        public User User { get; set; }

        
        

    }
}