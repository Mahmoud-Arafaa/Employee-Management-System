using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Model.ViewModel
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        public string ID { get; set; }
        
        [Required(ErrorMessage ="Role Name Is Required")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; } // All Users For Specific Role! (join)
    }
}
