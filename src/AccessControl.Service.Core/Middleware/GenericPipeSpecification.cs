using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using MassTransit;
using MassTransit.Configurators;
using MassTransit.PipeBuilders;
using MassTransit.PipeConfigurators;
using MassTransit.Pipeline;

namespace AccessControl.Service.Core.Middleware
{
    /// <summary>
    ///     Represents a generic specification for a single filter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericPipeSpecification<T> : IPipeSpecification<T>
        where T : class, PipeContext
    {
        private readonly IFilter<T> _filter;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericPipeSpecification{T}" /> class.
        /// </summary>
        /// <param name="filter">The filter to be added.</param>
        public GenericPipeSpecification(IFilter<T> filter)
        {
            Contract.Requires(filter != null);
            _filter = filter;
        }

        /// <summary>
        ///     Apply the specification to the builder
        /// </summary>
        /// <param name="builder">The pipe builder</param>
        public void Apply(IPipeBuilder<T> builder)
        {
            builder.AddFilter(_filter);
        }

        /// <summary>
        ///     Validate the configuration of this configurator, to make sure
        ///     that you haven't done silly mistakes.
        /// </summary>
        /// <returns />
        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}