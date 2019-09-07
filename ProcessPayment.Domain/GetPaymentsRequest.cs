using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ProcessPayment.Domain
{
	public class PaymentDto
	{
		public int Id { get; internal set; }
		public decimal Amount { get; internal set; }
		public string StateName { get; internal set; }
		public string Status { get; internal set; } = "";
	}

	public class GetPaymentsRequest : IRequest<List<PaymentDto>>
	{

	}

	public class GetPaymentsRequestHandler : IRequestHandler<GetPaymentsRequest, List<PaymentDto>>, IDisposable
	{
		readonly IPaymentsRepository paymentsRepository;

		public GetPaymentsRequestHandler(IPaymentsRepository paymentsRepository)
		{
			this.paymentsRepository = paymentsRepository;
		}

		/// <inheritdoc />
		public async Task<List<PaymentDto>> Handle(GetPaymentsRequest request, CancellationToken cancellationToken)
		{
			return await paymentsRepository.GetAllPayments(cancellationToken);
		}

		/// <inheritdoc />
		public void Dispose()
		{
			paymentsRepository?.Dispose();
		}

		~GetPaymentsRequestHandler()
		{
			Dispose();
		}
	}
}