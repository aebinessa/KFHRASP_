using KFHRBackEnd.Models.Entites;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class AddEmployeeRequest
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public DateTime DOB { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [AllowNull]
    [Url]
    public string? ProfilePicURL { get; set; }

    public int? NFCIdNumber { get; set; }

    public int? PositionId { get; set; }

    public int? DepartmentId { get; set; }

    public int? PointEarned { get; set; }
}
