namespace ForgeReportSystem.Api.Models;

public class Report
{
    public int Id { get; set; }

    public string ProjectTitle { get; set; } = string.Empty;

    public int DepartmentId { get; set; }

    public Department? Department { get; set; }

    public int SubmittedById { get; set; }

    public User? SubmittedBy { get; set; }

    public string ProjectStatus { get; set; } = string.Empty;

    public string ProjectSummary { get; set; } = string.Empty;

    public string Challenges { get; set; } = string.Empty;

    public string NextSteps { get; set; } = string.Empty;

    public string Importance { get; set; } = string.Empty;

    public DateTime DueDate { get; set; }

    public DateTime ReportingDate { get; set; }

    public string ReviewStatus { get; set; } = "Pending";

    public string? AdminFeedback { get; set; }

    public string? AttachmentFileName { get; set; }
public string? AttachmentFilePath { get; set; }
}