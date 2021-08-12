using Sample.Rest.Api.Domain;
using Sample.Rest.Api.Extensions.Pagination;
using Sample.Rest.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Rest.Api.Repositories
{
    public class CompanyRepository
    {
        // Fields

        private static readonly List<Company> _companies = SeedCompanies();

        // Public Methods

        public IPagedList<CompanyItem> Find(CompanyParameters parameters)
        {
            var query = _companies.AsQueryable();

            query = Filter(query, parameters);
            query = Order(query, parameters.OrderBy);

            var total = query.Count();

            var items = parameters.IgnoreItems ? null : query
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(p => new CompanyItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    CreatedAt = $"{p.CreatedAt:yyyy-MM-dd}",
                    Status = p.Status.ToString()
                });

            return new PagedList<CompanyItem>(items, total, parameters.Page, parameters.PageSize);
        }

        public void Add(Company company)
        {
            _companies.Add(company);
        }

        public bool AnyByName(string name)
        {
            return _companies.Any(c => c.Name == name);
        }

        public Company GetById(Guid companyId)
        {
            return _companies.FirstOrDefault(c => c.Id == companyId);
        }
        
        public void Remove(Company company)
        {
            _companies.Remove(company);
        }

        // Private Methods

        private static IQueryable<Company> Order(IQueryable<Company> query, string orderBy)
        {
            return orderBy switch
            {
                "name-asc" => query.OrderBy(c => c.Name),
                "name-desc" => query.OrderByDescending(c => c.Name),

                "createdAt-asc" => query.OrderBy(c => c.CreatedAt),
                "createdAt-desc" => query.OrderByDescending(c => c.CreatedAt),

                "status-asc" => query.OrderBy(c => c.Status),
                "status-desc" => query.OrderByDescending(c => c.Status),

                _ => query.OrderBy(c => c.Name),
            };
        }

        private static IQueryable<Company> Filter(IQueryable<Company> query, CompanyParameters parameters)
        {
            if (parameters.HasQuery)
            {
                query = query.Where(c =>
                    c.Name.ToLowerInvariant().Contains(parameters.Query) ||
                    c.CreatedAt.ToString("yyyy-MM-dd").Contains(parameters.Query) ||
                    c.Status.ToString().ToLowerInvariant().Contains(parameters.Query)
                    );
            }

            if (!string.IsNullOrWhiteSpace(parameters.Name))
            {
                query = query.Where(c => c.Name.ToLowerInvariant().StartsWith(parameters.Name));
            }

            if (parameters.CreatedAt.HasValue)
            {
                query = query.Where(c => c.CreatedAt == parameters.CreatedAt.Value);
            }

            if (parameters.Status.HasValue)
            {
                query = query.Where(c => c.Status == parameters.Status.Value);
            }

            return query;
        }

        private static List<Company> SeedCompanies()
        {
            var activatedCompany1 = new Company("Walt Mart", DateTime.Parse("2020-03-01"), new Guid("0527a3a6-2cd3-4b43-8359-91783d061f1b"));
            activatedCompany1.Active();

            var activatedCompany2 = new Company("Disney", DateTime.Parse("2020-03-13"), new Guid("ad3f11a9-6201-4135-b61b-ab8083cf32a9"));
            activatedCompany2.Active();

            var blockedCompany = new Company("Mc Donalds", DateTime.Parse("2020-03-16"), new Guid("39932bdc-a73f-4fba-89b1-a68f647c1ea9"));
            blockedCompany.Block();

            return new()
            {
                activatedCompany1,
                activatedCompany2,
                blockedCompany,
                new Company("Burguer King", DateTime.Parse("2019-05-20")),
                new Company("Sony", DateTime.Parse("2019-02-19")),
                new Company("Nintendo", DateTime.Parse("2019-03-17")),
                new Company("Microsoft", DateTime.Parse("2020-03-16")),
                new Company("Smart Fit", DateTime.Parse("2020-03-05")),
                new Company("Corsair", DateTime.Parse("2020-07-16")),
                new Company("Logitech", DateTime.Parse("2019-03-16")),
                new Company("Samsung", DateTime.Parse("2020-07-05")),
                new Company("LG", DateTime.Parse("2020-03-16")),
                new Company("Apple", DateTime.Parse("2020-03-16")),
                new Company("Xiaomi", DateTime.Parse("2019-03-16")),
                new Company("Google", DateTime.Parse("2020-03-05")),
                new Company("Amazon", DateTime.Parse("2019-07-16")),
                new Company("Mercado Livre", DateTime.Parse("2020-03-05")),
                new Company("Redfit", DateTime.Parse("2019-07-05")),
            };
        }
    }
}
