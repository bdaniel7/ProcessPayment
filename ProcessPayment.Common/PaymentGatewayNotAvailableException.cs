using System;
using System.Runtime.Serialization;

namespace ProcessPayment.Common
{
	[Serializable]
	public class PaymentGatewayNotAvailableException : Exception
	{
		public PaymentGatewayNotAvailableException() { }
		public PaymentGatewayNotAvailableException(string message) : base(message) { }
		public PaymentGatewayNotAvailableException(string message, Exception inner) : base(message, inner) { }

		protected PaymentGatewayNotAvailableException(
			SerializationInfo info,
			StreamingContext context) : base(info, context) { }
	}
}