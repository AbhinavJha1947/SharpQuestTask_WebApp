namespace ManageEmployee.Models
{
        public class Employee
        {
            public int EmployeeId { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? SSN { get; set; }
            public DateTime DOB { get; set; }
            public string? Address { get; set; }
            public string? City { get; set; }
            public string? State { get; set; }
            public string? Zip { get; set; }
            public string? Phone { get; set; }
            public DateTime JoinDate { get; set; }
            public DateTime? ExitDate { get; set; }

            // non-mapped:
            public string? CurrentTitle { get; set; }
            public decimal? CurrentSalary { get; set; }

            public string FullName => $"{FirstName} {LastName}";
        }
    }
