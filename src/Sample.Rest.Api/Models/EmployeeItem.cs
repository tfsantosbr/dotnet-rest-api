using System;

namespace Sample.Rest.Api.Models
{
    public class EmployeeItem
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string HiredIn { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
