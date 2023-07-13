using System.Collections.Generic;

namespace EmployeeManagement.Model.ViewModel
{
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Claims = new List<UserClaims>();  
        }
        public string userId { get; set; }
        public List<UserClaims> Claims { get; set; }
    }
}
