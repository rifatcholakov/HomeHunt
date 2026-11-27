using HomeHunt.Common;
using HomeHunt.Service.Data.Images.Request;
using HomeHunt.Service.Data.Properties.Request;
using HomeHunt.Service.Data.Properties.Response;
using HomeHunt.ViewModels;

namespace HomeHunt.Mapping
{
    public static class PropertyImageFormMapper
    {
        public static List<ImageUploadRequestData> ToUploadData(IEnumerable<IFormFile> files)
        {
            return files
                .Select(file => new ImageUploadRequestData
                {
                    FileName = file.FileName,
                    Content = file.OpenReadStream()
                })
                .ToList();
        }

        public static List<ImageOrderRequestData> ToImageOrder(IEnumerable<string> tokens)
        {
            List<ImageOrderRequestData> order = new List<ImageOrderRequestData>();

            foreach (string token in tokens)
            {
                if (token.Length < 2 || !int.TryParse(token.AsSpan(1), out int number))
                {
                    continue;
                }

                if (token[0] == ImageOrderTokens.Existing)
                {
                    order.Add(new ImageOrderRequestData { ExistingImageId = number });
                }
                else if (token[0] == ImageOrderTokens.New)
                {
                    order.Add(new ImageOrderRequestData { NewImageIndex = number });
                }
            }

            return order;
        }

        public static List<PropertyImageViewModel> ToViewModels(IEnumerable<GetPropertyImageResponseData> images)
        {
            return images
                .Select(image => new PropertyImageViewModel { Id = image.Id, Url = image.Url })
                .ToList();
        }
    }
}
