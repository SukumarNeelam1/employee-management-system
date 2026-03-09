using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee.api.Model
{
    [Table("employeeTbl")]
    public class EmployeeModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int employeeId { get; set; }

        public string name { get; set; }

        public string email { get; set; } = string.Empty;

        public string passwordHash { get; set; } = string.Empty;

        public string? contactNo { get; set; }

        public string? city { get; set; }

        public string? state { get; set; }

        public string? pincode { get; set; }

        public string? altContactNo { get; set; }

        public string? address { get; set; }

        public int? designationId { get; set; }

        public DateTime createdDate { get; set; }

        public DateTime modifiedDate { get; set; }

        public string role { get; set; } = "User";
    }
}