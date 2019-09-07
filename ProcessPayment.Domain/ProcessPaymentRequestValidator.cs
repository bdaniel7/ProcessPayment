using System;
using FluentValidation;

namespace ProcessPayment.Domain
{
	public class ProcessPaymentRequestValidator : AbstractValidator<ProcessPaymentRequest>
	{
		public ProcessPaymentRequestValidator()
		{
			RuleFor(p => p.CardHolder)
				.NotNull()
				.NotEmpty().WithMessage("The card holder is mandatory!");

			RuleFor(p => p.ExpirationDate)
				.NotNull()
				.NotEmpty()
				.Must(BeInTheFuture)
				.WithMessage("The expiration date must not be in the past!");

			RuleFor(p => p.CreditCardNumber)
				.NotNull()
				.NotEmpty().WithMessage("The credit card number is empty!")
				//.CreditCard()
				.Must(BeAValidCCN)
				.WithMessage("The credit card number is not valid!");
			;

			RuleFor(p => p.SecurityCode)
				.MaximumLength(3).WithMessage("The security code must have exactly 3 digits")
				.MinimumLength(3).WithMessage("The security code must have exactly 3 digits")
				.When(c => !string.IsNullOrEmpty(c.SecurityCode))
				;

			RuleFor(p => p.Amount)
				.NotEmpty()
				.NotNull()
				.GreaterThanOrEqualTo(1)
				.WithMessage("The amount must not be zero!");
		}

		bool BeAValidCCN(string ccn)
		{
			return ccn.Length == 13;
		}

		bool BeInTheFuture(DateTime d)
		{
			return d > DateTime.Today.AddHours(24);
		}
	}
}