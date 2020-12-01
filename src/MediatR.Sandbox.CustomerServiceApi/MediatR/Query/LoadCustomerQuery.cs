using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR.Sandbox.CustomerServiceApi.Data;
using MediatR.Sandbox.CustomerServiceApi.DataTransferObjects;
using MediatR.Sandbox.CustomerServiceApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediatR.Sandbox.CustomerServiceApi.MediatR.Query
{
    public record LoadCustomerQuery(Guid CustomerId) : IRequest<CustomerDto?>;

    // ReSharper disable once UnusedType.Global
    public class LoadCustomerQueryValidation : AbstractValidator<LoadCustomerQuery>
    {
        public LoadCustomerQueryValidation()
        {
            RuleFor(query => query.CustomerId)
                .NotEqual(Guid.Empty);
        }
    }
    
    public class LoadCustomerQueryHandler : IRequestHandler<LoadCustomerQuery, CustomerDto?>
    {
        private readonly MediatRDbContext _dbContext;

        public LoadCustomerQueryHandler(MediatRDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CustomerDto?> Handle(LoadCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Set<Customer>()
                .SingleOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);

            if (customer is null) return null;

            return new CustomerDto(customer.Id, customer.Name);
        }
    }
}