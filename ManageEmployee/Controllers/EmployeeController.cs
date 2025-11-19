using EmployeeMvcAdo.Data;
using ManageEmployee.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace EmployeeMvcAdo.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly DbHelper _db;
        public EmployeeController(DbHelper db) { _db = db; }

        // GET: /Employee
        public async Task<IActionResult> Index(string? name, string? title)
        {
            // Name/title search for current title only (ToDate IS NULL)
            var sql = @"
SELECT e.EmployeeId, e.FirstName, e.LastName, e.SSN, e.DOB, e.Address, e.City, e.[State], e.Zip, e.Phone, e.JoinDate, e.ExitDate,
       s.Title, s.Salary
FROM dbo.Employee e
LEFT JOIN dbo.EmployeeSalary s ON e.EmployeeId = s.EmployeeId AND s.ToDate IS NULL
WHERE (@name IS NULL OR (e.FirstName + ' ' + e.LastName) LIKE '%' + @name + '%')
  AND (@title IS NULL OR s.Title LIKE '%' + @title + '%')
ORDER BY e.EmployeeId;
";
            var list = await _db.ExecuteReaderAsync(sql, reader => new Employee
            {
                EmployeeId = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                SSN = reader.GetString(3),
                DOB = reader.GetDateTime(4),
                Address = reader.IsDBNull(5) ? null : reader.GetString(5),
                City = reader.IsDBNull(6) ? null : reader.GetString(6),
                State = reader.IsDBNull(7) ? null : reader.GetString(7),
                Zip = reader.IsDBNull(8) ? null : reader.GetString(8),
                Phone = reader.IsDBNull(9) ? null : reader.GetString(9),
                JoinDate = reader.GetDateTime(10),
                ExitDate = reader.IsDBNull(11) ? (DateTime?)null : reader.GetDateTime(11),
                CurrentTitle = reader.IsDBNull(12) ? null : reader.GetString(12),
                CurrentSalary = reader.IsDBNull(13) ? (decimal?)null : reader.GetDecimal(13)
            },
            new SqlParameter("@name", (object?)name ?? DBNull.Value),
            new SqlParameter("@title", (object?)title ?? DBNull.Value)
            );

            ViewBag.Name = name;
            ViewBag.TitleFilter = title;
            return View(list);
        }

        // GET: /Employee/Titles
        public async Task<IActionResult> Titles()
        {
            var sql = @"
SELECT Title, MIN(Salary) AS MinSalary, MAX(Salary) AS MaxSalary
FROM dbo.EmployeeSalary
GROUP BY Title
ORDER BY Title;
";
            var list = await _db.ExecuteReaderAsync(sql, reader => new TitleStat
            {
                Title = reader.IsDBNull(0) ? "" : reader.GetString(0),
                MinSalary = reader.GetDecimal(1),
                MaxSalary = reader.GetDecimal(2)
            });

            return View(list);
        }

        // GET: /Employee/Add
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // POST: /Employee/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Employee model, string title, decimal salary)
        {
            if (!ModelState.IsValid) return View(model);

            // Insert Employee
            var insertEmpSql = @"
INSERT INTO dbo.Employee (FirstName, LastName, SSN, DOB, Address, City, [State], Zip, Phone, JoinDate, ExitDate)
VALUES (@fn, @ln, @ssn, @dob, @addr, @city, @st, @zip, @phone, @join, NULL);
SELECT SCOPE_IDENTITY();
";
            var newIdObj = await _db.ExecuteScalarAsync(insertEmpSql,
                new SqlParameter("@fn", model.FirstName ?? ""),
                new SqlParameter("@ln", model.LastName ?? ""),
                new SqlParameter("@ssn", model.SSN ?? ""),
                new SqlParameter("@dob", model.DOB),
                new SqlParameter("@addr", (object?)model.Address ?? DBNull.Value),
                new SqlParameter("@city", (object?)model.City ?? DBNull.Value),
                new SqlParameter("@st", (object?)model.State ?? DBNull.Value),
                new SqlParameter("@zip", (object?)model.Zip ?? DBNull.Value),
                new SqlParameter("@phone", (object?)model.Phone ?? DBNull.Value),
                new SqlParameter("@join", model.JoinDate)
            );

            var newId = Convert.ToInt32(newIdObj);

            // Insert Salary (current)
            var insertSalSql = @"
INSERT INTO dbo.EmployeeSalary (EmployeeId, FromDate, ToDate, Title, Salary)
VALUES (@empId, @from, NULL, @title, @salary);
";
            await _db.ExecuteNonQueryAsync(insertSalSql,
                new SqlParameter("@empId", newId),
                new SqlParameter("@from", DateTime.Now.Date),
                new SqlParameter("@title", title),
                new SqlParameter("@salary", salary)
            );

            return RedirectToAction(nameof(Index));
        }
    }
}
