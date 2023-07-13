using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Model
{
    public static class ModelBuliderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee { ID=1,Name="Mahmoud Arafa",Department=Dept.Cs,Email="Mahmoud@Arafa.com"}
              );
            
        }
    }
}
