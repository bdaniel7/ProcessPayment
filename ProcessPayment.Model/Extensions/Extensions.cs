using System;
using System.Collections.Generic;
using System.Linq;
using ProcessPayment.Common;

namespace ProcessPayment.Model
{
	public static class Extensions
	{
		public static List<PaymentState> PaymentStateAsEntities()
		{
			return Enum.GetValues(typeof(PaymentStateEnum)).Cast<PaymentStateEnum>()
				.ToDictionary(kvp => (int) kvp, kpv => kpv.ToString())
				.Select(e => new PaymentState(e.Key, e.Value))
				.ToList();

		}
	}
}