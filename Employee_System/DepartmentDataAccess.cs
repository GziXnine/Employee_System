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
                    Console.WriteLine("\nAvailable Departments:");
                    Console.WriteLine("----------------------");
                    while (reader.Read())
                        Console.WriteLine($"ID: {reader["DepartmentID"]}, Name: {reader["DepartmentName"]}");
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
    }
}
