using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace MediatR.Sandbox.CustomerServiceApi.MediatR.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validator is null)
            {
                return await next();
            }

            var validationContext = new ValidationContext<TRequest>(request);
            var validationResult = await _validator.ValidateAsync(validationContext, cancellationToken);

            if (validationResult.IsValid)
            {
                return await next();
            }

            throw new ValidationException(validationResult.Errors);
        }
    }
}