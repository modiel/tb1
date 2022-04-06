using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using tb.Core.Models;

namespace tb.Web.ViewModels
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public string ContactName { get; set; }

        [Required] [StringLength(20)]
        public string Phone { get; set; } 

        [Required] [StringLength(20)]
        public string AltPhone { get; set; } 
        
        [Required]
        public string  AddressLineOne { get; set; } = string.Empty;
        public string  AddressLineTwo { get; set; } = "";
        public string  AddressLineThree { get; set; } = "";

        [Required]
        public string  Postcode { get; set; } = "";
        
        [Required]
        public DateTime Dob { get; set; } = DateTime.Now;

        [Required]
        public Gender Gender  { get; set; } = Gender.Male;
 
        [Required]
        [EmailAddress]
        [Remote(action: "VerifyEmailAvailable", controller: "User", AdditionalFields = nameof(Id))]
        public string Email { get; set; }

        // read-only properties
        public string Name => $"{FirstName} {LastName}";
        public int Age => (int)DateTime.Now.Subtract(Dob).TotalDays / 365;
        public bool Adult => Age >= 18;

        
        public Role Role { get; set; }

        // Navigation property
        public Student Student { get; set; }

        

    }
}