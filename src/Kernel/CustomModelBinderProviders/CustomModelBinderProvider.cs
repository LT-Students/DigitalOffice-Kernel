using System;
using System.Reflection;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.CustomModelBinderProviders.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LT.DigitalOffice.Kernel.CustomModelBinderProviders
{
  public class CustomModelBinderProvider : IModelBinderProvider
  {
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
      if (context is null)
        throw new ArgumentNullException(nameof(context));

      return context.Metadata.ModelType.GetCustomAttribute<TrimAttribute>() is not null
        ? new StringTrimmerBinder()
        : null;
    }
  }
}
