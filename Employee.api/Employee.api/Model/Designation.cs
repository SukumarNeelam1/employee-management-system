using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Employee.api.Model
{
    [Table("designationTbl")]
    public class Designation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int designationId { get; set; }
        public string designationName { get; set; } = string.Empty;
        public int departmentId { get; set; }

        [ForeignKey("departmentId")]
        public Department? Department { get; set; }
    }
}
