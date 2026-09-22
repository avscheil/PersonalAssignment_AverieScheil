using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PersonalAssignment_AverieScheil.Models
{
    public class User : IdentityUser
    {
        public string? UserRole {  get; set; }
    }
}
