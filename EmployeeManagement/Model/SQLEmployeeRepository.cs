using System.Collections.Generic;

namespace EmployeeManagement.Model
{
    public class SQLEmployeeRepository : IEmployeeReopsitory
    {
        private AppDbContext _appDbContext;

        public SQLEmployeeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public Employee Add(Employee employee)
        {
            _appDbContext.Add(employee);
            _appDbContext.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            var emp = _appDbContext.Employees.Find(id);
            _appDbContext.Employees.Remove(emp);
            _appDbContext.SaveChanges();
            return emp;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
           return _appDbContext.Employees;
        }

        public Employee GetEmployee(int id)
        {
            return _appDbContext.Employees.Find(id);
        }

        public Employee Update(Employee employeeChanges)
        {
            var employee = _appDbContext.Employees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _appDbContext.SaveChanges();
            return employeeChanges;
        }
    }
}
