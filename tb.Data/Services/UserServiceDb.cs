using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using tb.Core.Models;
using tb.Core.Services;
using tb.Core.Security;
using tb.Data.Repositories;

namespace tb.Data.Services
{
    public class UserServiceDb : IUserService
    {
        private readonly DatabaseContext  ctx;

        public UserServiceDb()
        {
            ctx = new DatabaseContext(); 
        }

        public void Initialise()
        {
           ctx.Initialise(); 
        }

        // ------------------ User Related Operations ------------------------

        // retrieve list of Users
        public IList<User> GetUsers()
        {
            return ctx.Users.ToList();
        }

        // Retrieve User by Id 
        public User GetUser(int id)
        {
            return ctx.Users.FirstOrDefault(s => s.Id == id);
        }

        public User GetUserWithStudents(int id)
        { 
           
            return ctx.Users
                      .Include(u => u.UserStudents)  
                      .ThenInclude(us => us.Student) 
                      .FirstOrDefault(s => s.Id == id);
        }
 // Add a new User checking a User with same email does not exist
        public User AddUser(string fname, string lname, string contactname, string phone, string altphone,string email, string address1, string address2, string address3, string postcode, DateTime dob, Gender gender,  string password, Role role)
        {     
            var existing = GetUserByEmail(email);
            if (existing != null)
            {
                return null;
            } 

            var user = new User
            {            
                FirstName = fname,
                LastName = lname,
                ContactName = contactname,
                Phone = phone,
                AltPhone = altphone,
                Email = email,
                AddressLineOne = address1,
                AddressLineTwo = address2,
                AddressLineThree = address3,
                Postcode = postcode,
                Dob = dob,
                Gender = gender, 
                Password = Hasher.CalculateHash(password), // can hash if required 
                Role = role              
            };
            ctx.Users.Add(user);
            ctx.SaveChanges();
            return user; // return newly added User
        }

        // Add a new User checking a User with same email does not exist - could be used if only adding a particular type of user
        // public User AddUser(User u)- DM may return to this method next iteration
        // {     
        //     var existing = GetUserByEmail(u.Email);
        //     if (existing != null)
        //     {
        //         return null;
        //     } 

        //     var user = new User
        //     {            
        //         FirstName = u.FirstName,
        //         LastName = u.LastName,
        //         ContactName = u.ContactName,
        //         Phone = u.Phone,
        //         AltPhone = u.AltPhone,
        //         AddressLineOne = u.AddressLineOne,
        //         AddressLineTwo = u.AddressLineTwo,
        //         AddressLineThree = u.AddressLineThree,
        //         Dob = u.Dob,
        //         Postcode = u.Postcode,
        //         Gender = u.Gender,
        //         Email = u.Email,
        //         Password = Hasher.CalculateHash(u.Password), // can hash if required 
        //         Role = u.Role              
        //     };
        //     ctx.Users.Add(user);
        //     ctx.SaveChanges();
        //     return user; // return newly added User
        // }

        // Delete the User identified by Id returning true if deleted and false if not found
        // public bool DeleteUser(int id, Student s)
        // {
        //     var user = GetUser(id);
        //     if (user == null)
        //     {
        //         return false;
        //     }
        //      if (user.Role != Role.Pupil)
        //     {
        //         return false; // user must be a pupil
        //     }

            
        //     ctx.Users.Remove(user);
        //     ctx.SaveChanges();
        //     return true;
        // }
          public bool DeleteUser(int id)
        {
            var u = GetUser(id);
            if (u == null)
            {
                return false;
            }

            ctx.Users.Remove(u);
            ctx.SaveChanges();
            return true;
        }

        // Update the User with the details in updated 
        public User UpdateUser(User updated)
        {
            // verify the User exists
            var User = GetUser(updated.Id);
            if (User == null)
            {
                return null;
            }
            // verify email address is registered or available to this user
            if (!IsEmailAvailable(updated.Email, updated.Id))
            {
                return null;
            }

            // update the details of the User retrieved and save
            User.FirstName = updated.FirstName;
            User.LastName = updated.LastName;
            User.ContactName = updated.ContactName;
            User.Phone = updated.Phone;
            User.AltPhone = updated.AltPhone;
            User.AddressLineOne = updated.AddressLineOne;
            User.AddressLineTwo = updated.AddressLineTwo;
            User.AddressLineThree = updated.AddressLineThree;
            User.Postcode = updated.Postcode;
            User.Dob = updated.Dob;
            User.Gender = updated.Gender;
            User.Email = updated.Email;
            User.Password = Hasher.IsHashed(updated.Password) ? updated.Password : Hasher.CalculateHash(updated.Password); // hash only if not already hashed 
            User.Role = updated.Role;       

            ctx.SaveChanges();          
            return User;
        }

        // Find a user with specified email address
        public User GetUserByEmail(string email)
        {
            return ctx.Users.FirstOrDefault(u => u.Email == email);
        }

        // Verify if email is available or registered to specified user
        public bool IsEmailAvailable(string email, int userId)
        {
            var user = GetUserByEmail(email);
            return user == null || user.Id == userId;
        }

        public IList<User> GetUsersQuery(Func<User,bool> q)
        {
            return ctx.Users.Where(q).ToList();
        }

        public User Authenticate(string email, string password)
        {
            // retrieve the user based on the EmailAddress (assumes EmailAddress is unique)
            var user = GetUserByEmail(email);

            // Verify the user exists and Hashed User password matches the password provided
            return (user != null && Hasher.ValidateHash(user.Password, password)) ? user : null;
            //return (user != null && user.Password == password ) ? user: null;
        }

        // ------------------ Student Related Operations ------------------------

        // retrieve list of Students
         public IList<Student> GetStudents()
         {
         return ctx.Students.Include(s => s.User).ToList();//will default to order by Id
         }


        // retrieve list of Students when using an order
        public IList<Student> GetStudents(string order = null)

        { 

            // return the collection as a list
            if (order.ToLower() == "name") //will order alphbetically 
            { 
            return ctx.Students.OrderBy(s => s.User.LastName)
                                .Include(s => s.User)
                                .ToList();
            }
        
            if (order.ToLower() == "inst1") //will order by instrument
            { 
            return ctx.Students.OrderBy(s => s.InstrumentOne ) 
                               
                                .Include(s => s.User).ToList();
            }

            if (order.ToLower() == "age") //will order by date of birth
            { 
            return ctx.Students.OrderBy(s => s.User.Dob)
                                .Include(s => s.User).ToList();
            }

            if (order.ToLower() == "id")//will order by Id
            { 
            return ctx.Students.OrderBy(s => s.Id)
                                .Include(s => s.User).ToList();
            }

            else
            {
            return ctx.Students.Include(s => s.User).ToList();//will default to order by Id
            }
        }


        // Retrieve student by Id 
        public Student GetStudentById(int id)
        {
            return ctx.Students
                    .Include(s => s.User)
                    .Include(s => s.Queries)
                    .Include(s => s.ProgressLogs)
                    .FirstOrDefault(s => s.Id == id);
        }

        public Student GetStudentByUserId(int id)
        {
            // user associated with student must be a Pupil
            return ctx.Students
                    .Include(s => s.User)
                    .Include(s => s.Queries)
                    .Include(s => s.ProgressLogs)
                    .FirstOrDefault(s => s.User.Id == id && s.User.Role == Role.Pupil );
        }

        public IList<Student> GetStudentsForUser(int uId)
        {
            // get students for specified user
            return ctx.UserStudents                 
                    .Where(us => us.UserId == uId)
                    .Include(us => us.Student.User)
                    .Select(us => us.Student)
                    .Distinct()               
                    .ToList(); 
        }
           

        public Student AddStudent(Student s)
        {
            // check user with email does not exist
            var existing = GetUserByEmail(s.User.Email);
            if (existing != null)
            {
                return null;
            }

            // create the student and add the user
            var student = new Student {
                Allergies = s.Allergies,
                AdditionalNeeds = s.AdditionalNeeds,
                GeneralNotes = s.GeneralNotes,
                InstrumentOne = s.InstrumentOne,
                InstrumentTwo = s.InstrumentTwo,
                CurrentGradeInstOne = s.CurrentGradeInstOne,
                CurrentGradeInstTwo = s.CurrentGradeInstTwo,
                CurrentTheoryGrade = s.CurrentTheoryGrade,
                Aurals = s.Aurals,
                LessonFormat = s.LessonFormat,
                LessonOneDay  = s.LessonOneDay,
                LessonTwoDay = s.LessonTwoDay,

                // ideally create a new User from properties in s.User
                User = new User {           
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                    ContactName = s.User.ContactName,
                    Phone = s.User.Phone,
                    AltPhone = s.User.AltPhone,
                    AddressLineOne = s.User.AddressLineOne,
                    AddressLineTwo = s.User.AddressLineTwo,
                    AddressLineThree = s.User.AddressLineThree,
                    Postcode = s.User.Postcode,
                    Dob = s.User.Dob,
                    Gender = s.User.Gender,
                    Email = s.User.Email,
                    Password = Hasher.CalculateHash(s.User.Password), // can hash if required 
                    Role = Role.Pupil
                }
            };
            ctx.Students.Add(student);

            ctx.SaveChanges(); // write to database
            return student; // return newly added student
        }

        public Student AddStudent(User u, Student s)
        {
            // check user exists
            var user = GetUser(u.Id);            
            if (user == null)
            {
                return null; // user account must exist
            } 
            if (user.Role != Role.Pupil)
            {
                return null; // user must be a pupil
            }

            // create the student and add the user
            var student = new Student {
                Allergies = s.Allergies,
                AdditionalNeeds = s.AdditionalNeeds,
                GeneralNotes = s.GeneralNotes,
                InstrumentOne = s.InstrumentOne,
                InstrumentTwo = s.InstrumentTwo,
                CurrentGradeInstOne = s.CurrentGradeInstOne,
                CurrentGradeInstTwo = s.CurrentGradeInstTwo,
                CurrentTheoryGrade = s.CurrentTheoryGrade,
                Aurals = s.Aurals,
                LessonFormat = s.LessonFormat,
                LessonOneDay  = s.LessonOneDay,
                LessonTwoDay = s.LessonTwoDay,
                User = new User {   
                    Id = s.User.Id,
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                    ContactName = s.User.ContactName,
                    Phone = s.User.Phone,
                    AltPhone = s.User.AltPhone,
                    AddressLineOne = s.User.AddressLineOne,
                    AddressLineTwo = s.User.AddressLineTwo,
                    AddressLineThree = s.User.AddressLineThree,
                    Postcode = s.User.Postcode,
                    Dob = s.User.Dob,
                    Gender = s.User.Gender,
                    Email = s.User.Email,
                    Password = Hasher.CalculateHash(s.User.Password), // hash password 
                    Role = Role.Pupil
                }
            };
            ctx.Students.Add(student);

            ctx.SaveChanges(); // write to database
            return student; // return newly added student
        }

        public UserStudent AssignUserToStudent(int uId, int sId)
        {
            var user = GetUser(uId);
            var student = GetStudentById(sId);
            if (user == null || student == null)
            {
                return null;
            }
            var userStudent = new UserStudent { UserId = uId, StudentId = sId };
            ctx.UserStudents.Add(userStudent);
            ctx.SaveChanges();
            return userStudent;
        }

        // Delete the student identified by Id returning true if deleted and false if not found
        public bool DeleteStudent(int id)
        {
            var s = GetStudentById(id);
            if (s == null)
            {
                return false;
            }
            ctx.Students.Remove(s);
            ctx.SaveChanges(); // write to database
            return true;
        }

        public bool DeleteStudent(Student s, User u)
        {
            var student = GetStudentById(s.Id);
                       
            if (student == null)
            {
                return false; // student account must exist
            } 

            var user = GetUser(student.UserId);
    
            ctx.Students.Remove(student);
            ctx.Users.Remove(user);
            ctx.SaveChanges(); // write to database
            return true;
        }

        // Update the student with the details in updated 
        public Student UpdateStudent(Student s)
        {
            // verify the student exists
            var student = GetStudentById(s.Id);
            if (student == null)
            {
                return null;
            }
            // update the details of the student retrieved and save
            student.User.FirstName = s.User.FirstName;
            student.User.LastName = s.User.LastName;
            student.User.ContactName = s.User.ContactName;
            student.User.Phone = s.User.Phone;
            student.User.AltPhone = s.User.AltPhone;
            student.User.AddressLineOne = s.User.AddressLineOne;
            student.User.AddressLineTwo = s.User.AddressLineTwo;
            student.User.AddressLineThree = s.User.AddressLineThree;
            student.User.Postcode = s.User.Postcode;
            student.User.Dob = s.User.Dob;
            student.User.Gender = s.User.Gender;
            student.User.Email = s.User.Email;
            student.User.Role = student.User.Role;
            //student.User.Password = Hasher.IsHashed(s.User.Password) ? s.User.Password : Hasher.CalculateHash(s.User.Password); // hash if not already hashed 
               
            student.Allergies = s.Allergies;
            student.AdditionalNeeds = s.AdditionalNeeds;
            student.GeneralNotes = s.GeneralNotes;
            student.InstrumentOne = s.InstrumentOne;
            student.InstrumentTwo = s.InstrumentTwo;
            student.CurrentGradeInstOne = s.CurrentGradeInstOne;
            student.CurrentGradeInstTwo = s.CurrentGradeInstTwo;
            student.CurrentTheoryGrade = s.CurrentTheoryGrade;
            student.Aurals = s.Aurals;
            student.LessonFormat = s.LessonFormat;
            student.LessonOneDay = s.LessonOneDay;
            student.LessonTwoDay = s.LessonTwoDay;

            ctx.SaveChanges(); // write to database
            return student;
        }

        public IList<Student> GetStudentsQuery(Func<Student,bool> q)
        {
            return ctx.Students
                     .Include(s => s.Queries)
                     .Where(q).ToList();
        }

         // Miscellaneous
        public bool IsDuplicateEmail(string email, int studentId) 
        {
            var existing = GetUserByEmail(email);
            // if a student with email exists and the Id does not match
            // the studentId (if provided), then they cannot use the email
            return existing != null && studentId != existing.Id;           
        }

        // =================== Query Management ===================
        public Query CreateQuery(int studentId, string issue)
        {
            var student = GetStudentById(studentId);
            if (student == null) return null;

            var query = new Query
            {
                // Id created by Database               
                Issue = issue,      
                StudentId = studentId,

                // set by default in model but can override here if required
                CreatedOn = DateTime.Now,
                Active = true,               
            };
            student.Queries.Add(query);
            ctx.SaveChanges(); // write to database
            return query;
        }

        // return query and related student
        public Query GetQuery(int id)
        {
            return ctx.Queries
                     .Include(q => q.Student)
                     .ThenInclude (u => u.User)
                     .FirstOrDefault(q => q.Id == id);
        }

        // Closed the specified query - must exist and not already closed
        public Query CloseQuery(int id, string resolution ) 
        {
            var query = GetQuery(id);
            // if query does not exist or is already closed return null
            if (query == null || !query.Active) return null;
            
            // query exists and is active so close
            query.Active = false;
            
            //sets time of resolution
            query.ResolvedOn = DateTime.Now;
            query.Resolution = resolution;
            
            ctx.SaveChanges(); // write to database
            return query;  // return closed query
        }

        // delete specified query returning true if successful otherwise false
        public bool DeleteQuery(int id)
        {
            // find query
            var query = GetQuery(id);
            if (query == null) return false;
            
            // remove query from student
            var result = query.Student.Queries.Remove(query);
           ctx.SaveChanges();

            return result;  
        }

        // return all Queries and the student generating the query
        public IList<Query> GetAllQueries()
        {
            var Queries =ctx.Queries
                            .Include(q => q.Student)
                            .ThenInclude (s => s.User)
                            .ToList();
            return Queries;
        }

        // get only active Queries and the student generating the query
        public IList<Query> GetOpenQueries()
        {
             return ctx.Queries
                      .Include(q => q.Student)
                      .ThenInclude (s => s.User)
                      .Where(q => q.Active)
                      .ToList();
        } 
        
        public IList<Query> GetQueriesCheck(Func<Query,bool> q)
        {
            return ctx.Queries
                     .Include(s => s.Student)
                     .ThenInclude (s => s.User)
                     .Where(q).ToList();
        }
        
        // perform a search of the queries based on a
        // QueryRange of ALL, OPEN, CLOSED and a search query
        public IList<Query> SearchQueries(QueryRange range, string query) 
        {
            
            if (query == null)
            {
                query = "";
            }
            var r1 = ctx.Queries
                     .Include (t => t.Student)
                     .ThenInclude(s => s.User)
                     .Where(t => t.Student.User.FirstName.ToLower().Contains(query.ToLower()));
            
             var r2 = ctx.Queries
                     .Include(t => t.Student)
                     .ThenInclude(s => s.User)
                     .Where(t => t.Issue.ToLower().Contains(query.ToLower()));
 
            
            //Use union to join both queries  and ToList(0 to execute query
            return r1.Union(r2).Where(t => 
                                    range == QueryRange.OPEN && t.Active ||
                                    range == QueryRange.CLOSED && !t.Active ||
                                    range == QueryRange.ALL).ToList();
        }


        // =================== ProgressLog Management ============   

        public ProgressLog GetProgressLogById(int id)
        {
            return ctx.ProgressLogs
                        .Include(pl => pl.Student)
                        .FirstOrDefault(pl => pl.Id == id);
        }

        public ProgressLog AddProgressLog(ProgressLog pl)
        {   
            //retrieve student from database
            var student = GetStudentById(pl.StudentId);

            // if student does not exist return null
            if (student == null) return null;

            //add progresslog to student
            ctx.ProgressLogs.Add(pl);
            
            //write to database
            ctx.SaveChanges();
            return pl; //return new progresslog to student
        }

         public ProgressLog UpdateProgressLog(ProgressLog pl)
        {
            // verify the progresLog exists
            var progressLog = GetProgressLogById(pl.Id);
           
            if (progressLog == null)
            {
                return null;
            }
            // update the details of the student retrieved and save
            progressLog.CreatedOn = pl.CreatedOn;
            progressLog.Progress = pl.Progress;

            ctx.SaveChanges(); // write to database
            return progressLog;
        }

        public bool DeleteProgressLog(int id)
        {
            // find progress log
            var progresslog = GetProgressLogById(id);
            if (progresslog == null) return false;


            //remove progresslog from student
            ctx.ProgressLogs.Remove(progresslog);
            
            //write to database
            ctx.SaveChanges();

            return true;
        }
   
    }
}