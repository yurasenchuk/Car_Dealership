using System.ComponentModel.DataAnnotations;

namespace Car_Dealership.Models
{
    public class Registration
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter first name of user!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter last name of user!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter email!")]
        [EmailAddress(ErrorMessage = "Your email is not valid! Email format: ***@***.***")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter password!")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Minimum length must be 8, maximum 30!")]
        [Attributes.PasswordAttribute]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}