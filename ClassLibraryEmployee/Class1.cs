namespace ClassLibraryEmployee
{
    public enum Gender
    {
        Male,
        Female
    }

    public class Employee
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public int EmployeeSalary { get; set; }
        public int DepartmentID { get; set; }
        public Gender EmployeeGender { get; set; }
    }
}
