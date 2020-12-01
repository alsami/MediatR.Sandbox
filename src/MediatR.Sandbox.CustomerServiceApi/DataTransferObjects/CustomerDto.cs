using System;

namespace MediatR.Sandbox.CustomerServiceApi.DataTransferObjects
{
    public class CustomerDto
    {
        public CustomerDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
        
        public Guid Id { get; }

        public string Name { get; }
    }
}