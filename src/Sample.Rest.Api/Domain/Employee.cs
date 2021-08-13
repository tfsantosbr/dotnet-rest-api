using System;

namespace Sample.Rest.Api.Domain
{
    public class Employee
    {
        public Employee(Guid companyId, string name, string email, DateTime hiredIn, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            CompanyId = companyId;
            Name = name;
            Email = email;
            HiredIn = hiredIn;
        }

        public Guid Id { get; private set; }
        public Guid CompanyId { get; private set; }
        public DateTime HiredIn { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }

        public void Update(string name, string email, DateTime hiredIn)
        {
            Name = name;
            Email = email;
            HiredIn = hiredIn;
        }
    }
}
