
using tb.Core.Models;
using tb.Core.Services;

namespace tb.Data.Services
{
    public static class Seeder
    {
        // use this class to seed the database with dummy 
        // test data using an IUserService 
         public static void Seed(IUserService svc)
        {
            svc.Initialise();

            // add users 
            svc.AddUser("Admin", "admin@mail.com", "admin", Role.Admin);
            svc.AddUser("Tutor", "tutor@mail.com", "tutor", Role.Tutor);
            svc.AddUser("AdultStudent", "adultstudent@mail.com", "adultstudent", Role.AdultStudent);
            svc.AddUser("Parent", "parent@mail.com", "parent", Role.Parent);  
            svc.AddUser("Pupil", "pupil@mail.com", "pupil", Role.Pupil);

            //add students
            var s1 = new Student
            {   
                FirstName = "Ronald",
                LastName = "Green",
                ContactName = "",
                Email = "rgreen@email.com",
                Phone = "000000000000",
                AltPhone = "010010101010",
                AddressLineOne = "1 Green Way",
                AddressLineTwo = "Greening Street",
                AddressLineThree = "",
                Postcode = "GR11 1RE",
                Age = 62,
                Dob = "01/01/1960",
                Gender = Gender.Male,
                Allergies = "None",
                AdditionalNeeds = "None",
                InstrumentOne = "Piano",
                InstrumentTwo = "",
                CurrentGradeInstOne = 1,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 0,
                Aurals = Aurals.No,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = Core.Models.DaysOfWeek.Monday,
                LessonTwoDay = Core.Models.DaysOfWeek.NA
            };
            svc.AddStudent(s1); 

            var s2 = new Student
            {   
                FirstName = "Missy",
                LastName = "Scarlet",
                ContactName = "",
                Email = "mpeacock@email.com",
                Phone = "2222222222",
                AltPhone = "020 20202020",
                AddressLineOne = "22 Peacock Street",
                AddressLineTwo = "Belfast",
                AddressLineThree = "",
                Postcode = "BT00 2PE",
                Age = 12,
                Dob = "02/02/2010",
                Gender = Gender.Female,
                Allergies = "Tree nuts",
                AdditionalNeeds = "ADD - mild but check if meds have been taken",
                InstrumentOne = "Piano",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.No,
                LessonFormat = LessonFormat.Hybrid,
                LessonOneDay = Core.Models.DaysOfWeek.Monday,
                LessonTwoDay = Core.Models.DaysOfWeek.NA
            };
            svc.AddStudent(s2); 

            var s3 = new Student
            {   
                FirstName = "Anna",
                LastName = "Arendelle",
                ContactName = "",
                Email = "aarendelle@email.com",
                Phone = "020 303030303",
                AltPhone = "020 20202020",
                AddressLineOne = "1 Castle Avenue",
                AddressLineTwo = "Arendelle",
                AddressLineThree = "",
                Postcode = "BT01 2AA",
                Age = 19,
                Dob = "01/04/2002",
                Gender = Gender.Female,
                Allergies = "",
                AdditionalNeeds = "",
                InstrumentOne = "Flute",
                InstrumentTwo = "",
                CurrentGradeInstOne = 4,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.OnlineOnly,
                LessonOneDay = Core.Models.DaysOfWeek.Monday,
                LessonTwoDay = Core.Models.DaysOfWeek.NA
            };
            svc.AddStudent(s3); 
            
            var s4 = new Student
            {   
                FirstName = "Elsa",
                LastName = "Arendelle",
                ContactName = "",
                Email = "earendelle@email.com",
                Phone = "020 303030303",
                AltPhone = "020 20202020",
                AddressLineOne = "1 Castle Avenue",
                AddressLineTwo = "Arendelle",
                AddressLineThree = "",
                Postcode = "BT01 2AA",
                Age = 22,
                Dob ="01/04/1999",
                Gender = Gender.Female,
                Allergies = "",
                AdditionalNeeds = "Turns things to ice when stressed. The cold never bothered her anyway",
                InstrumentOne = "Piano",
                InstrumentTwo = "Flute",
                CurrentGradeInstOne = 6,
                CurrentGradeInstTwo = 2,
                CurrentTheoryGrade = 5,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = Core.Models.DaysOfWeek.Monday,
                LessonTwoDay = Core.Models.DaysOfWeek.NA
            };
            svc.AddStudent(s4); 
              
            var s5 = new Student
            {   
                FirstName = "Olaf",
                LastName = "Snowman",
                ContactName="Sven Reindeer",
                Email = "sreindeer@email.com",
                Phone = "020 403030303",
                AltPhone = "",
                AddressLineOne = "1 Wood Way",
                AddressLineTwo = "Mountainside",
                AddressLineThree = "",
                Postcode = "BT01 2OS",
                Age = 6,
                Dob = "12/12/2016",
                Gender = Gender.SelfDescribe,
                Allergies = "Sunlight",
                AdditionalNeeds = "Keep temperature low",
                InstrumentOne = "Piano",
                InstrumentTwo = "",
                CurrentGradeInstOne = 0,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 0,
                Aurals =  Aurals.No,
                LessonFormat = LessonFormat.OnlineOnly,
                LessonOneDay = Core.Models.DaysOfWeek.Monday,
                LessonTwoDay = Core.Models.DaysOfWeek.NA
            };
            svc.AddStudent(s5); 

            //=============now seed progress=============
            var pl1 = new ProgressLog
            {
                Progress = "Scales need more work!",
                StudentId = s1.Id,
            };
            svc.AddProgressLog(pl1);

            var pl2 = new ProgressLog
            {
                Progress = "Fur Elise: Bars 1-9 hands together",
                StudentId = s1.Id,
            };
            svc.AddProgressLog(pl2);

            // add queries
            var q1 = svc.CreateQuery(s1.Id, "I've lost my music");
            var q2 = svc.CreateQuery(s1.Id, "I forgot my password.");
            var q3 = svc.CreateQuery(s2.Id, "When is my next lesson?");
            var q4 = svc.CreateQuery(s4.Id, "I'm having difficulty with pedalling. How do I let it go?");
            var q5 = svc.CreateQuery(s5.Id, "I can't do lessons in summer.");
            var q6 = svc.CreateQuery(s5.Id, "I forgot my email address."); 
        }    
    }

}
