using Sample.Rest.Api.Domain.Employees;
using Sample.Rest.Api.Domain.Employees.Models;
using Sample.Rest.Api.Extensions.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Rest.Api.Repositories
{
    public class EmployeeRepository
    {
        // Fields

        private static readonly List<Employee> _employees = SeedEmployees();

        // Public Methods

        public IPagedList<EmployeeItem> Find(Guid companyId, EmployeeParameters parameters)
        {
            var query = _employees.Where(e => e.CompanyId == companyId).AsQueryable();

            query = Filter(query, parameters);
            query = Order(query, parameters.OrderBy);

            var total = query.Count();

            var items = parameters.IgnoreItems ? null : query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(p => new EmployeeItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    Email = p.Email,
                    HiredIn = $"{p.HiredIn:yyyy-MM-dd}",
                });

            return new PagedList<EmployeeItem>(items, total, parameters.Page, parameters.PageSize);
        }

        public void Add(Employee employee)
        {
            _employees.Add(employee);
        }

        public bool AnyByEmail(string email, Guid? ignoredEmployeeId = null)
        {
            return _employees.Any(c => c.Email == email && c.Id != ignoredEmployeeId);
        }

        public Employee GetById(Guid companyId, Guid employeeId)
        {
            return _employees.FirstOrDefault(c => c.CompanyId == companyId && c.Id == employeeId);
        }

        public void Remove(Employee employee)
        {
            _employees.Remove(employee);
        }

        // Private Methods

        private static IQueryable<Employee> Order(IQueryable<Employee> query, string orderBy)
        {
            return orderBy switch
            {
                "name-asc" => query.OrderBy(c => c.Name),
                "name-desc" => query.OrderByDescending(c => c.Name),

                "hiredIn-asc" => query.OrderBy(c => c.HiredIn),
                "hiredIn-desc" => query.OrderByDescending(c => c.HiredIn),

                _ => query.OrderBy(c => c.Name),
            };
        }

        private static IQueryable<Employee> Filter(IQueryable<Employee> query, EmployeeParameters parameters)
        {
            if (parameters.HasQuery)
            {
                query = query.Where(c =>
                    c.Name.ToLowerInvariant().Contains(parameters.Query) ||
                    c.HiredIn.ToString("yyyy-MM-dd").Contains(parameters.Query) ||
                    c.Email.ToString().ToLowerInvariant().Contains(parameters.Query)
                    );
            }

            if (!string.IsNullOrWhiteSpace(parameters.Name))
            {
                query = query.Where(c => c.Name.ToLowerInvariant().StartsWith(parameters.Name));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Email))
            {
                query = query.Where(c => c.Email.ToLowerInvariant().StartsWith(parameters.Email));
            }

            if (parameters.HiredIn.HasValue)
            {
                query = query.Where(c => c.HiredIn == parameters.HiredIn.Value);
            }

            return query;
        }

        private static List<Employee> SeedEmployees()
        {
            var companyId = new Guid("0527a3a6-2cd3-4b43-8359-91783d061f1b");

            return new()
            {
                new Employee(companyId, "Iran Silva", "iran@email.com", new DateTime(2020, 1, 8), new Guid("0aa264c2-c748-48e1-a642-f4a07135cbf5")),
                new Employee(companyId, "Thaina Bastos", "thaina@email.com", new DateTime(2020, 2, 1), new Guid("55f1aa1c-fa0c-43a9-b6e1-5acb5a2d9740")),
                new Employee(companyId, "Dani Morelo", "dani@email.com", new DateTime(2020, 3, 9), new Guid("25a32fdf-1cc8-4aa8-ad95-35d555d8637b")),
                new Employee(companyId, "Naira Estrela", "naira@email.com", new DateTime(2020, 2, 13), new Guid("65bfa170-a92a-4eed-8c17-d5c8a198303d")),
                new Employee(companyId, "Tiago Santos", "tiago@email.com", new DateTime(2020, 2, 15), new Guid("4d7ac74a-e461-452f-a6bc-3771c8f9eeb0")),
            };
        }
    }
}
