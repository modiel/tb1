
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
            var admin = svc.AddUser(
                new User { FirstName = "Admin", LastName = "", Email = "admin@mail.com", Password =  "admin", Role =  Role.Admin }
            );
            var tutor = svc.AddUser(
                new User { FirstName = "Tutor", LastName = "", Email = "tutor@mail.com", Password =  "tutor", Role =  Role.Tutor }
            );
            var parent = svc.AddUser(
                new User { FirstName = "Parent", LastName = "", Email = "parent@mail.com", Password =  "parent", Role =  Role.Parent }
            );  

            var tutor1 = svc.AddUser(
                new User { FirstName = "Tutor1", LastName = "", Email = "tutor1@mail.com", Password =  "tutor1", Role =  Role.Tutor }
            );
          
            //add students
            var s1 = new Student
            {   
                User = new User { FirstName = "Pupil1", LastName = "", Email = "pupil1@mail.com", Password =  "pupil1", 
                                  Role =  Role.Pupil, Dob = new DateTime(2010,1,1) 
                                },
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
            var student1 = svc.AddStudent(s1); 

            var s2 = new Student
            {   
                User = new User { FirstName = "Pupil2", LastName = "", Email = "pupil2@mail.com", Password =  "pupil2", 
                                  Role =  Role.Pupil, Dob = new DateTime(2000,1,1) 
                                },
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
            var student2 = svc.AddStudent(s2); 

            var s3 = new Student
            {   
                User= new User { FirstName = "Pupil3", LastName = "", Email = "pupil3@mail.com", Password =  "pupil3", 
                                 Role =  Role.Pupil, Dob = new DateTime(1990,4,1) 
                                },
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
            var student3 = svc.AddStudent(s3); 
            
            var s4 = new Student
            {   
                User = new User { FirstName = "Pupil4", LastName = "", Email = "pupil4@mail.com", Password =  "pupil4", 
                                  Role =  Role.Pupil, Dob = new DateTime(1995,10,1) 
                                }, 
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
            var student4 = svc.AddStudent(s4); 
              
            var s5 = new Student
            {   
                User = new User { FirstName = "Pupil5", LastName = "", Email = "pupil5@mail.com", Password =  "pupil5", 
                                  Role =  Role.Pupil, Dob = new DateTime(1989,3,1) 
                                },
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
            var student5 = svc.AddStudent(s5); 

            // add students to tutor student list
            svc.AssignUserToStudent(tutor.Id, student1.Id);
            svc.AssignUserToStudent(tutor.Id, student2.Id);
            svc.AssignUserToStudent(tutor.Id, student3.Id);
            svc.AssignUserToStudent(tutor.Id, student4.Id); 
            svc.AssignUserToStudent(tutor.Id, student5.Id);    

            // tutor 1 has no students

            
            // add students 3 and 4 to parent student list
            svc.AssignUserToStudent(parent.Id, student3.Id);
            svc.AssignUserToStudent(parent.Id, student4.Id);

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
