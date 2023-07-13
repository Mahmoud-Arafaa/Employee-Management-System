using EmployeeManagement.utilities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Model.ViewModel
{
    public class RegisterViewModel
    {
        
        
            [Required]
            [EmailAddress]
            [Remote(action: "EmailInUse", controller:"Account")]
            [ValidEmailDomainAttribute(allowedDomain: "Arafa.com"
            ,ErrorMessage ="Email Domain Must Be Arafa.com")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password",
                ErrorMessage = "Password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

        public string City { get; set; }

    }
}
