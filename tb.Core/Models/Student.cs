using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; // required for validation annotations
using System.Text.Json.Serialization;        // required for custom json serialization options


namespace tb.Core.Models
{
    public enum Gender
    {
        Male, Female, SelfDescribe
    }

    public enum Aurals
    {
        Yes, No
    }

    public enum DaysOfWeek
    {
       NA, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
    }

    public enum LessonFormat
    { 
         InPersonOnly, OnlineOnly, Hybrid, 
    }
    public class Student //gets and sets properties for all students
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string ContactName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [StringLength(20)]
        public string AltPhone { get; set; }
        
        [Required]
        public string  AddressLineOne { get; set; }
        public string  AddressLineTwo { get; set; }
        public string  AddressLineThree { get; set; }

        [Required]
        public string  Postcode { get; set; }
        
        [Range(5,90)]
        public int Age { get; set; } 

        [Required]
        public string Dob { get; set; }

        [Required]
        public Gender Gender  { get; set; }

        public string Allergies { get; set; }

        public string AdditionalNeeds { get; set; }

        [Required]
        public string InstrumentOne { get; set; }

        public string InstrumentTwo { get; set; }

        [Range(0,8)]
        public int CurrentGradeInstOne { get; set; }

        [Required]
        public int CurrentGradeInstTwo { get; set; }

        [Required]    
        [Range(0,8)]
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
    }
}