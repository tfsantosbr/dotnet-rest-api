using System;

namespace Sample.Rest.Api.Domain.Employees.Models
{
    public class CreateEmployee
    {
        public DateTime HiredIn { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
