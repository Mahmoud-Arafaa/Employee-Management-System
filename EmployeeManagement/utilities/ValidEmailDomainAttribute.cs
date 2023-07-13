using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.utilities
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string _allowedDomain;

        public ValidEmailDomainAttribute(string allowedDomain)
        {
            _allowedDomain = allowedDomain;
        }
        public override bool IsValid(object value)
        {
            string [] url = value.ToString().Split('@');
            return url[1] == _allowedDomain;

        }
    }
}
