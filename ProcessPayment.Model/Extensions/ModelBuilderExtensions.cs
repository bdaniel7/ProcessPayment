using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace ProcessPayment.Model
{
	public static class ModelBuilderExtensions
	{
		/// <summary>
		/// Extracts all types that implement IEntityTypeConfiguration&lt;T> and runs their Configure() method.
		/// </summary>
		/// <param name="modelBuilder"></param>
		/// <typeparam name="TContext"></typeparam>
		public static void ApplyConfigurations<TContext>(this ModelBuilder modelBuilder)
			where TContext : DbContext
		{
			var contextNamespace = typeof(TContext).Namespace;

			// Types that do entity mapping
			var mappingTypes = typeof(TContext)
				.GetTypeInfo()
				.Assembly
				.GetTypes()
				.Where(
					x => x.GetInterfaces().Any(y => y.GetTypeInfo().IsGenericType
													&&
													y.GetGenericTypeDefinition() ==
													typeof(IEntityTypeConfiguration<>))
				)
				.Where(t => t.Namespace.Equals(contextNamespace));

			// Get the generic Entity method of the ModelBuilder type
			var entityMethod = typeof(ModelBuilder).GetMethods()
				.Single(x => x.Name == "Entity" &&
							x.IsGenericMethod &&
							x.ReturnType.Name == "EntityTypeBuilder`1");

			foreach (var mappingType in mappingTypes)
			{
				// Get the type of entity to be mapped
				var genericTypeArg = mappingType.GetInterfaces().Single().GenericTypeArguments.Single();

				// Get the method builder.Entity<TEntity>
				var genericEntityMethod = entityMethod.MakeGenericMethod(genericTypeArg);

				// Invoke builder.Entity<TEntity> to get a builder for the entity to be mapped
				var entityBuilder = genericEntityMethod.Invoke(modelBuilder, null);

				// Create the mapping type and do the mapping
				var entityTypeConfiguration = Activator.CreateInstance(mappingType);

				entityTypeConfiguration
					.GetType()
					.GetMethod("Configure")
					.Invoke(entityTypeConfiguration, new[] {entityBuilder});
			}
		}
	}
}