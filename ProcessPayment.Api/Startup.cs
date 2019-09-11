using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProcessPayment.Domain;
using ProcessPayment.Model;
using Serilog;

namespace ProcessPayment.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		public IContainer Container { get; private set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services
				.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
				.AddFluentValidation(cfg =>
				{
					cfg.RegisterValidatorsFromAssemblyContaining<ProcessPaymentRequest>();
					cfg.ImplicitlyValidateChildProperties = true;
				});

			services.AddMvcCore()
				.AddJsonFormatters()
				.AddDataAnnotations()
				.AddApiExplorer()
				.AddJsonOptions(o =>
				{
					o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
					o.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
					o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					o.SerializerSettings.Formatting = Formatting.Indented;
					o.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
					o.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
				});

			services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

			var requestAssembly = typeof(ProcessPaymentRequest).Assembly;
			var modelAssembly = typeof(PaymentDbContext).Assembly;
			var apiAssembly = typeof(PaymentsController).Assembly;

			var connectionString = Configuration.GetConnectionString("ProcessPayment");

			services.AddDbContext<PaymentDbContext>(options =>
			{
				options.UseSqlServer(connectionString,
					sql => sql.MigrationsAssembly(modelAssembly.GetName().Name)
				).EnableSensitiveDataLogging();
			}).AddDbContextPool<PaymentDbContext>(options =>
			{
				options.UseSqlServer(connectionString);
			});

			var builder = new ContainerBuilder();

			builder.RegisterMediatR(requestAssembly);

			builder.RegisterAssemblyTypes(requestAssembly,
											apiAssembly,
											modelAssembly
										)
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();

			builder.Register(ctx => new CheapPaymentGateway(ctx.Resolve<ExpensivePaymentGateway>()));
			builder.Register(ctx => new ExpensivePaymentGateway(ctx.Resolve<PremiumPaymentGateway>()));
			builder.Register(ctx => new PremiumPaymentGateway(null));

			builder.Populate(services);

			Container = builder.Build();

			return new AutofacServiceProvider(Container);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseMiddleware<ErrorHandlerMiddleware>();
			app.UseMvc();
		}
	}
}