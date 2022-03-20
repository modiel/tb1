
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
        IList<User> GetUserStudents();
        User GetUser(int id);
        User GetUserByEmail(string email);
        bool IsEmailAvailable(string email, int userId);
        User AddUser(string name, string email, string password, Role role);
        User UpdateUser(User user);
        bool DeleteUser(int id);
        User Authenticate(string email, string password);


        // ---------------- Student Management --------------
         IList<Student> GetStudents();
    
        Student GetStudent(int id);
        Student GetStudentByEmail(string email);
        bool IsDuplicateEmail(string email, int studentId);  
        Student AddStudent(Student s);
        Student UpdateStudent(Student u);  
        bool DeleteStudent(int id);
        IList<Student> GetStudentsQuery(Func<Student,bool> q);

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
