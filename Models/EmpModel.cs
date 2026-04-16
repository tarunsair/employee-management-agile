using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace employee_management_agile.Models
{
    [Table("EmployeesTable")]
    public class EmpModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        [Display(Name = "Employee Name")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Department is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Department must be between 2 and 100 characters.")]
        [Display(Name = "Department")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Department can only contain letters and spaces.")]
        public string? Department { get; set; }
        [Required(ErrorMessage = "Position is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Position must be between 2 and 100 characters.")]
        [Display(Name = "Position")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Position can only contain letters and spaces.")]
        public string? Position { get; set; }
        [Required(ErrorMessage = "Salary is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        [Display(Name = "Salary")]
        public decimal Salary { get; set; }
        [Display(Name = "Role")]
        public string? Role { get; set; } = "Employee"; // Default role is Employee
    }
}