using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUpLibrary.Model
{
    public class RegisterBindingModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Password must be atleast 6 characters long.", MinimumLength = 6)]
        [RegularExpression(@".*[0-9].*", ErrorMessage = "Password must contain a digit.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First_Name")]
        public string First_Name { get; set; }

        [Required]
        [Display(Name = "Last_Name")]
        public string Last_Name { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
    }
}
