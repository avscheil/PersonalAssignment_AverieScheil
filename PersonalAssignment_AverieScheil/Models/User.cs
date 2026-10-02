using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalAssignment_AverieScheil.Data;

namespace PersonalAssignment_AverieScheil.Models
{
    public class User : ApplicationUser
    {
        public string? UserRole {  get; set; }
    }
}
