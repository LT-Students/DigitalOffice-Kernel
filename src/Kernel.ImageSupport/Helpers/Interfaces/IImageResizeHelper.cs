using LT.DigitalOffice.Kernel.Attributes;
using System.Threading.Tasks;

namespace LT.DigitalOffice.Kernel.ImageSupport.Helpers.Interfaces
{
  [AutoInject]
  public interface IImageResizeHelper
  {
    Task<(bool isSuccess, string resizedContent, string extension)> ResizeForAvatarAsync(string inputBase64, string extension, int conditionalWidth = 1, int conditionalHeight = 1, int resizeMaxValue = 150);

    Task<(bool isSuccess, string resizedContent, string extension)> ResizeAsync(string inputBase64, string extension, int resizeMinValue = 150);
  }
}
