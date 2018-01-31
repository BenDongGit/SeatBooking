namespace SeatBookingSystem.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using Ninject;

    using SeatBookingSystem.Entities;
    using SeatBookingSystem.Helper;
   
    /// <summary>
    /// The Ninject dependency resolver
    /// </summary>
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectDependencyResolver"/> class
        /// </summary>
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns>The service</returns>
        public object GetService(Type serviceType)
        {
            return kernel.Get(serviceType);
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns>The service collections</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        /// <summary>
        /// Adds the bindings
        /// </summary>
        private void AddBindings()
        {
            this.kernel.Bind<IDbContextHelper<SeatBookingContext>>()
                       .To<DbContextHelper<SeatBookingContext>>();
        }
    }
}