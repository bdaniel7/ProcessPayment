using System;
using Polly;
using Polly.Retry;
using Polly.Wrap;
using ProcessPayment.Common;
using Serilog;

namespace ProcessPayment.Domain
{
	public static class RetryPolicies
	{
		public static AsyncRetryPolicy PremiumGatewayRetryPolicy { get; }
			= Policy.Handle<PaymentNotProcessedException>()
				.WaitAndRetryAsync(
					3,
					retry => TimeSpan.FromSeconds(5),
					(exception, timeSpan, retryCount, context) =>
					{
						Log.Error("Received an error on PremiumGateway. Retrying...");
					});

		public static AsyncRetryPolicy CheapGatewayRetryPolicy { get; }
			= Policy.Handle<PaymentGatewayNotAvailableException>()
				.WaitAndRetryAsync(
					1,
					retry => TimeSpan.FromSeconds(5),
					(exception, timeSpan, retryCount, context) =>
					{
						Log.Error("Received an error on ExpensiveGateway. Retrying...");
					});

	}
}