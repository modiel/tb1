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
            // var u1 = ctx.Users
            //         .Include ( u => u.UserStudents)
            //         .Where(u => u.Student.ContactName.ToLower() == u.Name);
            // var u2 = ctx.Users
            //         .Include ( u => u.UserStudents)
            //         .Where (u => u.Student.Email.ToLower() == u.Email);

            // return u1.Union(u2)
            // .Include(u => u.UserStudents.ToList())
            // .FirstOrDefault(u => u.Id == id);
        }

        //retrieve student(s) associated with user
        public IList<User> GetUserStudents()
        {
            //Retrieve User by Id
            // var user = GetUser(id);

            // if (user != null)
            // {
            //     return null;
            // } 

            //retrieve students
            

            //if Student ContactName equals User Name || Student email = User Email

            return ctx.UserStudents.ToList();
        }



        // Add a new User checking a User with same email does not exist
        public User AddUser(string name, string email, string password, Role role)
        {     
            var existing = GetUserByEmail(email);
            if (existing != null)
            {
                return null;
            } 

            var user = new User
            {            
                Name = name,
                Email = email,
                Password = Hasher.CalculateHash(password), // can hash if required 
                Role = role              
            };
            ctx.Users.Add(user);
            ctx.SaveChanges();
            return user; // return newly added User
        }

        // Delete the User identified by Id returning true if deleted and false if not found
        public bool DeleteUser(int id)
        {
            var s = GetUser(id);
            if (s == null)
            {
                return false;
            }
            ctx.Users.Remove(s);
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
            User.Name = updated.Name;
            User.Email = updated.Email;
            User.Password = Hasher.CalculateHash(updated.Password);  
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
            return ctx.Users.FirstOrDefault(u => u.Email == email && u.Id != userId) == null;
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
            // return the collection as a list
            return ctx.Students.ToList();
        }


        // Retrieve student by Id 
        public Student GetStudent(int id)
        {
            return ctx.Students
                    .Include(s => s.Queries)
                    .Include(pl => pl.ProgressLogs)
                    .FirstOrDefault(s => s.Id == id);
        }

        // Add a new student checking a student with same email does not exist
        public Student AddStudent(Student s)
        {
            // check if email is already in use by another student
            var existing = GetStudentByEmail(s.Email);
            
            if (existing != null)
            {
                return null; // email in use so we cannot create student
            } 
            // email is unique so create student
            ctx.Students.Add(s);
            ctx.SaveChanges(); // write to database
            return s; // return newly added student
        }

        // Delete the student identified by Id returning true if deleted and false if not found
        public bool DeleteStudent(int id)
        {
            var s = GetStudent(id);
            if (s == null)
            {
                return false;
            }
            ctx.Students.Remove(s);
            ctx.SaveChanges(); // write to database
            return true;
        }

        // Update the student with the details in updated 
        public Student UpdateStudent(Student u)
        {
            // verify the student exists
            var student = GetStudent(u.Id);
            if (student == null)
            {
                return null;
            }
            // update the details of the student retrieved and save
            student.FirstName = u.FirstName;
            student.LastName = u.LastName;
            student.ContactName = u.ContactName;
            student.Email = u.Email;
            student.Phone = u.Phone;
            student.AltPhone = u.AltPhone;
            student.AddressLineOne = u.AddressLineOne;
            student.AddressLineTwo = u.AddressLineTwo;
            student.AddressLineThree = u.AddressLineThree;
            student.Postcode = u.Postcode;
            student.Age = u.Age;
            student.Dob = u.Dob;
            student.Gender = u.Gender;
            student.Allergies = u.Allergies;
            student.AdditionalNeeds = u.AdditionalNeeds;
            student.InstrumentOne = u.InstrumentOne;
            student.InstrumentTwo = u.InstrumentTwo;
            student.CurrentGradeInstOne = u.CurrentGradeInstOne;
            student.CurrentGradeInstTwo = u.CurrentGradeInstTwo;
            student.CurrentTheoryGrade = u.CurrentTheoryGrade;
            student.Aurals = u.Aurals;
            student.LessonFormat = u.LessonFormat;
            student.LessonOneDay = u.LessonOneDay;
            student.LessonTwoDay = u.LessonTwoDay;

            ctx.SaveChanges(); // write to database
            return student;
        }

        public Student GetStudentByEmail(string email)
        {
            return ctx.Students.FirstOrDefault(s => s.Email == email);
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
            var existing = GetStudentByEmail(email);
            // if a student with email exists and the Id does not match
            // the studentId (if provided), then they cannot use the email
            return existing != null && studentId != existing.Id;           
        }

        // =================== Query Management ===================
        public Query CreateQuery(int studentId, string issue)
        {
            var student = GetStudent(studentId);
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
                            .ToList();
            return Queries;
        }

        // get only active Queries and the student generating the query
        public IList<Query> GetOpenQueries()
        {
             return ctx.Queries
                      .Include(t => t.Student)
                      .Where(t => t.Active)
                      .ToList();
        } 
        
        public IList<Query> GetQueriesCheck(Func<Query,bool> q)
        {
            return ctx.Queries
                     .Include(s => s.Student)
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
                     .Where(t => t.Student.FirstName.ToLower().Contains(query.ToLower()));
            
             var r2 = ctx.Queries
                     .Include(t => t.Student)
                     .Where(t => t.Issue.ToLower().Contains(query.ToLower()));
 
            
            //Use union to join both queries  and ToList(0 to execute query
            return r1.Union(r2).Where(t => 
                                    range == QueryRange.OPEN && t.Active ||
                                    range == QueryRange.CLOSED && !t.Active ||
                                    range == QueryRange.ALL).ToList();
            // replace with return search results
            // return new List<Query>(); 
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
            var student = GetStudent(pl.StudentId);

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

        //DIANE- POTENTIAL WAY OF IMPROVING AGE INPUT 
//         public static int GetAge(DateTime birthDate)
// {
//     DateTime n = DateTime.Now; // To avoid a race condition around midnight
//     int age = n.Year - birthDate.Year;

//     if (n.Month < birthDate.Month || (n.Month == birthDate.Month && n.Day < birthDate.Day))
//         age--;

//     return age;
    // }


   
    }
}