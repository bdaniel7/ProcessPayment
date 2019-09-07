using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace ProcessPayment.Api
{
	public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		readonly IEnumerable<IValidator<TRequest>> validators;

		public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
		{
			this.validators = validators;
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
											RequestHandlerDelegate<TResponse> next)
		{
			var context = new ValidationContext(request);
			var failures = validators
				.Select(v => v.Validate(context))
				.SelectMany(result => result.Errors)
				.Where(f => f != null)
				.ToList();

			if (failures.Count > 0)
			{
				throw new ValidationException(failures);
			}

			return await next();
		}
	}
}