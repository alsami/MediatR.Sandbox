using System;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR.Sandbox.CustomerServiceApi.GrpcServices;
using MediatR.Sandbox.CustomerServiceApi.MediatR.Commands;
using MediatR.Sandbox.CustomerServiceApi.MediatR.Query;

namespace MediatR.Sandbox.CustomerServiceApi.GRPC
{
    public class CustomerServiceGrpc : GrpcServices.CustomerServiceGrpc.CustomerServiceGrpcBase
    {
        private readonly IMediator _mediator;

        public CustomerServiceGrpc(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<CustomerGrpcMessage> LoadCustomer(LoadCustomerGrpcMessage request, ServerCallContext context)
        {
            var command = new LoadCustomerQuery(new Guid(request.CustomerId.ToByteArray()));

            var customer = await _mediator.Send(command);

            return new CustomerGrpcMessage
            {
                Id = ByteString.CopyFrom(customer!.Id.ToByteArray()),
                Name = customer.Name
            };
        }

        public override async Task<CustomerIdGrpcMessage> CreateCustomer(CreateCustomerGrpcMessage request, ServerCallContext context)
        {
            var createCustomerCommand = new CreateCustomerCommand(request.Name);
            var id = await _mediator.Send(createCustomerCommand);

            return new CustomerIdGrpcMessage
            {
                Id = ByteString.CopyFrom(id.ToByteArray())
            };
        }

        public override async Task<Empty> DeleteCustomer(DeleteCustomerGrpcMessage request, ServerCallContext context)
        {
            var command = new DeleteCustomerCommand(new Guid(request.Id.ToByteArray()));
            await _mediator.Send(command);

            return new Empty();
        }
    }
}