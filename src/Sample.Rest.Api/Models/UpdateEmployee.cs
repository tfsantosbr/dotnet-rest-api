using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Rest.Api.Models
{
    public class UpdateEmployee
    {
        public DateTime HiredIn { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
