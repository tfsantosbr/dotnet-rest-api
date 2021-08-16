using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Rest.Api.Domain.Companies;
using Sample.Rest.Api.Domain.Companies.Models;
using Sample.Rest.Api.Repositories;
using System;

namespace Sample.Rest.Api.Controllers
{
    [ApiController]
    [Route("companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly CompanyRepository _repository;

        public CompaniesController(CompanyRepository repository)
        {
            _repository = repository;
        }

        // Find Companies

        [HttpGet, HttpHead]
        public IActionResult FindCompanies([FromQuery] CompanyParameters parameters)
        {
            // Set IgnoreItems if Request is HEAD

            parameters.IgnoreItems = Request.Method == HttpMethods.Head;

            // Find paginated items

            var items = _repository.Find(parameters);

            // Set header meta-data

            Response.Headers.Add("X-Current-Page", items.CurrentPage.ToString());
            Response.Headers.Add("X-Total-Pages", items.TotalPages.ToString());
            Response.Headers.Add("X-Page-Size", items.PageSize.ToString());
            Response.Headers.Add("X-Total-Records", items.TotalRecords.ToString());

            // Return Ok (200) with resources found

            return Ok(items);
        }

        // Create Company

        [HttpPost]
        public IActionResult CreateCompany(CreateCompany request)
        {
            // Validate and return Unprocessable Entity (422) if errors are found

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return UnprocessableEntity(new { Code = "Name", Description = "Name is required" });
            }

            if (_repository.AnyByName(request.Name))
            {
                return UnprocessableEntity(new { Code = "Name", Description = $"Company with name '{request.Name}' already exists" });
            }

            // Create company domain

            var company = new Company(request.Name, request.CreatedAt);

            _repository.Add(company);

            // Convert do details DTO

            var details = new CompanyDetails
            {
                Id = company.Id,
                Name = company.Name,
                CreatedAt = $"{company.CreatedAt:yyyy-MM-dd}",
                Status = company.Status.ToString(),
            };

            // Send created response status (201) with URL and created resource

            return Created($"companies/{details.Id}", details);
        }

        // Get Company

        [HttpGet("{companyId}")]
        public IActionResult GetCompany(Guid companyId)
        {
            // Find company by ID

            var company = _repository.GetById(companyId);

            // If company not found return Not Found (404) with some message to help

            if (company is null)
            {
                return NotFound(new { Code = "Company", Description = "Company not found" });
            }

            // Convert do details DTO

            var details = new CompanyDetails
            {
                Id = company.Id,
                Name = company.Name,
                CreatedAt = $"{company.CreatedAt:yyyy-MM-dd}",
                Status = company.Status.ToString(),
            };

            // Return Ok (200) with the resource found

            return Ok(details);
        }

        // Update Company

        [HttpPut("{companyId}")]
        public IActionResult UpdateCompany(Guid companyId, UpdateCompany request)
        {
            // Find company by ID

            var company = _repository.GetById(companyId);

            // If company not found return Not Found (404) with some message to help

            if (company is null)
            {
                return NotFound(new { Code = "Company", Description = "Company not found" });
            }

            // Validate and return Unprocessable Entity (422) if errors are found

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return UnprocessableEntity(new { Code = "Name", Description = "Name is required" });
            }

            if (_repository.AnyByName(request.Name, company.Id))
            {
                return UnprocessableEntity(new { Code = "Name", Description = $"Company with name '{request.Name}' already exists" });
            }

            // Update company

            company.Update(request.Name);

            // Return success with no resource needed (204)

            return NoContent();
        }

        // Active Company

        [HttpPatch("{companyId}/activated")]
        public IActionResult ActiveCompany(Guid companyId)
        {
            // Find company by ID

            var company = _repository.GetById(companyId);

            // If company not found return Not Found (404) with some message to help

            if (company is null)
            {
                return NotFound(new { Code = "Company", Description = "Company not found" });
            }

            if (company.Status == CompanyStatus.Activated)
            {
                return UnprocessableEntity(new { Code = "Company", Description = "Company is already activated" });
            }

            // Update company

            company.Active();

            // Return success with no resource needed (204)

            return NoContent();
        }

        // Inactive Company

        [HttpPatch("{companyId}/inactivated")]
        public IActionResult InactiveCompany(Guid companyId)
        {
            // Find company by ID

            var company = _repository.GetById(companyId);

            // If company not found return Not Found (404) with some message to help

            if (company is null)
            {
                return NotFound(new { Code = "Company", Description = "Company not found" });
            }

            if (company.Status == CompanyStatus.Inactivated)
            {
                return UnprocessableEntity(new { Code = "Company", Description = "Company is already inactivated" });
            }

            // Update company

            company.Inactive();

            // Return success with no resource needed (204)

            return NoContent();
        }

        // Block Company

        [HttpPatch("{companyId}/blocked")]
        public IActionResult BlockCompany(Guid companyId)
        {
            // Find company by ID

            var company = _repository.GetById(companyId);

            // If company not found return Not Found (404) with some message to help

            if (company is null)
            {
                return NotFound(new { Code = "Company", Description = "Company not found" });
            }

            if (company.Status == CompanyStatus.Blocked)
            {
                return UnprocessableEntity(new { Code = "Company", Description = "Company is already blocked" });
            }

            // Update company

            company.Block();

            // Return success with no resource needed (204)

            return NoContent();
        }

        // Remove Company

        [HttpDelete("{companyId}")]
        public IActionResult RemoveCompany(Guid companyId)
        {
            // Find company by ID

            var company = _repository.GetById(companyId);

            // If company not found return Not Found (404) with some message to help

            if (company is null)
            {
                return NotFound(new { Code = "Company", Description = "Company not found" });
            }

            // Remove company

            _repository.Remove(company);

            // Return success with no resource needed (204)

            return NoContent();
        }
    }
}
