using System;
using System.Threading;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using ProcessPayment.Model;

namespace ProcessPayment.Domain
{
	public class ProcessPaymentRequest : IRequest<ProcessPaymentRequest>
	{
		public int Id { get; set; }
		public string CreditCardNumber { get; set; }
		public string CardHolder { get; set; }
		public DateTime ExpirationDate { get; set; }
		public string SecurityCode { get; set; }
		public decimal Amount { get; set; }
		public string Status { get; set; }
	}

	public class ProcessPaymentRequestHandler : IRequestHandler<ProcessPaymentRequest, ProcessPaymentRequest>,
												IDisposable
	{
		readonly IPaymentsRepository paymentsRepository;
		readonly IGatewayProvider gatewayProvider;

		public ProcessPaymentRequestHandler(IPaymentsRepository paymentsRepository, IGatewayProvider gatewayProvider)
		{
			this.paymentsRepository = paymentsRepository;
			this.gatewayProvider = gatewayProvider;
		}

		/// <inheritdoc />
		public async Task<ProcessPaymentRequest> Handle(ProcessPaymentRequest request,
														CancellationToken cancellationToken)
		{
			var payment = request.Adapt<ProcessPaymentRequest, Payment>();

			IPaymentGateway paymentGateway = gatewayProvider.GetPaymentGateway(payment.Amount);

			var paymentStatus = await paymentGateway.ProcessPayment(payment);

			payment.UpdateState(paymentStatus);

			await paymentsRepository.SavePayment(payment, cancellationToken);

			payment.Adapt(request);

			return request;
		}

		/// <inheritdoc />
		public void Dispose()
		{
			paymentsRepository?.Dispose();
		}

		~ProcessPaymentRequestHandler()
		{
			Dispose();
		}
	}
}