using Sample.Rest.Api.Domain;
using Sample.Rest.Api.Extensions;
using System;

namespace Sample.Rest.Api.Models
{
    public class CompanyParameters : Parameter
    {
        public string Name { get; set; }
        public CompanyStatus? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
