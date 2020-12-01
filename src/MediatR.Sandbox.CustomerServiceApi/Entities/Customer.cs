using System;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace MediatR.Sandbox.CustomerServiceApi.Entities
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Customer
    {
        private Customer()
        {
        }

        public Customer(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;
    }
}