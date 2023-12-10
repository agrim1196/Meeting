namespace MeetingPlannerAPI.Model
{
    public record Employees
    {
        public int EmployeeId { get; set; }

        public string? EmployeeName { get; set; }

        public string? Password { get; set; }

        public string? Client { get; set; }

        public string? BU { get; set; }
    }
}
