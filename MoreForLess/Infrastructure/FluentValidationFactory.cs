using System;
using FluentValidation;
using SimpleInjector;

namespace MoreForLess.Infrastructure
{
    public class FluentValidationFactory : IValidatorFactory
    {
        private readonly Container _container;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FluentValidationFactory"/> class.
        /// </summary>
        /// <param name="container">
        ///     The Simple Injector Container
        /// </param>
        public FluentValidationFactory(Container container)
        {
            this._container = container;
        }

        /// <summary>
        ///     Gets the validator for the specified type.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of validator.
        /// </typeparam>
        /// <returns>
        ///     Returns instance of type that implments <see cref="IValidator"/>
        /// </returns>
        public IValidator<T> GetValidator<T>()
        {
            return this._container.GetInstance<IValidator<T>>();
        }

        /// <summary>
        ///     Gets the validator for the specified type.
        /// </summary>
        /// <param name="type">
        ///     Type of validator.
        /// </param>
        /// <returns>
        ///     Returns instance of type that implments <see cref="IValidator"/>
        /// </returns>
        public IValidator GetValidator(Type type)
        {
            var validator = typeof(IValidator<>).MakeGenericType(type);

            try
            {
                return (IValidator)this._container.GetInstance(validator);
            }
            catch (ActivationException)
            {
                return null;
            }
        }
    }
}