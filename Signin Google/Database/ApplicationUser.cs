using Microsoft.AspNetCore.Identity;

namespace Signin_Google.Database;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ProfilePictureUrl { get; set; }
}