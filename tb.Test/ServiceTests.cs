
using Xunit;
using tb.Core.Models;
using tb.Core.Services;
using tb.Data.Services;
using System;

namespace tb.Test
{
    public class ServiceTests
    {
        private IUserService service;

        public ServiceTests()
        {
            service = new UserServiceDb();
            service.Initialise();
        }
        // =================  User Tests ==========================
        [Fact]
        public void EmptyDbShouldReturnNoUsers()
        {
            // act
            var users = service.GetUsers();

            // assert
            Assert.Equal(0, users.Count);
        }

        [Fact]
        public void AddingUsersShouldWork()
        {
            // arrange
            service.AddUser( new User { FirstName = "Tutor", Email = "tutor@mail.com", Password = "tutor", Role = Role.Tutor,
                LastName = "YYYYY",  Phone = "xxxxxxxxxxxY",  AltPhone = "yyyyyyyyyyX",  
                AddressLineOne = "10 Test Way", AddressLineTwo = "Test Road",
                AddressLineThree = "", Postcode = "XXXX YYZ", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Male });
            service.AddUser (new User { FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female });



            // act
            var users = service.GetUsers();

            // assert
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public void UpdatingUserShouldWork()
        {
            // arrange
            var user = service.AddUser( new User { FirstName = "Tutor", Email = "tutor@mail.com", Password = "tutor", Role = Role.Tutor,
                LastName = "YYYYY",  Phone = "xxxxxxxxxxxY",  AltPhone = "yyyyyyyyyyX",  
                AddressLineOne = "10 Test Way", AddressLineTwo = "Test Road",
                AddressLineThree = "", Postcode = "XXXX YYZ", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Male });
         

            // act
            user.FirstName = "Tutor1";
            user.Email = "tutor1@mail.com";
            var updatedUser = service.UpdateUser(user);

            // assert
            Assert.Equal("Tutor1", user.FirstName);
            Assert.Equal("tutor1@mail.com", user.Email);
        }

        [Fact]
        public void LoginWithValidCredentialsShouldWork()
        {
            // arrange
            service.AddUser(new User { FirstName = "Tutor", Email = "tutor@mail.com", Password = "tutor", Role = Role.Tutor,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female });
        
            // act            
            var auth = service.Authenticate("tutor@mail.com", "tutor");

            // assert
            Assert.NotNull(auth);

        }

        [Fact]
        public void LoginWithInvalidCredentialsShouldNotWork()
        {
            // arrange
            service.AddUser( new User { FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female, });

            // act      
            var user = service.Authenticate("tutor@mail.com", "xxx");

            // assert
            Assert.Null(user);

        }

        // =================  Student Tests =====================
        [Fact]
        public void Student_GetStudentsWhenNone_ShouldReturnNone()
        {
            //arrange

            //act
            var students = service.GetStudents(); //database should be empty
            var count = students.Count;

            //assert 
            Assert.Equal(0, count);
        }

        [Fact]
        public void Student_GetStudentsWhenStudentAdded_ShouldReturnStudent()
        {
            // arrange
            var pupil =  new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil,
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            service.AddStudent(student);

            //assert student exists and matches attributes
            Assert.NotNull(student);
            Assert.Equal(student.UserId, pupil.Id);
        }


        [Fact]
        public void Student_GetStudentsWhenTwoAdded_ShouldReturnTwo()
        {
            // arrange
            // create dummy student one

            // arrange
            var pupil1 =   new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };
            var pupil2 =  new User { 
                FirstName = "Pupil2", Email = "pupil2@mail.com", Password = "pupil2", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "10 Test Way", AddressLineTwo = "Main Street",
                AddressLineThree = "", Postcode = "ZZZZ YYY", Dob = new System.DateTime(1975,1,1),
                Gender = Gender.Male,
            };

            // create student
            var ns1 = new Student
            {   User = pupil1,
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            var ns2 = new Student
            {   
                User = pupil2,
                Allergies = "Bees",
                AdditionalNeeds = "ADHD",
                InstrumentOne = "yyy",
                InstrumentTwo = "ZZZ",
                CurrentGradeInstOne = 2,
                CurrentGradeInstTwo = 1,
                CurrentTheoryGrade = 2,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Tuesday,
                LessonTwoDay = DaysOfWeek.Thursday

            };

            //add students to service
            var s1 = service.AddStudent( ns1);
            var s2 = service.AddStudent( ns2);

            // act
            //get students and check count
            var students = service.GetStudents();
            var count = students.Count;

            // assert
            //should return two students
            Assert.Equal(2, count);
        }

        [Fact]
        public void Student_AddStudentWhenUnique_ShouldSetAllProperties()
        {
            // arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            student = service.AddStudent(student);
           
            var s = service.GetStudentById(student.Id); // retrieve student saved 

            // assert - that student is not null
            Assert.NotNull(s);
            //assert - that student matches set properties
            Assert.Equal(s.User.Email, pupil.Email);
        }

        [Fact]
        public void Student_AddWhenDuplicateEmail_ShouldReturnNull()
        {
            // arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            var s1 = service.AddStudent(student);

            // act - add duplicate
            var s2 = service.AddStudent( student);

            // assert
            Assert.NotNull(s1);
            Assert.Null(s2);
        }

        [Fact]
        public void Student_DeleteStudentThatExists_ShouldReturnTrue()
        {
            // arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            student = service.AddStudent(student);

            //act
            var deleted = service.DeleteStudent(student.Id);
            var student1 = service.GetStudentById(student.Id); //attempt to get the student

            //asert
            Assert.True(deleted); //delete student should return true
            Assert.Null(student1); // student1 should be null (as does not exist)
        }

        [Fact]
        public void Student_DeleteStudentThatExists_ShouldReduceStudentCountByOne()
        {
            /// arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            student = service.AddStudent(student);

            //act
            var deleted = service.DeleteStudent(student.Id);
            var students = service.GetStudents(); //retrieve list of students to confirm student is deleted

            //assert
            Assert.Equal(0, students.Count); //confirm students count is 0

        }

        [Fact]
        public void Student_DeleteStudentThatDoesntExist_ShouldNotChangeStudentCount()
        {
            // arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            student = service.AddStudent(student);

            // act 	
            service.DeleteStudent(0);               // delete non existent Student
            var Students = service.GetStudents();   // retrieve list of Students

            // assert
            Assert.Equal(1, Students.Count);    // should be 1 Student
        }

        [Fact]
        public void Student_UpdateWhenExists_ShouldSetAllProperties()
        {
            // arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            var ns = service.AddStudent(student);

        
            //update test student

            ns.User.FirstName = "Joe";
            ns.User.LastName = "Bloggs";
            ns.User.Email = "YYY@email.com";
            ns.User.Phone = "yyyyyyyyyyy";
            ns.User.AltPhone = "zzzzzzzzzzz";
            ns.User.AddressLineOne = "1 Update Way";
            ns.User.AddressLineTwo = "Update Street";
            ns.User.AddressLineThree = "";
            ns.User.Postcode = "ZZZZ YYY";
            ns.User.Dob = new DateTime(1965,1,1);
            ns.User.Gender = Gender.SelfDescribe;
            ns.Allergies = "Bees";
            ns.AdditionalNeeds = "ADHD";
            ns.InstrumentOne = "yyy";
            ns.InstrumentTwo = "ZZZ";
            ns.CurrentGradeInstOne = 2;
            ns.CurrentGradeInstTwo = 1;
            ns.CurrentTheoryGrade = 2;
            ns.Aurals = Aurals.Yes;
            ns.LessonFormat = LessonFormat.OnlineOnly;
            ns.LessonOneDay = DaysOfWeek.Thursday;
            ns.LessonTwoDay = DaysOfWeek.Friday;

            //act - overwrite the attributes

            service.UpdateStudent(ns);

            //assert - attributes should match the updated records
            Assert.NotNull(ns);
            Assert.Equal("Joe", ns.User.FirstName);
            Assert.Equal("Bloggs", ns.User.LastName);
            Assert.Equal("YYY@email.com", ns.User.Email);
            Assert.Equal("yyyyyyyyyyy", ns.User.Phone);
            Assert.Equal("zzzzzzzzzzz", ns.User.AltPhone);
            Assert.Equal("1 Update Way", ns.User.AddressLineOne);
            Assert.Equal("Update Street", ns.User.AddressLineTwo);
            Assert.Equal("", ns.User.AddressLineThree);
            Assert.Equal(new DateTime(1965,1,1), ns.User.Dob);
            Assert.Equal(Gender.SelfDescribe, ns.User.Gender);

            Assert.Equal("Bees", ns.Allergies);
            Assert.Equal("ADHD", ns.AdditionalNeeds);
            Assert.Equal("yyy", ns.InstrumentOne);
            Assert.Equal("ZZZ", ns.InstrumentTwo);
            Assert.Equal(2, ns.CurrentGradeInstOne);
            Assert.Equal(1, ns.CurrentGradeInstTwo);
            Assert.Equal(Aurals.Yes, ns.Aurals);
            Assert.Equal(LessonFormat.OnlineOnly, ns.LessonFormat);
            Assert.Equal(2, ns.CurrentGradeInstOne);
            Assert.Equal(2, ns.CurrentGradeInstOne);
            Assert.Equal(DaysOfWeek.Thursday, ns.LessonOneDay);
            Assert.Equal(DaysOfWeek.Friday, ns.LessonTwoDay);

        }

        // =================  Query Tests ==========================

        [Fact] // --- AddQuery should be Active
        public void Query_CreateQueryForExistingStudent_ShouldBeActive()
        {
            // arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            var s = service.AddStudent(student);

            // act
            var q = service.CreateQuery(s.Id, "Dummy Query 1");

            // assert- query should be active
            Assert.True(q.Active);
        }

        [Fact] // --- GetOpenQueries When two added should return two 
        public void Query_GetOpenQueriesWhenTwoAdded_ShouldReturnTwo()
        {
            // arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            var s  = service.AddStudent(student);

            //create two queries
            var t1 = service.CreateQuery(s.Id, "Dummy Query 1");
            var t2 = service.CreateQuery(s.Id, "Dummy Query 2");

            // act
            var open = service.GetOpenQueries();

            // assert- should be two open queries
            Assert.Equal(2, open.Count);
        }

        [Fact]
        public void Query_CloseQueryWhenOpen_ShouldReturnQuery()
        {
            // arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            var s = service.AddStudent(student);

            var t = service.CreateQuery(s.Id, "Dummy Query");

            // act
            var r = service.CloseQuery(t.Id, "Resolved");

            // assert
            Assert.NotNull(r);                          // verify closed query returned          
            Assert.False(r.Active);                     // verify its closed
            Assert.Equal("Resolved", t.Resolution);     // verify the resolution
            Assert.NotEqual(System.DateTime.MinValue, r.ResolvedOn);
        }

        [Fact]
        public void Query_CloseQueryWhenAlreadyClosed_ShouldReturnNull()
        {
            // arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            var s = service.AddStudent(student);

            var t = service.CreateQuery(s.Id, "Dummy Query");

            // act
            var closed = service.CloseQuery(t.Id, "Resolved"); // close active query    
            closed = service.CloseQuery(t.Id, "");                 // close non active query

            // assert
            Assert.Null(closed);                    // no query returned as already closed
        }

        [Fact]
        public void Query_GetAllQuerysWhenOneOpenAndOneClosed_ShouldReturnTwo()
        {
            // arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            var s = service.AddStudent(student);
            
            var t1 = service.CreateQuery(s.Id, "Dummy Query 1");
            var t2 = service.CreateQuery(s.Id, "Dummy Query 2");
            var closed = service.CloseQuery(t1.Id, "Resolved");     // close one query    

            // act
            var queries = service.GetAllQueries();      // get all queries

            // assert
            Assert.Equal(2, queries.Count);
        }

        [Fact]
        public void Ticket_SearchQueriessWhenOneResultAvailableInOpenQueries_ShouldReturnOne()
        {
           // arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            var s = service.AddStudent(student);

            var t1 = service.CreateQuery(s.Id, "Dummy Query 1");
            var t2 = service.CreateQuery(s.Id, "Dummy Query 2");
            var closed = service.CloseQuery(t1.Id, "Resolved");     // close one query    

            // act
            var queries = service.SearchQueries(QueryRange.OPEN, "Dummy");      // search open queries

            // assert
            Assert.Equal(1, queries.Count);
        }

        [Fact]
        public void Query_DeleteQuery_WhenDeleted_ShouldDeleteQuery()
        {
            // arrange
            //create dummy student to add to the service
             var pupil =   new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {
                User = pupil,
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            var s = service.AddStudent(student);


            //create query
            var t1 = service.CreateQuery(s.Id, "Dummy Query 1");

            var deleted = service.DeleteQuery(t1.Id);

            //attempt to retrieve query from database
            service.GetQuery(t1.Id);

            //assert
            Assert.True(deleted);
        }

        [Fact]
        public void Query_DeleteQuery_WhenDeleted_ShouldReduceQueryCountByOne()
        {
            //arrange
            //create dummy student to add to the service
            var pupil =  new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create studente
            var student = new Student
            {   User = pupil,
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            student = service.AddStudent(student);

            //add queries to student
            var query1 = service.CreateQuery(student.Id,"Dummy Query 1" );
            var query2 = service.CreateQuery(student.Id,"Dummy Query 2" );

            //act
            var deleted = service.DeleteQuery(student.Id);
            var queries = service.GetAllQueries(); //retrieve list of students to confirm student is deleted

            //assert
            Assert.Equal(1, queries.Count); //confirm queries count is 1


        }
        // =================  ProgressLog Tests ====================
        [Fact]
        public void ProgressLog_AddProgressLogToStudent_WhenCreated_ShouldReturnProgressLog()
        {
           // arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            var s = service.AddStudent(student);

            //create test progress log and add to database
            var progressLog = new ProgressLog
            {
                Progress = "Practise more",
                StudentId = s.Id
            };

            //act

            //add progress log to student 
            var spl = service.AddProgressLog(progressLog); //check this method 

            //attempt to retrieve progresslog from database
            var spl1 = service.GetProgressLogById(spl.Id);

            //assert
            Assert.NotNull(spl1); //progress log should exist

        }

        [Fact]
        public void ProgressLog_AddProgressLogToStudent_WhenTwoCreated_ShouldReturnTwo()
        {
            // arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            var student1 = service.AddStudent(student);

            //create test progress log and add to database
            var pl1 = new ProgressLog
            {
                Progress = "Practise more",
                StudentId = student1.Id,
            };

            //act
            //add progress log to student 
            var spl1 = service.AddProgressLog(pl1); 
            //add secong log to student
            var pl2 = new ProgressLog
            {
                Progress = "Minor Scales",
                StudentId = student1.Id,
            };
            var spl2 = service.AddProgressLog(pl2);

            //attempt to retrieve progresslogs from database
            var s = service.GetStudentById(student1.Id);
         
            //assert
            Assert.Equal(2, s.ProgressLogs.Count); //2 progress logs should exist
        }

        [Fact]
        public void ProgressLog_UpdateProgressLog_WhenUpdated_ShouldUpdate()
        {
           // arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            var student1 = service.AddStudent(student);

            //create test progress log and add to database
            var pl = new ProgressLog
            {
                Progress = "Practise more",
                StudentId = student1.Id,
            };

            //add progress log to student 
            var spl1 = service.AddProgressLog(pl); 

            //act- overwrite the attributes
            pl.Progress = "Practise for 30 minutes each day";
        
            //act - update progress log
            service.UpdateProgressLog(pl);

            //assert updated
            Assert.Equal("Practise for 30 minutes each day", pl.Progress);
            
        }

        [Fact]
        public void Student_DeleteStudent_WhenDeleted_ShouldAlsoDeleteProgressLog()
        {
            // arrange
            //create dummy student to add to the service
            var pupil = new User { 
                FirstName = "Pupil", Email = "pupil@mail.com", Password = "pupil", Role = Role.Pupil,
                LastName = "XXXX",  Phone = "xxxxxxxxxxxx",  AltPhone = "yyyyyyyyyyy",  
                AddressLineOne = "1 Test Way", AddressLineTwo = "Test Street",
                AddressLineThree = "", Postcode = "XXXX YYY", Dob = new System.DateTime(1965,1,1),
                Gender = Gender.Female,
            };

            // create student
            var student = new Student
            {   
                User = pupil, 
                Allergies = "xxx",
                AdditionalNeeds = "ADD",
                InstrumentOne = "yyy",
                InstrumentTwo = "",
                CurrentGradeInstOne = 3,
                CurrentGradeInstTwo = 0,
                CurrentTheoryGrade = 1,
                Aurals = Aurals.Yes,
                LessonFormat = LessonFormat.InPersonOnly,
                LessonOneDay = DaysOfWeek.Monday,
                LessonTwoDay = DaysOfWeek.NA
            };

            //add student to service
            var student1 = service.AddStudent(student);

            //create test progress log and add to database
            var progressLog = new ProgressLog
            {
                Progress = "Practise more",
                StudentId = student1.Id,
            };
            var spl = service.AddProgressLog(progressLog);

            //act 
            //delete student from database
            var deleted = service.DeleteStudent(student1.Id);

            //attempt to retrieve progress log from database
            var spl1 = service.GetProgressLogById(spl.Id);

            //assert
            Assert.True(deleted);
            Assert.Null(spl1);
        }
    }
}
