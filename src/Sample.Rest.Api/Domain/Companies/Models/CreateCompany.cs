using System;

namespace Sample.Rest.Api.Domain.Companies.Models
{
    public class CreateCompany
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
