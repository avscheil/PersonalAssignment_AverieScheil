using Microsoft.AspNetCore.Identity;
namespace PersonalAssignment_AverieScheil.Data;
// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string? UserRole { get; set; }
}
