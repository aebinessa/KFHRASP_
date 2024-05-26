using KFHRBackEnd.Models.Entites;
using KFHRBackEnd.Models.Entites.Request.Employee;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;



public class AddEmployee
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

        public Position? PositionId { get; set; }

        public Department? DepartmentId { get; set; }

        public int? PointEarned { get; set; }
    }

