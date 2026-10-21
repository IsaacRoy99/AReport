namespace ForgeReportSystem.Api.Models;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = "DepartmentHead";

    public int DepartmentId { get; set; }

    public Department? Department { get; set; }

    public ICollection<Report> Reports { get; set; } = new List<Report>();
}