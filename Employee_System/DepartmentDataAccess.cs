using Microsoft.Data.SqlClient;

namespace Employee_System
{
    public class DepartmentDataAccess
    {
        private static string connectionString = DataBaseHelper.GetConnectionString();

        public static void DisplayDepartments()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT DepartmentID, DepartmentName FROM Departments";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    List<(int, string)> departments = new List<(int, string)>();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        departments.Add((id, name));
                    }

                    if (departments.Count == 0)
                    {
                        Console.WriteLine("No departments found.");
                        Console.ReadKey();
                        return;
                    }

                    Console.Clear();
                    Console.WriteLine("╔══════════════╦════════════════════════════════╗");
                    Console.WriteLine("║ DepartmentID ║         Department Name        ║");
                    Console.WriteLine("╠══════════════╬════════════════════════════════╣");

                    foreach (var dept in departments)
                    {
                        Console.WriteLine($"║ {dept.Item1,-12} ║ {dept.Item2,-30} ║");
                    }

                    Console.WriteLine("╚══════════════╩════════════════════════════════╝");

                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                }
            }
        }

        public static bool DepartmentExists(int departmentID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Departments WHERE DepartmentID = @DepartmentID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        public static int AddDepartment()
        {
            Console.Write("Enter Department Name: ");
            string? deptName = Console.ReadLine()?.Trim();

            while (string.IsNullOrWhiteSpace(deptName))
            {
                Console.Write("Department name cannot be empty. Enter Department Name: ");
                deptName = Console.ReadLine()?.Trim();
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Departments (DepartmentName) VALUES (@DepartmentName); SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentName", deptName);
                    int newDeptID = Convert.ToInt32(cmd.ExecuteScalar());
                    Console.WriteLine($"New department added with ID: {newDeptID}");
                    return newDeptID;
                }
            }
        }

        public static void DeleteDepartment()
        {
            Console.Write("Enter Department ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
            {
                Console.WriteLine("Invalid Department ID! Press any key to return...");
                Console.ReadKey();
                return;
            }

            Console.Write($"Are you sure you want to delete Department ID {id}? (Y / N): ");
            string? confirmation = Console.ReadLine()?.Trim().ToUpper();

            if (confirmation != "Y")
            {
                Console.WriteLine("Deletion cancelled.");
                Console.ReadKey();
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Departments WHERE DepartmentID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        Console.WriteLine("Department successfully deleted!");
                    else
                        Console.WriteLine("Department not found or deletion failed.");
                }
            }
            Console.ReadKey();
        }
    }
}
