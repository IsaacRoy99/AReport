using ForgeReportSystem.Api.Models;

namespace ForgeReportSystem.Api.Data;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context)
    {
        // Don't seed twice
        if (context.Users.Any())
            return;

        var departments = new List<Department>
        {
            new() { Name = "Academics", Site = "Bloemfontein" },
            new() { Name = "Academics", Site = "Clearwater" },
            new() { Name = "Construct", Site = "Clearwater" },
            new() { Name = "Consult", Site = "Clearwater" },
            new() { Name = "EXCO", Site = "Clearwater" },
            new() { Name = "Finance", Site = "Clearwater" },
            new() { Name = "FNC", Site = "Northern Campus" },
            new() { Name = "HR & Payroll", Site = "Clearwater" },
            new() { Name = "IT/Ozone", Site = "Clearwater" },
            new() { Name = "Studios", Site = "Clearwater" },
            new() { Name = "Talent", Site = "Clearwater" },
            new() { Name = "Compliance", Site = "Underberg" }
        };

        context.Departments.AddRange(departments);
        context.SaveChanges();

        Department GetDepartment(string name, string site)
        {
            return context.Departments.First(d =>
                d.Name == name && d.Site == site);
        }

        var users = new List<User>
        {
            new()
            {
                FullName = "Jane Oosthuizen",
                Email = "jane@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("Academics", "Bloemfontein").Id
            },

            new()
            {
                FullName = "Disebo Mokoena",
                Email = "disebo@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("Academics", "Clearwater").Id
            },

            new()
            {
                FullName = "Keegan Abdul",
                Email = "keegan@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("Construct", "Clearwater").Id
            },

            new()
            {
                FullName = "Ali Muhammed",
                Email = "Ali@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("Construct", "Clearwater").Id
            },

            new()
            {
                FullName = "Lilian Mathuntuta",
                Email = "lilian@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("Consult", "Clearwater").Id
            },

            new()
            {
                FullName = "Tariq Shurmah",
                Email = "tariq@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("EXCO", "Clearwater").Id
            },

            new()
            {
                FullName = "Craig Clutty",
                Email = "craig@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("EXCO", "Clearwater").Id
            },

            new()
            {
                FullName = "Lauren Theron",
                Email = "lauren@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("EXCO", "Clearwater").Id
            },

            new()
            {
                FullName = "Kishanry Chetty",
                Email = "kishanry@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("Finance", "Clearwater").Id
            },

            new()
            {
                FullName = "Linda Earl",
                Email = "linda@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("Finance", "Clearwater").Id
            },

            new()
            {
                FullName = "Francis Olusola",
                Email = "francis@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("FNC", "Northern Campus").Id
            },

            new()
            {
                FullName = "Wayne Lardner",
                Email = "wayne@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("FNC", "Northern Campus").Id
            },

            new()
            {
                FullName = "Charmain Lardner",
                Email = "charmain@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("FNC", "Northern Campus").Id
            },

            new()
            {
                FullName = "Lezel Ruth Scholtz",
                Email = "lezel@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("HR & Payroll", "Clearwater").Id
            },

            new()
            {
                FullName = "Jabulani Mashimbye",
                Email = "jabu@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("IT/Ozone", "Clearwater").Id
            },

            new()
            {
                FullName = "Michael Van Der Westhuizen",
                Email = "michael@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("Studios", "Clearwater").Id
            },

            new()
            {
                FullName = "Renoo Shurmah",
                Email = "renoo@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("Talent", "Clearwater").Id
            },

            new()
            {
                FullName = "Kayleen Lutchman",
                Email = "kayleen@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("Talent", "Clearwater").Id
            },

            new()
            {
                FullName = "Des Webster",
                Email = "des@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("FNC", "Northern Campus").Id
            },

            new()
            {
                FullName = "Olga-Jane Ackerman",
                Email = "olga@forgeacademy.co.za",
                Role = "DepartmentHead",
                DepartmentId = GetDepartment("Compliance", "Underberg").Id
            },

            // Temporary admin account
            new()
            {
                FullName = "System Administrator",
                Email = "admin@forgeacademy.co.za",
                Role = "Admin",
                DepartmentId = GetDepartment("EXCO", "Clearwater").Id
            }
        };

        context.Users.AddRange(users);
        context.SaveChanges();
    }
}