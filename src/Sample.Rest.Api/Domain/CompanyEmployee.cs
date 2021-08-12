using System;

namespace Sample.Rest.Api.Domain
{
    public class CompanyEmployee
    {
        public CompanyEmployee(Guid companyId, string name, string email, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            CompanyId = companyId;
            Name = name;
            Email = email;
        }

        public Guid Id { get; private set; }
        public Guid CompanyId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
    }
}
