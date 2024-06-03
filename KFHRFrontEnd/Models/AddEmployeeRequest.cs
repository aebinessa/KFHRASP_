using KFHRBackEnd.Models.Entites;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class AddEmployeeRequest
{
    
    public string Name { get; set; }

   
    public string Email { get; set; }

    public string ProfilePicURL { get; set; }

    public DateTime? DOB { get; set; }

    public Gender Gender { get; set; }
    public int PointEarned { get; set; }

    public int? PositionId { get; set; }
    public string? PositionName { get; set; }

    public int? DepartmentId { get; set; }

    public Department DepartmentName { get; set; }
}

public enum Gender
{
    Male,
    Female
}
