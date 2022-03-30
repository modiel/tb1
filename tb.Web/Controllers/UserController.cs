
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

using tb.Core.Models;
using tb.Core.Services;
using tb.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using tb.Core.Security;

/**
 *  User Management Controller providing registration and login functionality
 */
namespace tb.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly IUserService _svc;

        public UserController(IUserService svc, IConfiguration config)
        {        
            _config = config;    
            _svc = svc;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] UserLoginViewModel m)
        {
            var user = _svc.Authenticate(m.Email, m.Password);
            // check if login was unsuccessful and add validation errors
            if (user == null)
            {
                ModelState.AddModelError("Email", "Invalid Login Credentials");
                ModelState.AddModelError("Password", "Invalid Login Credentials");
                return View(m);
            }

            // Login Successful, so sign user in using cookie authentication
            await SignInCookie(user);

            Alert("Successfully Logged in", AlertType.info);

            return Redirect("/");
        }

        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Admin,Tutor")]
        public IActionResult Register(UserRegisterViewModel m)       
        {
            if (!ModelState.IsValid)
            {
                return View(m);
            }
            // add Tutor user via service
            var user = _svc.AddUser( new User {
                FirstName = m.FirstName, 
                LastName = m.LastName, 
                Email = m.Email,
                Password = m.Password, 
                Role = Role.Tutor
            });
            // check if error adding user and display warning
            if (user == null) {
                Alert("There was a problem Registering. Please try again", AlertType.warning);
                return View(m);
            }

            Alert("Successfully Registered. Now login", AlertType.info);

            return RedirectToAction(nameof(Login));
        }


         // GET /user/index
        [Authorize(Roles="Admin,Tutor")]
        public IActionResult Index()
        {
            var users = _svc.GetUsers();
           
            return View(users);
        }

         // GET /user/details/{id}
        [Authorize]
        public IActionResult UserDetails(int id)
        {
            // retrieve the student with specified id from the service
            var u = _svc.GetUser(id);

            // check if s is null and return NotFound()
            if (u == null)
            {
                Alert("User Not Found", AlertType.warning);
                return RedirectToAction(nameof(Index), new { Id = id });
            }

            // pass user as parameter to the view
            return View(u);
        }

        [Authorize]
        public IActionResult UpdateProfile()
        {
           // use BaseClass helper method to retrieve Id of signed in user 
            var user = _svc.GetUser(GetSignedInUserId());
            var userViewModel = new UserProfileViewModel { 
                Id = user.Id, 
                FirstName = user.FirstName,
                LastName = user.LastName,
                ContactName = user.ContactName,
                Phone = user.Phone,
                AltPhone = user.AltPhone,
                AddressLineOne = user.AddressLineOne,
                AddressLineTwo = user.AddressLineTwo,
                AddressLineThree = user.AddressLineThree,
                Postcode = user.Postcode,
                Dob = user.Dob,
                Gender = user.Gender,
                Email = user.Email,                 
                Role = user.Role
            };
            return View(userViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile([Bind("Id,Name,Email,Role")] UserProfileViewModel m)       
        {
            var user = _svc.GetUser(m.Id);
            // check if form is invalid and redisplay
            if (!ModelState.IsValid || user == null)
            {
                return View(m);
            } 

            // update user details and call service
            user.FirstName = m.FirstName;
            user.Email = m.Email;
            user.Role = m.Role;  
            // add other properties
            user.LastName = m.LastName;
            user.ContactName = m.ContactName;
            user.Phone = m.Phone;
            user.AltPhone = m.AltPhone;
            user.AddressLineOne = m.AddressLineOne;
            user.AddressLineTwo = m.AddressLineTwo;
            user.AddressLineThree = m.AddressLineThree;
            user.Postcode = m.Postcode;
            user.Dob = m.Dob;
            user.Gender = m.Gender;
            user.Email = m.Email;                
            user.Role = m.Role;

            var updated = _svc.UpdateUser(user);

            // check if error updating service
            if (updated == null) {
                Alert("There was a problem Updating. Please try again", AlertType.warning);
                return View(m);
            }

            Alert("Successfully Updated Account Details", AlertType.info);
            
            // sign the user in with updated details)
            await SignInCookie(user);

            return RedirectToAction("Index","Home");
        }

        // Change Password
        [Authorize]
        public IActionResult UpdatePassword()
        {
            // use BaseClass helper method to retrieve Id of signed in user 
            var user = _svc.GetUser(GetSignedInUserId());
            var passwordViewModel = new UserPasswordViewModel { 
                Id = user.Id, 
                Password = user.Password, 
                PasswordConfirm = user.Password, 
            };
            return View(passwordViewModel);
        }
        
        // GET / user/delete/{id}
        [Authorize(Roles="Admin,Tutor")]       
        public IActionResult DeleteUser(int id)
        {
            // load the student using the service
            var u = _svc.GetUser(id);
            // check the returned student is not null and if so return NotFound()
            if (u == null)
            {
                Alert("User Not Found", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }     
            
            // pass user to view for deletion confirmation
            return View(u);
        }

        // POST /user/delete/{id}
        [HttpPost]
        [Authorize(Roles="Admin,Tutor")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUserConfirm(int id, Student s)//int id if needed-DM
        {
            // delete student via service
            _svc.DeleteUser(id, s);
         
            Alert($"User {s.Name} deleted successfully", AlertType.success);
            // redirect to the index view
            return RedirectToAction(nameof(Index), new { Id = id });
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword([Bind("Id,OldPassword,Password,PasswordConfirm")] UserPasswordViewModel m)       
        {
            var user = _svc.GetUser(m.Id);
            if (!ModelState.IsValid || user == null)
            {
                return View(m);
            }  
            // update the password
            user.Password = m.Password; 
            // save changes      
            var updated = _svc.UpdateUser(user);
            if (updated == null) {
                Alert("There was a problem Updating the password. Please try again", AlertType.warning);
                return View(m);
            }

            Alert("Successfully Updated Password", AlertType.info);
            // sign the user in with updated details
            await SignInCookie(user);

            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        // Return not authorised and not authenticated views
        public IActionResult ErrorNotAuthorised() => View();
        public IActionResult ErrorNotAuthenticated() => View();

        // -------------------------- Helper Methods ------------------------------

        // Called by Remote Validation attribute on RegisterViewModel to verify email address is available
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyEmailAvailable(string email, int id)
        {
            // check if email is available, or owned by user with id 
            if (!_svc.IsEmailAvailable(email,id))
            {
                return Json($"A user with this email address {email} already exists.");
            }
            return Json(true);                  
        }

        // Called by Remote Validation attribute on ChangePassword to verify old password
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyPassword(string oldPassword)
        {
            // use BaseClass helper method to retrieve Id of signed in user 
            var id = GetSignedInUserId();            
            // check if email is available, unless already owned by user with id
            var user = _svc.GetUser(id);
            if (user == null || !Hasher.ValidateHash(user.Password, oldPassword))
            {
                return Json($"Please enter current password.");
            }
            return Json(true);                  
        }

        // Sign user in using Cookie authentication scheme
        private async Task SignInCookie(User user)
        {
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                AuthBuilder.BuildClaimsPrincipal(user)
            );
        }
    }
}