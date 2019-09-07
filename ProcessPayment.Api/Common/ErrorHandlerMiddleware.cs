using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ProcessPayment.Api
{
	public class ErrorHandlerMiddleware
	{
		readonly RequestDelegate     nextDelegate;
		readonly IHostingEnvironment environment;

		public ErrorHandlerMiddleware(RequestDelegate nextDelegate, IHostingEnvironment environment)
		{
			this.nextDelegate = nextDelegate;
			this.environment = environment;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await nextDelegate(context);
			}
			catch (Exception exception)
			{
				await handleErrorAsync(context, exception);
			}
		}

		async Task handleErrorAsync(HttpContext context, Exception exception)
		{
			var errorCode = "error";
			var statusCode = HttpStatusCode.BadRequest;

			string errorMessage = exception.Message;

			var response = new { code = errorCode, message = errorMessage };

			var payload = JsonConvert.SerializeObject(response);

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)statusCode;

			await context.Response.WriteAsync(payload);
		}
	}
}