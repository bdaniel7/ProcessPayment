using System;
using System.Runtime.Serialization;

namespace ProcessPayment.Common
{
	[Serializable]
	public class PaymentNotProcessedException : Exception
	{
		public PaymentNotProcessedException() { }
		public PaymentNotProcessedException(string message) : base(message) { }
		public PaymentNotProcessedException(string message, Exception inner) : base(message, inner) { }

		protected PaymentNotProcessedException(
			SerializationInfo info,
			StreamingContext context) : base(info, context) { }
	}
}