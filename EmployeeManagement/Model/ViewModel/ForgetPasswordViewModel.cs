using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Model.ViewModel
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
