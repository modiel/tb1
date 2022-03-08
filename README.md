
DotNet 6 Clean Architecture MVC Solution
========================================

This .NET project `tb` demonstrates concept of separation of concerns (SOC) and is a simple implementation of the Clean Architecture. When installed and used to create a new project, all references to ```tb``` will be replaced with the name of your project.

## Core Project

The Core project contains all domain entities and service layer interfaces and has no dependencies.

Password hashing functionality added via the ```tb.Core.Security.Hasher``` class. This is used in the Data project UserService to hash the user password before storing in database.

## Data Project

The Data project encapsulates all data related concerns. It provides an implementation of ```tb.Core.Services.IUserService``` using EntityFramework to handle data storage/retrieval. It defaults to using Sqlite for portability across platforms.

The Service is the only element exposed from this project and consumers of this project simply need reference it to access its functionalty.

## Test Project

The Test project references the Core and Data projects and should implement unit tests to test any service implementations created in the Data project. A tb test is provided for implementation of IUserService and the tests should be extended to fully exercise the functionality of your Service.

## Web Project

The Web project uses the MVC pattern to implement a web application. It references the Core and Data projects and uses the exposed services and models to access data management functionality. This allows the Web project to be completely independent of the persistence framework used in the Data project.

### Identity

The project provides extension methods to enable:

1. User Identity using cookie authentication is enabled without using the boilerplate tb used in the standard web projects (mvc,web). This allows the developer to gain a better appreciation of how Identity is implemented. The data project implements a User model and the UserService provides user management functionality such as Authenticate, Register, Change Password, Update Profile etc.

The Web project implements a UserController with actions for Login/Register/NotAuthorized/NotAuthenticated etc. The ```AuthBuilder``` helper class defined in ```tb.Web.Helpers``` provides a ```BuildClaimsPrinciple``` method to build a set of user claims for User Login action when using cookie authentication and this can be modified to amend the claims added to the cookie.

To enable cookie Authentication the following statement is included in Program.cs.

```c#
builder.Services.AddCookieAuthentication();
```

Then Authentication/Authorisation are then turned on in the Application via the following statements in Program.cs

```c#
app.UseAuthentication();
app.UseAuthorization();
```

### Additional Functionality

1. Any Controller that inherits from the Web project BaseController, can utilise:

    a. The Alert functionality. Alerts can be used to display alert messages following controller actions. Review the UserController for an example using alerts.

    ```Alert("The User Was Registered Successfully", AlertType.info);```

    b. Authentication function helper methods
    * ```GetSignedInUserId()``` - returns Id of current logged in user or 0 if not logged in
    * ```IsAuthenticated()``` - returns a boolean to indicate if user is logged in

2. Two custom TagHelpers are included that provide

    a. Authentication and authorisation Tags

    * ```<p asp-authorized>Only displayed if the user is authenticated</p>```

    * ```<p asp-roles="Admin,Manager">Only displayed if the user has one of specified roles</p>```

    Note: to enable these tag helpers Program.cs needs following service added to DI container
    ```builder.Services.AddHttpContextAccessor();```

    b. Conditional Display Tag

    * ```<p asp-condtion="@some_boolean_expression">Only displayed if the condition is true</p>```

## Install tb

To install this solution as a tb (tb name is **termonclean**)

1. Download current version of the tb

    ```$ git clone https://github.com/termon/DotNettb.git```

2. Install the tb so it can be used by ```dotnet new``` command. Use the path (i.e the directory location)to the cloned tb directory without trailing '/'

Linux/macOS

```$ dotnet new -i /path/DotNettb```

Windows

```c: dotnet new -i c:\path\DotNettb```

3. Once installed you can create a new project using this tb

    ```dotnet new termonclean -o SolutionName```
