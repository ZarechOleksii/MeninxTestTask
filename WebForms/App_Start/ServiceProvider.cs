using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace WebForms.App_Start
{
    public class ServiceProviderWrapper : IServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderWrapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, serviceType);
            }
            catch (InvalidOperationException)
            {
                return Activator.CreateInstance(serviceType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
            }
        }
    }
}