namespace ManageEmployee.Models
{
    public class EmployeeSalary
    {
        public int SalaryId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Title { get; set; }
        public decimal Salary { get; set; }
    }
}
