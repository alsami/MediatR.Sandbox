using System;
using System.Threading.Tasks;
using MediatR.Sandbox.CustomerServiceApi.DataTransferObjects;
using MediatR.Sandbox.CustomerServiceApi.MediatR.Commands;
using MediatR.Sandbox.CustomerServiceApi.MediatR.Query;
using Microsoft.AspNetCore.Mvc;

namespace MediatR.Sandbox.CustomerServiceApi.Rest
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<CustomerDto?>> FindAsync(Guid id)
        {
            var customer = await _mediator.Send(new LoadCustomerQuery(id));

            if (customer is null) return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerDto createCustomerDto)
        {
            var command = new CreateCustomerCommand(createCustomerDto.Name);
            var customerId = await _mediator.Send(command);

            return Ok(customerId);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var command = new DeleteCustomerCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}