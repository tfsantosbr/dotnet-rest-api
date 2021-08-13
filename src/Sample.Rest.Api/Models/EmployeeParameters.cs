using Sample.Rest.Api.Extensions;
using System;

namespace Sample.Rest.Api.Models
{
    public class EmployeeParameters : Parameter
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? HiredIn { get; set; }
    }
}
