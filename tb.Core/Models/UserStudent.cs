using System.ComponentModel.DataAnnotations; // required for validation annotations
using System.Text.Json.Serialization;        // required for custom json serialization options

namespace tb.Core.Models
{
    // user/student relationship (students can be associated with parent or tutor users)
    public class UserStudent 
    {
        public int Id { get; set; }

        public int UserId {get; set;}
        public User User {get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}