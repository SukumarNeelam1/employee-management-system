namespace Employee.api.DTOs
{
    public class EmployeeCreateDto
    {
        public string name { get; set; }
        public string email { get; set; }
        //public string password { get; set; }
        public int designationId { get; set; }

        public string contactNo { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
        public string altContactNo { get; set; }
        public string address { get; set; }
    }
}