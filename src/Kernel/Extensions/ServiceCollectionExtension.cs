using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LT.DigitalOffice.Kernel.Extensions;

/// <summary>
/// Helper class for services extensions.
/// </summary>
public static class ServiceCollectionExtension
{
  public static IServiceCollection AddBusinessObjects(
    this IServiceCollection services,
    ILogger logger = null)
  {
    if (services is null)
    {
      logger?.LogWarning($"Service collection is null, can not inject business objects.");

      return services;
    }

    try
    {
      var asmPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
      var files = Directory.GetFiles(asmPath, "*DigitalOffice*.dll");

      List<Assembly> assemblies = new();

      foreach (string fileName in files)
      {
        assemblies.Add(Assembly.LoadFrom(fileName));
      }

      List<Type> injectInterfaces = new();

      foreach (Assembly assembly in assemblies)
      {
        injectInterfaces.AddRange(assembly.ExportedTypes
          .Where(
            t =>
              t.IsInterface
              && t.IsPublic
              && t.GetCustomAttribute(typeof(AutoInjectAttribute)) is not null)
          .ToList());
      }

      foreach (Type injectInterface in injectInterfaces)
      {
        List<Type> injectClasses = new();

        foreach (Assembly assembly in assemblies)
        {
          injectClasses.AddRange(
            assembly.GetExportedTypes()
              .Where(t => t.GetInterface(injectInterface.Name) is not null)
              .ToList());
        }

        if (!injectClasses.Any())
        {
          logger?.LogWarning(
            $"No classes were found that inherit the interface '{injectInterface.Name}'.");

          continue;
        }

        if (injectClasses.Count > 1)
        {
          logger?.LogWarning(
            $"Found more than one class '{string.Join(',', injectClasses.Select(t => t.Name))}' inheriting the interface '{injectInterface.Name}'.");

          continue;
        }

        AutoInjectAttribute attr = injectInterface.GetCustomAttribute<AutoInjectAttribute>();
        switch (attr.InjectType)
        {
          case InjectType.Transient:
            services.AddTransient(injectInterface, injectClasses[0]);
            break;
          case InjectType.Scoped:
            services.AddScoped(injectInterface, injectClasses[0]);
            break;
          case InjectType.Singletone:
            services.AddSingleton(injectInterface, injectClasses[0]);
            break;
        }

        logger?.LogTrace(
          $"'{injectClasses[0].Name}' was successfuly '{attr.InjectType}' injected with interface '{injectInterface.Name}'.");
      }
    }
    catch (Exception exc)
    {
      logger?.LogError(exc, $"Exception while loading types from assemblies.");
    }

    return services;
  }
}
