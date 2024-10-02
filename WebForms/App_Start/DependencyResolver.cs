using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace WebForms.App_Start
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public DependencyResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDependencyScope BeginScope()
        {
            return new DependencyScope(_serviceProvider.CreateScope());
        }

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _serviceProvider.GetServices(serviceType);
        }

        public void Dispose() { }
    }

    public class DependencyScope : IDependencyScope
    {
        private readonly IServiceScope _serviceScope;

        public DependencyScope(IServiceScope serviceScope)
        {
            _serviceScope = serviceScope;
        }

        public object GetService(Type serviceType)
        {
            return _serviceScope.ServiceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _serviceScope.ServiceProvider.GetServices(serviceType);
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }
    }
}