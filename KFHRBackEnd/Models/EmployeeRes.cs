using KFHRBackEnd.Models.Entites;
using System.ComponentModel.DataAnnotations;

public class EmployeeRes
{
    public int Id { get; set; }
    public string Name { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public DateTime DOB { get; set; }

    public string? Gender { get; set; }

    [Url]
    public string? ProfilePicURL { get; set; }

    public int? NFCIdNumber { get; set; }


    public string? PositionName { get; set; }

    public Department DepartmentName { get; set; }


    public int? PointEarned { get; set; }

    public bool IsAdmin { get; set; }
}