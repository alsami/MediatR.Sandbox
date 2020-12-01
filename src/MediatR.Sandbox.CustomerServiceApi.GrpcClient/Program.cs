using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using MediatR.Sandbox.CustomerServiceApi.GrpcServices;

namespace MediatR.Sandbox.CustomerServiceApi.GrpcClient
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new CustomerServiceGrpc.CustomerServiceGrpcClient(channel);
            
            // Create a new customer
            var createCustomerMessage = new CreateCustomerGrpcMessage()
            {
                Name = Guid.NewGuid().ToString()
            };
            var customerIdResponse = await client.CreateCustomerAsync(createCustomerMessage);
            
            // load the customer using the previous id
            var loadCustomerMessage = new LoadCustomerGrpcMessage
            {
                CustomerId = customerIdResponse.Id
            };
            var customer = await client.LoadCustomerAsync(loadCustomerMessage);
            
            // delete the existing customer
            var deleteCustomerMessage = new DeleteCustomerGrpcMessage
            {
                Id = customer.Id
            };
            await client.DeleteCustomerAsync(deleteCustomerMessage);
        }
    }
}