using System;
using System.ComponentModel.DataAnnotations;

namespace tb.Core.Models
{
    // Adds User roles relevant to application
    public enum Role { Admin, Tutor, Parent, Pupil }
    
    public enum Gender { Male, Female, SelfDescribe }

    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        public string ContactName { get; set; } = "";

        [Required] [StringLength(20)]
        public string Phone { get; set; } = "";

        [Required] [StringLength(20)]
        public string AltPhone { get; set; } = "";
        
        [Required]
        public string  AddressLineOne { get; set; } = "";
        public string  AddressLineTwo { get; set; } = "";
        public string  AddressLineThree { get; set; } = "";

        [Required]
        public string  Postcode { get; set; } = "";
        
        [Required]
        public DateTime Dob { get; set; } = DateTime.Now;

    
        [Required]
        public Gender Gender  { get; set; } = Gender.Male;

        // login credentials and role
        [Required] [EmailAddress]
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public Role Role { get; set; } = Role.Pupil;

        // read-only properties
        public string Name => $"{FirstName} {LastName}";
        public int Age => (int)DateTime.Now.Subtract(Dob).TotalDays / 365;
        public bool Adult => Age >= 18;

        // 1-N relationship - a user may be related to many students (parent->student, tutor->student)
        public IList<UserStudent> UserStudents { get; set; } = new List<UserStudent>();
    
    }
}
