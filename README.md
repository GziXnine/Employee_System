# 🏢 Employee Management System

A simple ADO.NET console-based Employee Management System that allows users to manage employees and departments using a Microsoft SQL Server database.

## 📸 Preview

<p align="center">
  <img src="https://img.shields.io/badge/Completion-100%25-brightgreen" alt="Completion Status">
  &nbsp;&nbsp;
  <img src="https://visitor-badge.laobi.icu/badge?page_id=GziXnine/Employee_System" alt="Visitors">
  &nbsp;&nbsp;
  <img src="https://img.shields.io/github/repo-size/GziXnine/Employee_System" alt="Repository Size">
</p>

## 📌 Features

✅ Add New Employees & Departments  
✅ Display Employees & Departments  
✅ Sort Employees  
✅ Search Employees  
✅ Delete Employees & Departments  
✅ Exit the System  

<p align="center">
  <img src="https://github.com/GziXnine/Employee_System/blob/main/Photo/console_out.jpg" alt="Data Base Query">
  <br>
  <br>
  <img src="https://github.com/GziXnine/Employee_System/blob/main/Photo/Employee_table.jpg" alt="Data Base Employee">
  <br>
  <br>
  <img src="https://github.com/GziXnine/Employee_System/blob/main/Photo/department.jpg" alt="Data Base Department">
</p>

## 📂 Project Structure

```plaintext
Employee_System/
├── ClassLibraryEmployee/
│   └── Employee.cs
├── Employee_System/
│   ├── AppSettings.json
│   ├── DataBaseHelper.cs
│   ├── DepartmentDataAccess.cs
│   ├── EmployeeDataAccess.cs
│   └── Program.cs
```

## 🗄 Database Structure

The system consists of two tables:  

<p align="center">
  <img src="https://github.com/GziXnine/Employee_System/blob/main/Photo/Database_diagram.jpg" alt="Data Base Tables">
</p>

## 🔧 Setup Instructions

### 1️⃣ Clone the Repository

```sh
git clone https://github.com/GziXnine/Employee_System.git
cd Employee_System
```

## 2️⃣ Configure the Database  
- Open **SQL Server Management Studio (SSMS)** or any **SQL client**.  
- Execute the `NewCompanyDB_ADO.sql` to create the database and insert sample data.  

<p align="center">
  <img src="https://github.com/GziXnine/Employee_System/blob/main/Photo/sql_query.jpg" alt="Data Base Query">
</p>

## 3️⃣ Update the Connection String  
- Open `AppSettings.json ` 
- Modify the connection string to match your database settings:  

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.; Database=NewCompanyDB_ADO; Trusted_Connection=True; MultipleActiveResultSets=True; Integrated Security=SSPI; TrustServerCertificate=True;"
  }
}
```

## 4️⃣ Run the Project 🚀
```sh
dotnet run
```

## 👥 Authors  

This project was developed by:  

- [**Abdelrahman Emad**](https://github.com/omda777)  
- [**Ahmed Allam**](https://github.com/GziXnine)  

We collaborated to build this system using **ADO.NET, C#**, and **MS SQL** to ensure efficiency and scalability. 🚀  

## 🤝 Contributing  
💡 Contributions are welcome! Feel free to submit a pull request.  

## 📜 License  
📄 This project is licensed under the **BSD-3-Clause License**. See the [LICENSE](./LICENSE) file for more details.
