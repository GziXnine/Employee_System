namespace ClassLibraryEmployee
{
    public enum Gender
    {
        Male,
        Female
    }

    public class Employee : IComparable<Employee>
    {
        private static int counter = 1;
        public int ID { get; }  // Readonly ID
        public string Name { get; set; }
        public float Salary { get; set; }
        public Gender Gender { get; }  // Readonly Gender
        private int age;

        // Age Property
        public int Age
        {
            get { return age; }
            set
            {
                age = (value >= 18 && value < 100) ? value : 18;
            }
        }

        public Employee(string name, float salary, Gender gender, int age)
        {
            ID = counter++; // Auto Increment ID
            Name = name;
            Salary = salary;
            Gender = gender;
            Age = age;
        }

        public void Display()
        {
            Console.WriteLine($"\nEmployee Details:");
            Console.WriteLine($"ID: {ID}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Salary: {Salary:C}");
            Console.WriteLine($"Gender: {Gender}");
            Console.WriteLine($"Age: {Age}");
            Console.WriteLine("----------------------------------------");
        }

        // Implement IComparable - Default sorting by ID
        public int CompareTo(Employee other)
        {
            if (other == null)
                return 1; // Consider null as less than any object

            return this.ID.CompareTo(other.ID);
        }
    }

    //  Extension Method
    public static class EmployeeExtensions
    {
        public static void PrintAllEmployees(this Employee[] employees)
        {
            foreach (var emp in employees)
            {
                if (emp != null)
                    emp.Display();
            }
        }
    }
}
