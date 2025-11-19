Employee Management Web Application

Tech Stack: ASP.NET Core MVC · SQL Server · ADO.NET (No Entity Framework)

📌 Database Setup

Open SQL Server Management Studio (SSMS)

Run the script located at:

/Database/CreateEmployeeDb.sql


This script will:

Create the database

Create required tables

Insert 100 employees

Insert salary rows

Update the connection string in appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=EmployeeDemoDB;Trusted_Connection=True;"
}

▶️ Running the Project
dotnet restore
dotnet build
dotnet run

✨ Features Implemented
Employee List

Search by name

Search by title

Displays:

Full name

Current title

Current salary

Contact details

Join date

Age

Title List

Shows all distinct titles

Shows minimum & maximum salary

Add Employee

Add employee details

Add initial title and salary

Salary stored in EmployeeSalary table
