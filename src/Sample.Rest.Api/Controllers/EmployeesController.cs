using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.Rest.Api.Domain;
using Sample.Rest.Api.Models;
using Sample.Rest.Api.Repositories;
using System;

namespace Sample.Rest.Api.Controllers
{
    [ApiController]
    [Route("companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly CompanyRepository _companyRepository;

        public EmployeesController(EmployeeRepository employeeRepository, CompanyRepository companyRepository)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
        }

        // Find Employees

        [HttpGet, HttpHead]
        public IActionResult FindEmployees(Guid companyId, [FromQuery] EmployeeParameters parameters)
        {
            // Validate if company exists

            if (!_companyRepository.AnyById(companyId))
            {
                return NotFound(new { Code = "Company", Description = "Company not found" });
            }

            // Set IgnoreItems if Request is HEAD

            parameters.IgnoreItems = Request.Method == HttpMethods.Head;

            // Find paginated items

            var items = _employeeRepository.Find(companyId, parameters);

            // Set header meta-data

            Response.Headers.Add("X-Current-Page", items.CurrentPage.ToString());
            Response.Headers.Add("X-Total-Pages", items.TotalPages.ToString());
            Response.Headers.Add("X-Page-Size", items.PageSize.ToString());
            Response.Headers.Add("X-Total-Records", items.TotalRecords.ToString());

            // Return Ok (200) with resources found

            return Ok(items);
        }

        // Create Employee

        [HttpPost]
        public IActionResult CreateEmployee(Guid companyId, CreateEmployee request)
        {
            // Validate if company exists

            if (!_companyRepository.AnyById(companyId))
            {
                return NotFound(new { Code = "Company", Description = "Company not found" });
            }

            // Validate and return Unprocessable Entity (422) if errors are found

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return UnprocessableEntity(new { Code = "Name", Description = "Name is required" });
            }

            if (_employeeRepository.AnyByEmail(request.Email))
            {
                return UnprocessableEntity(new { Code = "Email", Description = $"Employee with e-mail '{request.Email}' already exists" });
            }

            // Create employee domain

            var employee = new Employee(companyId, request.Name, request.Email, request.HiredIn);

            _employeeRepository.Add(employee);

            // Convert do details DTO

            var details = new EmployeeDetails
            {
                Id = employee.Id,
                CompanyId = employee.CompanyId,
                Name = employee.Name,
                Email = employee.Email,
                HiredIn = $"{employee.HiredIn:yyyy-MM-dd}",
            };

            // Send created response status (201) with URL and created resource

            return Created($"employees/{details.Id}", details);
        }

        // Get Employee

        [HttpGet("{employeeId}")]
        public IActionResult GetEmployee(Guid companyId, Guid employeeId)
        {
            // Validate if company exists

            if (!_companyRepository.AnyById(companyId))
            {
                return NotFound(new { Code = "Company", Description = "Company not found" });
            }

            // Find employee by ID

            var employee = _employeeRepository.GetById(companyId, employeeId);

            // If employee not found return Not Found (404) with some message to help

            if (employee is null)
            {
                return NotFound(new { Code = "Employee", Description = "Employee not found" });
            }

            // Convert do details DTO

            var details = new EmployeeDetails
            {
                Id = employee.Id,
                CompanyId = employee.CompanyId,
                Name = employee.Name,
                Email = employee.Email,
                HiredIn = $"{employee.HiredIn:yyyy-MM-dd}",
            };

            // Return Ok (200) with the resource found

            return Ok(details);
        }

        // Update Employee

        [HttpPut("{employeeId}")]
        public IActionResult UpdateEmployee(Guid companyId, Guid employeeId, UpdateEmployee request)
        {
            // Validate if company exists

            if (!_companyRepository.AnyById(companyId))
            {
                return NotFound(new { Code = "Company", Description = "Company not found" });
            }

            // Find employee by ID

            var employee = _employeeRepository.GetById(companyId, employeeId);

            // If employee not found return Not Found (404) with some message to help

            if (employee is null)
            {
                return NotFound(new { Code = "Employee", Description = "Employee not found" });
            }

            // Validate and return Unprocessable Entity (422) if errors are found

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return UnprocessableEntity(new { Code = "Name", Description = "Name is required" });
            }

            if (_employeeRepository.AnyByEmail(request.Email))
            {
                return UnprocessableEntity(new { Code = "Email", Description = $"Employee with e-mail '{request.Email}' already exists" });
            }

            // Update employee

            employee.Update(request.Name, request.Email, request.HiredIn);

            // Return success with no resource needed (204)

            return NoContent();
        }

        // Remove Employee

        [HttpDelete("{employeeId}")]
        public IActionResult RemoveEmployee(Guid companyId, Guid employeeId)
        {
            // Validate if company exists

            if (!_companyRepository.AnyById(companyId))
            {
                return NotFound(new { Code = "Company", Description = "Company not found" });
            }

            // Find employee by ID

            var employee = _employeeRepository.GetById(companyId, employeeId);

            // If employee not found return Not Found (404) with some message to help

            if (employee is null)
            {
                return NotFound(new { Code = "Employee", Description = "Employee not found" });
            }

            // Remove employee

            _employeeRepository.Remove(employee);

            // Return success with no resource needed (204)

            return NoContent();
        }
    }
}
