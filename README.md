# SharpQuestTask_WebApp
Employee Management Web Application (ASP.NET Core MVC + SQL Server + ADO.NET)
Database Setup
Open SQL Server Management Studio (SSMS)
Run the script located at:
/Database/CreateEmployeeDb.sql
This:
Creates the database
Creates tables
Inserts 100 employees
Inserts salary rows

Update connection string in appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=EmployeeDemoDB;Trusted_Connection=True;"
}
Run the project:
dotnet restore
dotnet build
dotnet run

Features Implemented
1. Employee List
Search by name
Search by title
Displays:
Full name
Current title
Current salary
Contact details
Join date
Age
2. Title List
Shows all distinct titles
Shows minimum and maximum salary for each title
3. Add Employee
Add employee basic details
Add initial salary and title
Salary stored in EmployeeSalary table
