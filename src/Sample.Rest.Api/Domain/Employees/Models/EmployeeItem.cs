using System;

namespace Sample.Rest.Api.Domain.Employees.Models
{
    public class EmployeeItem
    {
        public Guid Id { get; set; }
        public string HiredIn { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
