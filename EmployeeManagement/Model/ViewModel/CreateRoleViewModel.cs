using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Model.ViewModel
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
    

}
