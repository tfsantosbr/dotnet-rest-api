using Microsoft.AspNetCore.Mvc;
using Sample.Rest.Api.Domain;
using System.Collections.Generic;

namespace Sample.Rest.Api.Controllers
{
    [ApiController]
    [Route("companies/{companyId}/employees")]
    public class CompanyEmployeesController : ControllerBase
    {
        private static readonly List<Company> _companies = new();

        [HttpGet, HttpHead]
        public IActionResult FindCompanies()
        {
            return Ok(_companies);
        }
    }
}
