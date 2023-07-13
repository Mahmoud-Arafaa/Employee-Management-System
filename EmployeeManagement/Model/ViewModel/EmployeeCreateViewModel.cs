using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Model.ViewModel
{
    public class EmployeeCreateViewModel
    {
        public int ID { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Can't Exceed 50 Char's Length!")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }
        [Required]
        public Dept? Department { get; set; }

        public IFormFile Photo { get; set; }
    }
}
