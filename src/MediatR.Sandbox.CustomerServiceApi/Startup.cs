using FluentValidation;
using MediatR.Sandbox.CustomerServiceApi.Data;
using MediatR.Sandbox.CustomerServiceApi.GRPC;
using MediatR.Sandbox.CustomerServiceApi.MediatR.Behaviors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Sandbox.CustomerServiceApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddGrpc(options => options.EnableDetailedErrors = true);

            services.AddDbContext<MediatRDbContext>(options => options.UseInMemoryDatabase("MediatRSandboxDb"), ServiceLifetime.Transient);
            
            // This line registers all dependencies of the following types:
            // - `IRequestPreprocessor<TRequest, TResponse>`
            // - `IRequestHandler<TRequest, TResponse>`
            // - `IRequestExceptionHandler<TRequest, TResponse, TException>`
            // - `IRequestExceptionAction<TRequest, TException>`
            // - `IRequestPostProcessor<TRequest, TResponse>`
            // - `INotificationHandler<TNotification>`
            services.AddMediatR(typeof(Startup).Assembly);
            // The pipeline behaviors have to be added manually
            // the extension above does not add them using assembly-scanning
            // the order of the registrations of generics is different, here it's first in - first out rather than last in - first out!
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            // This scans for all types of AbstractValidator<T>
            // and adds them to the container
            // So our validation may be executed in our ValidationBehavior<,>
            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<CustomerServiceGrpc>();
            });
        }
    }
}