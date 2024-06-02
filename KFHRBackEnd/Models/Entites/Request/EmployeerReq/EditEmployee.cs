using KFHRBackEnd.Models.Entites;



using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class EditEmployee
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public DateTime DOB { get; set; }

    [Required]
    public Gender Gender { get; set; }


    [Url]
 
    public string? ProfilePicURL { get; set; }

    public int? NFCIdNumber { get; set; }

    public string? PositionName { get; set; }

    public int? DepartmentId { get; set; }

    public int? PointEarned { get; set; }
}

