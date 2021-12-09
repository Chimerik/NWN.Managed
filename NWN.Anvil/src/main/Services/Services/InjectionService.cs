using System;
using System.Collections.Generic;
using System.Reflection;
using Anvil.Plugins;
using LightInject;

namespace Anvil.Services
{
  [ServiceBinding(typeof(InjectionService))]
  [ServiceBindingOptions(InternalBindingPriority.API)]
  public sealed class InjectionService : IDisposable
  {
    private readonly IServiceContainer container;
    private readonly List<PropertyInfo> injectedStaticProperties = new List<PropertyInfo>();

    public InjectionService(IServiceContainer container, PluginManager pluginManager)
    {
      this.container = container;
      InjectStaticProperties(pluginManager.LoadedTypes);
    }

    /// <summary>
    /// Injects all properties with <see cref="InjectAttribute"/> in the specified object.
    /// </summary>
    /// <param name="instance">The instance to inject.</param>
    /// <typeparam name="T">The instance type.</typeparam>
    /// <returns>The instance with injected dependencies.</returns>
    public T Inject<T>(T instance)
    {
      if (EqualityComparer<T>.Default.Equals(instance, default))
      {
        return default;
      }

      container.InjectProperties(instance);
      return instance;
    }

    // We clear injected properties as they can hold invalid references when reloading Anvil.
    void IDisposable.Dispose()
    {
      foreach (PropertyInfo propertyInfo in injectedStaticProperties)
      {
        propertyInfo.SetValue(null, default);
      }
    }

    private void InjectStaticProperties(IEnumerable<Type> types)
    {
      InjectPropertySelector propertySelector = new InjectPropertySelector(InjectPropertyTypes.StaticOnly);

      foreach (Type type in types)
      {
        List<PropertyInfo> injectableTypes = (List<PropertyInfo>)propertySelector.Execute(type);

        foreach (PropertyInfo propertyInfo in injectableTypes)
        {
          object value = container.TryGetInstance(propertyInfo.PropertyType);
          propertyInfo.SetValue(null, value);
          injectedStaticProperties.Add(propertyInfo);
        }
      }
    }
  }
}