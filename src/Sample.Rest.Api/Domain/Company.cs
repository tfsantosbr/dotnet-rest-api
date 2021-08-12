using System;

namespace Sample.Rest.Api.Domain
{
    public class Company
    {
        public Company(string name, DateTime createdAt, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            Name = name;
            CreatedAt = createdAt;
            Status = CompanyStatus.Inactive;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public CompanyStatus Status { get; private set; }

        public void Update(string name)
        {
            Name = name;
        }

        public void Active() => Status = CompanyStatus.Active;
        public void Inactive() => Status = CompanyStatus.Inactive;
        public void Block() => Status = CompanyStatus.Blocked;
    }

    public enum CompanyStatus : short
    {
        Active = 1,
        Inactive = 2,
        Blocked = 3
    }
}
