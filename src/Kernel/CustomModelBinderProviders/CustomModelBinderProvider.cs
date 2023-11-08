using DigitalOffice.Kernel.Attributes;
using DigitalOffice.Kernel.CustomModelBinderProviders.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Reflection;

namespace DigitalOffice.Kernel.CustomModelBinderProviders;

public class CustomModelBinderProvider : IModelBinderProvider
{
  public IModelBinder GetBinder(ModelBinderProviderContext context)
  {
    if (context is null)
    {
      throw new ArgumentNullException(nameof(context));
    }

    return context.Metadata.ModelType.GetCustomAttribute<TrimAttribute>() is not null
      ? new StringTrimmerBinder()
      : null;
  }
}
