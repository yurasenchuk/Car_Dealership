using System.ComponentModel.DataAnnotations;

namespace Car_Dealership.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Enter email!")]
        public string Email { get; set; }
         
        [Required(ErrorMessage = "Enter password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}