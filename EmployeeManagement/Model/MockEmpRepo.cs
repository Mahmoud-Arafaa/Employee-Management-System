using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace EmployeeManagement.Model
{
    public class MockEmpRepo : IEmployeeReopsitory
    {

        List<Employee> EmpsList = new List<Employee>
        {
            new Employee{ID=1,Name="Mahmoud Arafa",Email="Arafa@22",Department=Dept.Cs},
            new Employee{ID=2,Name="B",Email="B@33",Department=Dept.Ds},
            new Employee{ID=3,Name="C",Email="C@22",Department=Dept.IT}

        };

        public Employee Add(Employee employee)
        {
            employee.ID = EmpsList.Max(i => i.ID) + 1;
            EmpsList.Add(employee);
            return employee;

        }

        public Employee Delete(Employee employee)
        {
            throw new System.NotImplementedException();
        }

        public Employee Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return EmpsList;
        }

        public Employee GetEmployee(int id)
        {
            return EmpsList.FirstOrDefault(x=>x.ID == id);
        }

        public Employee Update(Employee employee)
        {
            throw new System.NotImplementedException();
        }
    }
}
