using ProcessPayment.Common;

namespace ProcessPayment.Model
{
	public class PaymentState
	{
		/// <inheritdoc />
		internal PaymentState(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}

		public int Id { get; set; }
		public string Name { get; set; }

		public static implicit operator PaymentStateEnum(PaymentState dsr)
		{
			return (PaymentStateEnum) dsr.Id;
		}

		public static implicit operator PaymentState(PaymentStateEnum ds)
		{
			return new PaymentState((int)ds, ds.ToString());
		}
	}
}