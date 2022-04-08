
using tb.Core.Models;

namespace tb.Core.Services
{
    // This interface describes the operations that a UserService class implementation should provide
    public interface IUserService
    {
        // Initialise the repository - only to be used during development 
        void Initialise();

        // ---------------- User Management --------------
        IList<User> GetUsers();
        User GetUser(int id);
        User GetUserByEmail(string email);
        User GetUserWithStudents(int id);
        bool IsEmailAvailable(string email, int userId);
        // User AddUser(User user);
        User AddUser(string fname, string lname, string contactname, string phone, string altphone,string email, string address1, string address2, string address3, string postcode, DateTime dob, Gender gender,  string password, Role role);

        User UpdateUser(User user);
        // bool DeleteUser(int id, Student s);
        bool DeleteUser(int id);
        User Authenticate(string email, string password);


        // ---------------- Student Management --------------
         IList<Student> GetStudents();
    
        Student GetStudentById(int id);
        Student GetStudentByUserId(int id);
        Student AddStudent(User u, Student s);
        Student AddStudent(Student s);
        Student UpdateStudent(Student u);  
        bool DeleteStudent(int id);
        bool DeleteStudent(Student s, User u);
        IList<Student> GetStudentsForUser(int uId);
        IList<Student> GetStudentsQuery(Func<Student,bool> q);

        UserStudent AssignUserToStudent(int uId, int sId);

        // ---------------- ProgressLog Management --------------
        
        ProgressLog GetProgressLogById(int id);
        ProgressLog AddProgressLog(ProgressLog pl);
        ProgressLog UpdateProgressLog(ProgressLog pl);
        bool DeleteProgressLog(int id);

        // ---------------- Query Management --------------------
        Query CreateQuery(int studentId, string issue);
        Query GetQuery(int id);
        Query CloseQuery(int id, string resolution); 
        bool DeleteQuery(int id);
        IList<Query> GetAllQueries();
        IList<Query> GetOpenQueries();        
        IList<Query> SearchQueries(QueryRange range,string query);
        IList<Query> GetQueriesCheck(Func<Query,bool> q);
     


       
    }
    
}
