// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="GoldenEagle.Desktop.Framwork development team">
//   Copyright (c) 2008 - 2012 GoldenEagle.Desktop.Framwork development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GoldenEagle.Desktop.Framwork.Services
{
    using System;

    /// <summary>
    /// Base class for all services in GoldenEagle.Desktop.Framwork.
    /// </summary>
    public abstract class ServiceBase : IServiceProvider
    {
        #region Methods
        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="serviceType"/>.-or- null if there is no service object of type <paramref name="serviceType"/>.</returns>
        public object GetService(Type serviceType)
        {
            return Catel.IoC.ServiceLocator.Default.ResolveType(serviceType);
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the service.</typeparam>
        /// <returns>The service instance.</returns>
        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }
        #endregion
    }
}