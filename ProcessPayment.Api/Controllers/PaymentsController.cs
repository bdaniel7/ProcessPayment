using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProcessPayment.Domain;

namespace ProcessPayment.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentsController : ControllerBase
	{
		readonly IMediator mediator;

		public PaymentsController(IMediator mediator)
		{
			this.mediator = mediator;
		}

		[HttpGet]
		[Route("/")]
		public IActionResult GetHi()
		{
			return Ok("Hey");
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var request = new GetPaymentsRequest();

			var payments = await mediator.Send(request);

			return Ok(payments);
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] ProcessPaymentRequest request)
		{
			await mediator.Send(request);

			return CreatedAtAction("Get",
				new {id = request.Id},
				request);
		}
	}
}