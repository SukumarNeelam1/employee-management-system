using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.api.Model
{
    [Table("employeeTbl")] 
    public class EmployeeModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int employeeId { get; set; }

        [Required, MaxLength(55)]
        public string name { get; set; }

        [Required, StringLength(10, MinimumLength =10)]
        public string contactNo { get; set; }

        [Required, EmailAddress]
        public string email { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string state { get; set; } = string.Empty;
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string pincode { get; set; } = string.Empty; 
        public string altContactNo { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public int designationId { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime modifiedDate { get; set; }
        public string role { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]

        public string email { get; set; } = string.Empty;
        public string contactNo { get; set; } = string.Empty;
    }
}
