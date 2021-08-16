using System;

namespace Sample.Rest.Api.Domain.Companies.Models
{
    public class CompanyDetails
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CreatedAt { get; set; }
        public string Status { get; set; }
    }
}
