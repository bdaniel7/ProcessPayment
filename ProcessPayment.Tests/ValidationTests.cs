using System;
using FluentValidation.TestHelper;
using NUnit.Framework;
using ProcessPayment.Domain;

namespace ProcessPayment.Tests
{
	[TestFixture]
	public class ValidationTests
	{
		ProcessPaymentRequestValidator pv;

		[SetUp]
		public void Setup()
		{
			pv = new ProcessPaymentRequestValidator();
		}

		[Test]
		public void PaymentIsValid()
		{
			var ppr = new ProcessPaymentRequest()
			{
				Amount = 2.4M,
				CardHolder = "qazwsxedcrfvt",
				CreditCardNumber = "1qazxsw2#EDCr",
				ExpirationDate = DateTime.Today.AddYears(3),
				SecurityCode = "098",
			};

			var vr = pv.Validate(ppr);
			Assert.IsTrue(vr.IsValid);
		}

		[Test]
		public void AmountShouldBeGreaterThanZero()
		{
			var ppr = new ProcessPaymentRequest()
			{
				CreditCardNumber = "1qazxsw2#EDCr",
				Amount = 0,
			};

			pv.ShouldHaveValidationErrorFor(r => r.Amount, ppr);
		}

		[Test]
		public void CardHolderNameShouldBePresent()
		{
			var ppr = new ProcessPaymentRequest()
			{
				CreditCardNumber = "1qazxsw2#EDCr",
			};

			pv.ShouldHaveValidationErrorFor(r => r.CardHolder, ppr);
		}

		[Test]
		public void CrediCardNumberShouldBeValid()
		{
			var ppr = new ProcessPaymentRequest()
			{
				CreditCardNumber = "1qazxsw2#EDC",
			};

			pv.ShouldHaveValidationErrorFor(r => r.CreditCardNumber, ppr);
		}

		[Test]
		public void ExpirationDateShouldBeInTheFuture()
		{
			var ppr = new ProcessPaymentRequest()
			{
				CreditCardNumber = "1qazxsw2#EDC",
				ExpirationDate = DateTime.Today.AddMonths(-3),
			};

			pv.ShouldHaveValidationErrorFor(r => r.ExpirationDate, ppr);
		}
	}
}