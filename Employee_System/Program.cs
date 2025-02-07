using Microsoft.Data.SqlClient;
using ClassLibraryEmployee;

namespace Employee_System
{
    internal class Program
    {
        private static readonly string connectionString = DataBaseHelper.GetConnectionString();
        static void Main(string[] args)
        {
            int highlight = 0;
            bool loop = true;
            string[] menu = new string[] { "  New  ", " Display ", "  Sort  ", " Search ", " Delete ", "  Exit  " };

            while (loop)
            {
                Console.Clear();
                for (int i = 0; i < menu.Length; i++)
                {
                    Console.BackgroundColor = (highlight == i) ? ConsoleColor.Blue : ConsoleColor.Black;
                    Console.SetCursorPosition(60, (i + 1) * (30 / (menu.Length + 1)));
                    Console.WriteLine(menu[i]);
                }

                ConsoleKeyInfo x = Console.ReadKey();
                switch (x.Key)
                {
                    case ConsoleKey.DownArrow:
                        highlight = (highlight + 1) % menu.Length;
                        break;
                    case ConsoleKey.UpArrow:
                        highlight = (highlight - 1 + menu.Length) % menu.Length;
                        break;
                    case ConsoleKey.Home:
                        highlight = 0;
                        break;
                    case ConsoleKey.End:
                        highlight = menu.Length - 1;
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        if (highlight == 0) AddEmployee();
                        else if (highlight == 1) DisplayEmployees();
                        //else if (highlight == 2) SortEmployees();
                        //else if (highlight == 3) SearchEmployee();
                        //else if (highlight == 4) DeleteEmployee();
                        else loop = false;
                        break;
                }
            }
        }

        static void AddEmployee()
        {
            int salary = 0, departmentID = 0;

            Console.Write("Enter Name: ");
            string? name = Console.ReadLine()?.Trim();

            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("Name cannot be empty. Enter Name: ");
                name = Console.ReadLine()?.Trim();
            }

            Console.Write("Enter Salary: ");
            while (!int.TryParse(Console.ReadLine(), out salary) || salary < 0)
                Console.Write("Invalid salary. Enter a valid number: ");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Display existing departments
                string deptQuery = "SELECT DepartmentID, DepartmentName FROM Departments";
                SqlCommand deptCmd = new SqlCommand(deptQuery, conn);
                SqlDataReader reader = deptCmd.ExecuteReader();

                Console.WriteLine("\nAvailable Departments:");
                Console.WriteLine("----------------------");

                while (reader.Read())
                    Console.WriteLine($"ID: {reader["DepartmentID"]}, Name: {reader["DepartmentName"]}");

                reader.Close();
            }

            // Ask for Department ID
            Console.Write("\nEnter Department ID: ");
            while (!int.TryParse(Console.ReadLine(), out departmentID) || departmentID < 0)
                Console.Write("Invalid department ID. Enter a valid number: ");

            // Check if department exists
            while (!DepartmentDataAccess.DepartmentExists(departmentID))
            {
                Console.WriteLine("Error: Department does not exist.");

                Console.Write("Would you like to add a new department? (Y/N): ");
                string? choice = Console.ReadLine()?.Trim().ToUpper();

                if (choice == "Y")
                {
                    DepartmentDataAccess.AddDepartment();
                }
                else
                {
                    Console.Write("Enter a valid Department ID: ");
                    while (!int.TryParse(Console.ReadLine(), out departmentID) || departmentID < 0)
                        Console.Write("Invalid department ID. Enter a valid number: ");
                }
            }

            Console.Write("Enter Gender (M / F): ");
            string? genderInput = Console.ReadLine()?.Trim().ToUpper();

            while (genderInput != "M" && genderInput != "F")
            {
                Console.Write("Invalid input. Enter Gender (M / F): ");
                genderInput = Console.ReadLine()?.Trim().ToUpper();
            }

            Gender gender = genderInput == "M" ? Gender.Male : Gender.Female;

            // Insert Employee
            EmployeeDataAccess.AddEmployee(new Employee { EmployeeName = name, EmployeeSalary = salary, DepartmentID = departmentID, EmployeeGender = gender });
            Console.WriteLine("Employee Added!");

            Console.ReadKey();
        }

        static void DisplayEmployees()
        {
            List<Employee> employees = EmployeeDataAccess.GetAllEmployees();

            foreach (var emp in employees)
                Console.WriteLine($"ID: {emp.EmployeeID}, Name: {emp.EmployeeName}, Salary: {emp.EmployeeSalary}, Gender: {emp.EmployeeGender}, Department: {emp.DepartmentID}");

            Console.ReadKey();
        }
    }
}