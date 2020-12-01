using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR.Sandbox.CustomerServiceApi.Data;
using MediatR.Sandbox.CustomerServiceApi.DataTransferObjects;
using MediatR.Sandbox.CustomerServiceApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MediatR.Sandbox.CustomerServiceApi.Tests
{
    public class CustomerControllerIntegrationTests : IAsyncLifetime, IClassFixture<WebApplicationFixture>
    {
        private readonly WebApplicationFixture _webApplicationFixture;
        private readonly Guid _testingCustomerId = Guid.NewGuid();

        public CustomerControllerIntegrationTests(WebApplicationFixture webApplicationFixture)
        {
            _webApplicationFixture = webApplicationFixture;
        }

        [Fact]
        public async Task LoadCustomer_Succeeds()
        {
            // create the customer on a lower-level
            await CreateTestCustomer(_testingCustomerId);

            var client = _webApplicationFixture.CreateClient();
            var response = await client.GetAsync($"api/customers/{_testingCustomerId}");
            
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async Task CreateCustomer_Succeeds()
        {
            // create the customer on a lower-level
            var createCustomer = new CreateCustomerDto
            {
                Name = Guid.NewGuid().ToString()
            };

            var client = _webApplicationFixture.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(createCustomer), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/customers/", content);
            
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async Task DeleteCustomer_Succeeds()
        {
            // create the customer on a lower-level
            await CreateTestCustomer(_testingCustomerId);

            var client = _webApplicationFixture.CreateClient();
            var response = await client.DeleteAsync($"api/customers/{_testingCustomerId}");
            
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        public Task InitializeAsync() => DeleteCustomer(_testingCustomerId);

        public Task DisposeAsync() => DeleteCustomer(_testingCustomerId);

        private async Task DeleteCustomer(Guid id)
        {
            using var scope = _webApplicationFixture.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<MediatRDbContext>();
            var existingCustomer = await context.Set<Customer>().SingleOrDefaultAsync(c => c.Id == id);

            if (existingCustomer is null)
            {
                return;
            }
            
            context.Set<Customer>().Remove(existingCustomer);
            await context.SaveChangesAsync();
        }

        private async Task CreateTestCustomer(Guid id)
        {
            var customer = new Customer(id, Guid.NewGuid().ToString());

            using var scope = _webApplicationFixture.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<MediatRDbContext>();
            await context.Set<Customer>().AddAsync(customer);
            await context.SaveChangesAsync();
        }
    }
}