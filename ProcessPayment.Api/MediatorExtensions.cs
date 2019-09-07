using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using FluentValidation;
using MediatR;

namespace ProcessPayment.Api
{
	public static class MediatorExtensions
	{
		public static List<Type> OpenTypes = new List<Type>
		{
			typeof(IRequestHandler<,>),
			typeof(IValidator<>),
		};

		public static ContainerBuilder RegisterMediatR(this ContainerBuilder builder,
														params Assembly[] assemblies)
		{
			builder.RegisterAssemblyTypes(typeof(IMediator)
					.GetTypeInfo().Assembly)
				.AsImplementedInterfaces();

			foreach (var openType in OpenTypes)
			{
				builder.RegisterAssemblyTypes(assemblies)
					.AsClosedTypesOf(openType)
					.AsImplementedInterfaces();
			}

			builder.Register<ServiceFactory>(ctx =>
			{
				var c = ctx.Resolve<IComponentContext>();
				return t => c.Resolve(t);
			});

			var typesInDomainAssembly = assemblies.SelectMany(ass => ass.GetTypes()).ToList();

			var typesOfMediatR = typesInDomainAssembly
				.Where(x => !x.IsAbstract && !x.IsInterface &&
							x.GetInterfaces().Any(i => OpenTypes.Select(t => t.Name).Contains(i.Name)))
				.ToList();

			builder.RegisterTypes(typesInDomainAssembly.Except(typesOfMediatR).ToArray())
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();

			return builder;
		}
	}
}