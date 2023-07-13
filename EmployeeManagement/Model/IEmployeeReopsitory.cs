using System.Collections.Generic;

namespace EmployeeManagement.Model
{
    public interface IEmployeeReopsitory
    {
        public Employee GetEmployee(int id);
        public IEnumerable<Employee> GetAllEmployee();
        Employee Add(Employee employee);
        Employee Update(Employee employee);
        Employee Delete(int id);



    }
}
