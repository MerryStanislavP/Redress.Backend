using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RedressAPI.Models
{
    public class User : IdentityUser<Guid>
    {
        [Key]
        public Guid UserId { get; set; }

        [Required, MaxLength(50)]
        public override string UserName { get; set; } = null!;

        [Required, EmailAddress]
        public override string Email { get; set; } = null!;

        [Required, Phone]
        public override string PhoneNumber { get; set; } = null!;

        [Required, Column(TypeName = "varchar(20)")] // Храним Role как строку в БД
        public UserRole Role { get; set; } = UserRole.Customer;

        public virtual Profile Profile { get; set; } = null!; 
    }
    public enum UserRole
    {
        Admin,
        Moderator,
        Customer
    }
}
