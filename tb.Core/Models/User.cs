using System;
namespace tb.Core.Models
{
    // Adds User roles relevant to application
    public enum Role { Admin, Tutor, AdultStudent, Parent, Pupil }
    
    public class User
    {
        public int Id { get; set; }

        // EF Dependant Relationship- Name belongs
        // to a User and should be same as Contact name in Student
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        // User role within application
        public Role Role { get; set; }

        // // EF Dependant Relationship Student belongs to a User
        // public int StudentId { get; set; }

        // Navigation property
        public Student Student { get; set; }

        //DM - this may be redundant
        public IList<Student> UsersStudents { get; set; } = new List<Student>();


    

    }
}
