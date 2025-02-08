using Microsoft.Data.SqlClient;
using ClassLibraryEmployee;
using Employee_System;

public static class EmployeeDataAccess
{
    private static readonly string connectionString = DataBaseHelper.GetConnectionString();

    public static void AddEmployee(Employee employee)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Employees (EmployeeName, EmployeeSalary, DepartmentID, EmployeeGender) VALUES (@Name, @Salary, @DepartmentID, @Gender);";
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Name", employee.EmployeeName);
            cmd.Parameters.AddWithValue("@Salary", employee.EmployeeSalary);
            cmd.Parameters.AddWithValue("@DepartmentID", employee.DepartmentID);
            cmd.Parameters.AddWithValue("@Gender", employee.EmployeeGender.ToString());

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public static List<Employee> GetAllEmployees()
    {
        List<Employee> employees = new List<Employee>();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT EmployeeID, EmployeeName, EmployeeSalary, DepartmentID, EmployeeGender FROM Employees";
            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                employees.Add(new Employee()
                {
                    EmployeeID = reader.GetInt32(0),
                    EmployeeName = reader.GetString(1),
                    EmployeeSalary = reader.GetInt32(2),
                    DepartmentID = reader.GetInt32(3),
                    EmployeeGender = Enum.Parse<Gender>(reader.GetString(4))
                });
            }
        }

        return employees;
    }

    public static bool DeleteEmployee(int id)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "DELETE FROM Employees WHERE EmployeeID = @ID";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0; // Returns true if at least one row was deleted
            }
        }
    }
}