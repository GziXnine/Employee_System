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

                Console.ResetColor();

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
                        if (highlight == 0) ChooseAdd();
                        else if (highlight == 1) ChooseDisplay();
                        else if (highlight == 2) SortEmployees();
                        else if (highlight == 3) SearchEmployee();
                        else if (highlight == 4) ChooseDelete();
                        else loop = false;
                        break;
                }
            }
        }

        static void ChooseAdd()
        {
            Console.Clear();
            Console.WriteLine("Choose what to add:");
            Console.WriteLine("1 - Employee");
            Console.WriteLine("2 - Department");
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine() ?? string.Empty;

            if (choice == "1") AddEmployee();
            else if (choice == "2") DepartmentDataAccess.AddDepartment();
            else Console.WriteLine("Invalid choice. Returning to menu.");

            Console.WriteLine();
        }

        static void ChooseDisplay()
        {
            Console.Clear();
            Console.WriteLine("Choose what to display:");
            Console.WriteLine("1 - Employees");
            Console.WriteLine("2 - Departments");
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine() ?? string.Empty;

            if (choice == "1") DisplayEmployees();
            else if (choice == "2") DepartmentDataAccess.DisplayDepartments();
            else Console.WriteLine("Invalid choice. Returning to menu.");

            Console.ReadKey();
        }

        static void ChooseDelete()
        {
            Console.Clear();
            Console.WriteLine("Choose what to delete:");
            Console.WriteLine("1 - Employee");
            Console.WriteLine("2 - Department");
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine() ?? string.Empty;

            if (choice == "1") DeleteEmployee();
            else if (choice == "2") DepartmentDataAccess.DeleteDepartment();
            else Console.WriteLine("Invalid choice. Returning to menu.");

            Console.ReadKey();
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

                Console.Write("Would you like to add a new department? (Y / N): ");
                string? choice = Console.ReadLine()?.Trim().ToUpper();

                if (choice == "Y")
                    DepartmentDataAccess.AddDepartment();
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

            if (employees.Count == 0)
            {
                Console.WriteLine("No employees found.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("╔══════════╦════════════════════╦════════════╦══════════╦══════════════╗");
            Console.WriteLine("║ Employee ║       Name         ║   Salary   ║  Gender  ║ DepartmentID ║");
            Console.WriteLine("╠══════════╬════════════════════╬════════════╬══════════╬══════════════╣");

            foreach (var emp in employees)
            {
                Console.WriteLine($"║ {emp.EmployeeID,-8} ║ {emp.EmployeeName,-18} ║ {emp.EmployeeSalary,-10} ║ {emp.EmployeeGender,-8} ║ {emp.DepartmentID,-12} ║");
            }

            Console.WriteLine("╚══════════╩════════════════════╩════════════╩══════════╩══════════════╝");

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static void SortEmployees()
        {
            string sortColumn = "EmployeeID"; // Default sort by EmployeeID

            Console.Clear();
            Console.WriteLine("Choose Sorting Method:");
            Console.WriteLine("1 - Sort by ID (Default)");
            Console.WriteLine("2 - Sort by Name");
            Console.WriteLine("3 - Sort by Salary");
            Console.WriteLine("4 - Sort by Gender");
            Console.Write("Enter your choice: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1: sortColumn = "EmployeeID"; break;
                    case 2: sortColumn = "EmployeeName"; break;
                    case 3: sortColumn = "EmployeeSalary"; break;
                    case 4: sortColumn = "EmployeeGender"; break;
                    default:
                        Console.WriteLine("Invalid choice! Sorting by Employee ID.");
                        break;
                }
            }
            else
                Console.WriteLine("Invalid input! Sorting by Employee ID.");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = $"SELECT EmployeeID, EmployeeName, EmployeeSalary, EmployeeGender FROM Employees ORDER BY {sortColumn}";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                Console.Clear();
                Console.WriteLine("╔══════════╦════════════════════╦════════════╦══════════╗");
                Console.WriteLine("║ Employee ║       Name         ║   Salary   ║  Gender  ║");
                Console.WriteLine("╠══════════╬════════════════════╬════════════╬══════════╣");

                bool hasRows = false;
                while (reader.Read())
                {
                    hasRows = true;
                    Console.WriteLine($"║ {reader["EmployeeID"],-8} ║ {reader["EmployeeName"],-18} ║ {reader["EmployeeSalary"],-10} ║ {reader["EmployeeGender"],-8} ║");
                }

                if (!hasRows)
                    Console.WriteLine("║            No employees found!              ║");

                Console.WriteLine("╚══════════╩════════════════════╩════════════╩══════════╝");

                reader.Close();
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static void SearchEmployee()
        {
            Console.Clear();
            Console.WriteLine("Search Employee By:");
            Console.WriteLine("1 - Employee ID");
            Console.WriteLine("2 - Employee Name");
            Console.WriteLine("3 - Employee Salary");
            Console.Write("Enter your choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 3)
            {
                Console.WriteLine("Invalid choice! Press any key to return...");
                Console.ReadKey();
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = string.Empty;
                SqlCommand cmd = new SqlCommand();

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter Employee ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int id))
                        {
                            Console.WriteLine("Invalid Employee ID! Press any key to return...");
                            Console.ReadKey();
                            return;
                        }
                        query = "SELECT * FROM Employees WHERE EmployeeID = @ID";
                        cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ID", id);
                        break;

                    case 2:
                        Console.Write("Enter Employee Name: ");
                        string? name = Console.ReadLine()?.Trim();
                        if (string.IsNullOrEmpty(name))
                        {
                            Console.WriteLine("Invalid Name! Press any key to return...");
                            Console.ReadKey();
                            return;
                        }
                        query = "SELECT * FROM Employees WHERE EmployeeName LIKE @Name";
                        cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Name", $"%{name}%"); // Allows partial matches
                        break;

                    case 3:
                        Console.Write("Enter Employee Salary: ");
                        if (!int.TryParse(Console.ReadLine(), out int salary))
                        {
                            Console.WriteLine("Invalid Salary! Press any key to return...");
                            Console.ReadKey();
                            return;
                        }
                        query = "SELECT * FROM Employees WHERE EmployeeSalary = @Salary";
                        cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Salary", salary);
                        break;
                }

                SqlDataReader reader = cmd.ExecuteReader();

                Console.Clear();
                Console.WriteLine("╔══════════╦════════════════════╦════════════╦══════════╦═════════════╗");
                Console.WriteLine("║ Employee ║       Name         ║   Salary   ║  Gender  ║ Department  ║");
                Console.WriteLine("╠══════════╬════════════════════╬════════════╬══════════╬═════════════╣");

                bool hasRows = false;
                while (reader.Read())
                {
                    hasRows = true;
                    Console.WriteLine($"║ {reader["EmployeeID"],-8} ║ {reader["EmployeeName"],-18} ║ {reader["EmployeeSalary"],-10} ║ {reader["EmployeeGender"],-8} ║ {reader["DepartmentID"],-11} ║");
                }

                if (!hasRows)
                    Console.WriteLine("║                  No employees found!                      ║");

                Console.WriteLine("╚══════════╩════════════════════╩════════════╩══════════╩═════════════╝");

                reader.Close();
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }

        static void DeleteEmployee()
        {
            Console.Write("Enter Employee ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            {
                Console.WriteLine("Invalid Employee ID! Press any key to return...");
                Console.ReadKey();
                return;
            }

            // Confirm deletion
            Console.Write($"Are you sure you want to delete Employee ID {id}? (Y / N): ");
            string? confirmation = Console.ReadLine()?.Trim().ToUpper();

            if (confirmation != "Y")
            {
                Console.WriteLine("Deletion cancelled.");
                Console.ReadKey();
                return;
            }

            bool isDeleted = EmployeeDataAccess.DeleteEmployee(id);

            if (isDeleted)
                Console.WriteLine("Employee successfully deleted!");
            else
                Console.WriteLine("Employee not found or deletion failed.");

            Console.ReadKey();
        }
    }
}