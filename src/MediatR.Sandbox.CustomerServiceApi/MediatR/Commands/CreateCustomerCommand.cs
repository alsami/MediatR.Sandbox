using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR.Sandbox.CustomerServiceApi.Data;
using MediatR.Sandbox.CustomerServiceApi.Entities;

namespace MediatR.Sandbox.CustomerServiceApi.MediatR.Commands
{
    public record CreateCustomerCommand(string Name) : IRequest<Guid>;

    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(command => command.Name)
                .MinimumLength(3)
                .NotEmpty();
        }
    }
    
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly MediatRDbContext _context;

        public CreateCustomerCommandHandler(MediatRDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer(Guid.NewGuid(), request.Name);

            await _context.Set<Customer>().AddAsync(customer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return customer.Id;
        }
    }
}