
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
            // var admin = svc.AddUser(
            //     new User { FirstName = "Admin", LastName = "", Email = "admin@mail.com", Password =  "admin", Role =  Role.Admin }
            // )-DM could use for future if employing an admin
            var tutor = svc.AddUser( "Tutor","McTuition", "", "156132321", "5132513" , "tutor@mail.com", "10 Test Way", "Test Road",
                "Tyrone",  "XXXX YYZ", new System.DateTime(1965,1,1), Gender.Female, "tutor", Role.Tutor);
            var parent = svc.AddUser(
            "Parent","McParent", "", "0209999999", "0208888888" , "parent@mail.com", "11 Test Way", "Test Road",
                "Down",  "XXXX YYZ", new System.DateTime(1975,1,1), Gender.Male, "parent", Role.Parent);
            var parent1 = svc.AddUser(
            "Iduna","Arendelle", "", "0209999999", "0208888888" , "iduna@mail.com", "10 Mountainside", "Mountainside",
                "Norway",  "XXXX YYZ", new System.DateTime(1972,1,1), Gender.Female, "iduna", Role.Parent);
            var parent2 = svc.AddUser(
            "Mrs","Peacock", "", "0209999999", "0208888888" , "peacock@mail.com","12 Test Way", "Test Road",
                "Down",  "XXXX YYZ", new System.DateTime(1979,5,1), Gender.Female, "peacock", Role.Parent);
            
          
            //add students
            var s1 = new Student
            {   
                User = new User { FirstName = "Missy", LastName = "Scarlet", ContactName = "Mrs Peacock", Email = "missy@mail.com", Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", 
                Dob = new DateTime(2010,1,1), Gender = Gender.Female, Password =  "missy",  Role =  Role.Pupil},
                Allergies = "None",
                AdditionalNeeds = "None",
                GeneralNotes = "Needs help with Bass Clef",
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
            var student1 = svc.AddStudent(s1); 

            var s2 = new Student
            {   
                User = new User { FirstName = "Sven", LastName = "Reindeer", ContactName = "Kristof Trollson", Email = "sven@mail.com", Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY",   
                Role =  Role.Pupil, Dob = new DateTime(2008,1,1), Password =  "sven", Gender = Gender.Male 
                },
                Allergies = "Tree nuts",
                AdditionalNeeds = "ADD - mild but check if meds have been taken",
                GeneralNotes = "Wants to do GCSE music",
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
            var student2 = svc.AddStudent(s2); 

            var s3 = new Student
            {   
                User= new User { FirstName = "Hans", LastName = "Isles", ContactName = "" , Email = "hans@mail.com",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Role =  Role.Pupil, Dob = new DateTime(1990,4,1), Password =  "hans", Gender = Gender.Male
                },
                Allergies = "",
                AdditionalNeeds = "",
                GeneralNotes = "Wants to do A Level music",
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
            var student3 = svc.AddStudent(s3); 
            
            var s4 = new Student
            {   
                User = new User { FirstName = "Elsa", LastName = "Arendelle", Email = "elsa@mail.com", Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Password = "elsa", Role =  Role.Pupil, Dob = new DateTime(1995,10,1), Gender = Gender.Female 
                                }, 
                Allergies = "",
                AdditionalNeeds = "Turns things to ice when stressed. The cold never bothered her anyway",
                GeneralNotes = "Learns by ear, work on sight reading.",
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
            var student4 = svc.AddStudent(s4); 
              
            var s5 = new Student
            {   
                User = new User { FirstName = "Olaf", LastName = "Arendelle", Email = "olaf@mail.com", Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY",   Password =  "olaf", Role =  Role.Pupil, Dob = new DateTime(1989,3,1), Gender = Gender.Male  
                },
                Allergies = "Sunlight",
                AdditionalNeeds = "Keep temperature low",
                GeneralNotes = "Doesn't have a good ear. Or any ears, for that matter.",
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
            var student5 = svc.AddStudent(s5); 

            var s6 = new Student
            {   
                User = new User { FirstName = "Anna", LastName = "Arendelle", ContactName = "Iduna Arendelle", Email = "anna@mail.com",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY",  Password =  "anna", 
                Role =  Role.Pupil, Dob = new DateTime(1995,10,1), Gender = Gender.Female  
                }, 
                Allergies = "",
                AdditionalNeeds = "",
                GeneralNotes = "Learns by ear, work on sight reading.",
                InstrumentOne = "Flute",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 2,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = Core.Models.DaysOfWeek.Monday,
                LessonTwoDay = Core.Models.DaysOfWeek.NA
            };
            var student6 = svc.AddStudent(s6); 

            // add students to tutor student list- not necessary in development stage but can be used to filter if using multiple tutors
            svc.AssignUserToStudent(tutor.Id, student1.Id);
            svc.AssignUserToStudent(tutor.Id, student2.Id);
            svc.AssignUserToStudent(tutor.Id, student3.Id);
            svc.AssignUserToStudent(tutor.Id, student4.Id); 
            svc.AssignUserToStudent(tutor.Id, student5.Id);
            svc.AssignUserToStudent(tutor.Id, student6.Id);      
   
            // add students 4 and 6 to parent1 student list 
            svc.AssignUserToStudent(parent1.Id, student4.Id);
            svc.AssignUserToStudent(parent1.Id, student6.Id);
            
            // add students 1 parent2 student list 
            svc.AssignUserToStudent(parent2.Id, student1.Id);

            //=============now seed progress=============
            var pl1 = new ProgressLog
            {
                Progress = "Scales need more work!",
                StudentId = student1.Id,
            };
            svc.AddProgressLog(pl1);

            var pl2 = new ProgressLog
            {
                Progress = "Fur Elise: Bars 1-9 hands together",
                StudentId = student1.Id,
            };
            svc.AddProgressLog(pl2);

            // add queries
            
            var q1 = svc.CreateQuery(student1.Id, "I've lost my music");
            var q2 = svc.CreateQuery(student1.Id, "I forgot my password.");
            var q3 = svc.CreateQuery(student2.Id, "When is my next lesson?");
            var q4 = svc.CreateQuery(student4.Id, "I'm having difficulty with pedalling. How do I let it go?");
            var q5 = svc.CreateQuery(student5.Id, "I can't do lessons in summer.");
            var q6 = svc.CreateQuery(student5.Id, "I forgot my email address.");


        }    
    }

}
