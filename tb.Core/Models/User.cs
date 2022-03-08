using System;
namespace tb.Core.Models
{
    // Adds User roles relevant to application
    public enum Role { Admin, Tutor, AdultStudent, Parent, Pupil }
    
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        // User role within application
        public Role Role { get; set; }

    }
}
