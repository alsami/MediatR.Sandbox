using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR.Sandbox.CustomerServiceApi.Data;
using MediatR.Sandbox.CustomerServiceApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediatR.Sandbox.CustomerServiceApi.MediatR.Commands
{
    public record DeleteCustomerCommand(Guid CustomerId) : IRequest;

    public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(command => command.CustomerId)
                .NotEqual(Guid.Empty);
        }
    }
    
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly MediatRDbContext _context;

        public DeleteCustomerCommandHandler(MediatRDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Set<Customer>()
                .SingleOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);

            if (customer is null)
            {
                return Unit.Value;
            }

            _context.Set<Customer>().Remove(customer);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}