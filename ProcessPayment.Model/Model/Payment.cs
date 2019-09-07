using System;
using ProcessPayment.Common;

namespace ProcessPayment.Model
{
	public class Payment
	{
		public int Id { get; internal set; }
		public string CreditCardNumber { get; internal set; }
		public string CardHolder { get; internal set; }
		public DateTime ExpirationDate { get; internal set; }
		public string SecurityCode { get; internal set; }
		public decimal Amount { get; internal set; }

		public int StateId { get; internal set; }
		public virtual PaymentState State { get; internal set; }

		public string Status { get; internal set; } = "";

		public void UpdateState(PaymentStatus paymentStatus)
		{
			StateId = (int)paymentStatus.State;
			Status = paymentStatus.Message;
		}
	}
}