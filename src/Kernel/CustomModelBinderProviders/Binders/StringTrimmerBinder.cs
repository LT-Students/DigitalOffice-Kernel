using System;
using System.IO;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace LT.DigitalOffice.Kernel.CustomModelBinderProviders.Binders
{
  public class StringTrimmerBinder : IModelBinder
  {
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
      if (bindingContext is null)
      {
        throw new ArgumentNullException(nameof(bindingContext));
      }

      Type modelType = bindingContext.ModelType;
      string json = await new StreamReader(bindingContext.HttpContext.Request.Body).ReadToEndAsync();
      object obj = JsonConvert.DeserializeObject(json, modelType);

      obj.TrimSpaces(modelType);

      bindingContext.Result = ModelBindingResult.Success(obj);
    }
  }
}
