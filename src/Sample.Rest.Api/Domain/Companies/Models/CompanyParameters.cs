using Sample.Rest.Api.Extensions;
using System;

namespace Sample.Rest.Api.Domain.Companies.Models
{
    public class CompanyParameters : Parameter
    {
        public string Name { get; set; }
        public CompanyStatus? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
