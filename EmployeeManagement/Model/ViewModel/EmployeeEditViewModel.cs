namespace EmployeeManagement.Model.ViewModel
{
    public class EmployeeEditViewModel : EmployeeCreateViewModel
    {
        public int ID { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
