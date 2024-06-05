using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "Логин")]
        public required string Login { get; set; }
    }
}
