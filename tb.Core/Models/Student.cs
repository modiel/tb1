using System.ComponentModel.DataAnnotations; // required for validation annotations
using System.Text.Json.Serialization;        // required for custom json serialization options

namespace tb.Core.Models
{
    public enum Aurals { Yes, No  }

    public enum DaysOfWeek {  NA, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }

    public enum LessonFormat {  InPersonOnly, OnlineOnly, Hybrid }
    public class Student //gets and sets properties for all students
    {
        public int Id { get; set; }

        public string Allergies { get; set; }

        public string AdditionalNeeds { get; set; }

        [Required]
        public string InstrumentOne { get; set; }

        public string InstrumentTwo { get; set; }

        [Range(0,8)]
        public int CurrentGradeInstOne { get; set; }

        [Required]
        public int CurrentGradeInstTwo { get; set; }

        [Required]  [Range(0,8)]
        public int CurrentTheoryGrade { get; set; }

        public Aurals Aurals { get; set; }

        [Required]
        public LessonFormat LessonFormat { get; set; }

        [Required]
        public DaysOfWeek LessonOneDay { get; set; }

        public DaysOfWeek LessonTwoDay { get; set; }
        
        // // 1-N relationship        
        public IList<Query> Queries { get; set; } = new List<Query>();

        // 1-N relationship        
        public IList<ProgressLog> ProgressLogs { get; set; } = new List<ProgressLog>();

        // a student will have a user account
        public int UserId { get; set; }
        public User User { get; set; }
               
        // convenience readonly property to access user/student name
        public string Name => $"{User?.Name}";

        // 1-N relationship - a student may be related to many users (parent->student, tutor->student)
        public IList<UserStudent> UserStudents { get; set; } = new List<UserStudent>();

    }
}