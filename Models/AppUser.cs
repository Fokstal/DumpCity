using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DumpCity.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string? FullName { get; set; }
    }
}