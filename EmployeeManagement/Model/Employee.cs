using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Model
{
    public class Employee
    {
       public int ID { get; set; }
       [NotMapped]
       public string EncrypedID { get; set; }
       [Required]
       [MaxLength(50,ErrorMessage ="Can't Exceed 50 Char's Length!")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage ="Invalid Email Format")]
        public string Email { get; set; }
        [Required]
        public Dept? Department { get; set; }

        public string PhotoPath { get; set; }

    }
}
