using Microsoft.AspNetCore.Hosting;
using ForgeReportSystem.Api.Data;
using ForgeReportSystem.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForgeReportSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    private readonly IWebHostEnvironment _environment;

    private static readonly string[] AllowedExtensions =
    {
    ".pdf", ".doc", ".docx", ".xls", ".xlsx",
    ".png", ".jpg", ".jpeg"
    };

    private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
    public ReportsController(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // GET ALL REPORTS
    [HttpGet]
    public async Task<IActionResult> GetReports()
    {
        var reports = await _context.Reports
            .Include(r => r.Department)
            .Include(r => r.SubmittedBy)
            .OrderByDescending(r => r.ReportingDate)
            .Select(r => new
            {
                id = r.Id,
                projectTitle = r.ProjectTitle,
                department = r.Department!.Name,
                site = r.Department!.Site,
                submittedBy = r.SubmittedBy!.FullName,
                projectStatus = r.ProjectStatus,
                projectSummary = r.ProjectSummary,
                challenges = r.Challenges,
                nextSteps = r.NextSteps,
                importance = r.Importance,
                dueDate = r.DueDate,
                reportingDate = r.ReportingDate,
                reviewStatus = r.ReviewStatus,
                adminFeedback = r.AdminFeedback,
                attachmentFileName = r.AttachmentFileName,
                attachmentFilePath = r.AttachmentFilePath
            })
            .ToListAsync();

        return Ok(reports);
    }

    // GET REPORTS FOR A PARTICULAR USER
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserReports(int userId)
    {
        var reports = await _context.Reports
            .Include(r => r.Department)
            .Include(r => r.SubmittedBy)
            .Where(r => r.SubmittedById == userId)
            .OrderByDescending(r => r.ReportingDate)
            .Select(r => new
            {
                id = r.Id,
                projectTitle = r.ProjectTitle,
                department = r.Department!.Name,
                site = r.Department!.Site,
                submittedBy = r.SubmittedBy!.FullName,
                projectStatus = r.ProjectStatus,
                projectSummary = r.ProjectSummary,
                challenges = r.Challenges,
                nextSteps = r.NextSteps,
                importance = r.Importance,
                dueDate = r.DueDate,
                reportingDate = r.ReportingDate,
                reviewStatus = r.ReviewStatus,
                adminFeedback = r.AdminFeedback
            })
            .ToListAsync();

        return Ok(reports);
    }

    // CREATE REPORT
    [HttpPost]
    public async Task<IActionResult> CreateReport([FromBody] Report report)
    {
        if (string.IsNullOrWhiteSpace(report.ProjectTitle))
            return BadRequest(new { message = "Project title is required." });

        if (report.DepartmentId <= 0)
            return BadRequest(new { message = "Department is required." });

        if (report.SubmittedById <= 0)
            return BadRequest(new { message = "Submitting user is required." });

        if (string.IsNullOrWhiteSpace(report.ProjectStatus))
            return BadRequest(new { message = "Project status is required." });

        if (string.IsNullOrWhiteSpace(report.ProjectSummary))
            return BadRequest(new { message = "Project summary is required." });

        if (string.IsNullOrWhiteSpace(report.Challenges))
            return BadRequest(new { message = "Challenges / blockers are required." });

        if (string.IsNullOrWhiteSpace(report.NextSteps))
            return BadRequest(new { message = "Next steps are required." });

        if (string.IsNullOrWhiteSpace(report.Importance))
            return BadRequest(new { message = "Importance is required." });

        report.ReviewStatus = "Pending";
        report.AdminFeedback = null;

        _context.Reports.Add(report);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Report submitted successfully.",
            reportId = report.Id
        });
    }

    // ADMIN APPROVE / REJECT
    [HttpPut("{id}/review")]
    public async Task<IActionResult> ReviewReport(
        int id,
        [FromBody] ReviewRequest request)
    {
        var report = await _context.Reports.FindAsync(id);

        if (report == null)
        {
            return NotFound(new
            {
                message = "Report not found."
            });
        }

        if (request.ReviewStatus != "Approved" &&
            request.ReviewStatus != "Rejected")
        {
            return BadRequest(new
            {
                message = "Review status must be Approved or Rejected."
            });
        }

        report.ReviewStatus = request.ReviewStatus;
        report.AdminFeedback = request.AdminFeedback;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = $"Report {request.ReviewStatus.ToLower()} successfully."
        });
    }

    // UPDATE / RESUBMIT A REJECTED REPORT
    [HttpPut("{id}/resubmit")]
    public async Task<IActionResult> ResubmitReport(
    int id,
    [FromBody] Report updatedReport)
{
        var report = await _context.Reports.FindAsync(id);

        if (report == null)
        {   
            return NotFound(new
        {
            message = "Report not found."
        });
        }

        if (report.ReviewStatus != "Rejected")
        {
            return BadRequest(new
        {
            message = "Only rejected reports can be resubmitted."
        });
        }

        if (string.IsNullOrWhiteSpace(updatedReport.ProjectTitle))
            return BadRequest(new { message = "Project title is required." });

        if (string.IsNullOrWhiteSpace(updatedReport.ProjectSummary))
            return BadRequest(new { message = "Project summary is required." });

        if (string.IsNullOrWhiteSpace(updatedReport.Challenges))
            return BadRequest(new { message = "Challenges / blockers are required." });

        if (string.IsNullOrWhiteSpace(updatedReport.NextSteps))
            return BadRequest(new { message = "Next steps are required." });

        report.ProjectTitle = updatedReport.ProjectTitle;
        report.ProjectStatus = updatedReport.ProjectStatus;
        report.ProjectSummary = updatedReport.ProjectSummary;
        report.Challenges = updatedReport.Challenges;
        report.NextSteps = updatedReport.NextSteps;
        report.Importance = updatedReport.Importance;
        report.DueDate = updatedReport.DueDate;
        report.ReportingDate = DateTime.UtcNow;

    // Send it back to the admin for review
        report.ReviewStatus = "Pending";
        report.AdminFeedback = null;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Report resubmitted successfully.",
            reportId = report.Id
        });
        }

    // UPLOAD AN ATTACHMENT TO A REPORT
[HttpPost("{id}/attachment")]
public async Task<IActionResult> UploadAttachment(
    int id,
    IFormFile file)
{
    if (file == null || file.Length == 0)
    {
        return BadRequest(new
        {
            message = "Please select a file."
        });
    }

    if (file.Length > MaxFileSize)
    {
        return BadRequest(new
        {
            message = "The maximum file size is 5 MB."
        });
    }

    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

    if (!AllowedExtensions.Contains(extension))
    {
        return BadRequest(new
        {
            message = "Allowed files: PDF, Word, Excel, PNG and JPG."
        });
    }

    var report = await _context.Reports.FindAsync(id);

    if (report == null)
    {
        return NotFound(new
        {
            message = "Report not found."
        });
    }

    var uploadFolder = Path.Combine(
        _environment.ContentRootPath,
        "uploads"
    );

    Directory.CreateDirectory(uploadFolder);

    // Generate a safe, unique filename
    var storedFileName = $"{Guid.NewGuid()}{extension}";
    var filePath = Path.Combine(uploadFolder, storedFileName);

    await using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    report.AttachmentFileName = Path.GetFileName(file.FileName);
    report.AttachmentFilePath = storedFileName;

    await _context.SaveChangesAsync();

    return Ok(new
    {
        message = "Attachment uploaded successfully.",
        fileName = report.AttachmentFileName
    });
}

}

public class ReviewRequest
{
    public string ReviewStatus { get; set; } = string.Empty;

    public string? AdminFeedback { get; set; }
}