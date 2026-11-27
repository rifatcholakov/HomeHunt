using HomeHunt.Service.Data.Properties.Request;
using HomeHunt.ViewModels;

namespace HomeHunt.Mapping
{
    public static class PropertyFormMappings
    {
        public static SavePropertyRequestData ToCreateRequest(this PropertyFormViewModel model)
        {
            return Fill(new SavePropertyRequestData(), model);
        }

        public static UpdatePropertyRequestData ToUpdateRequest(this PropertyFormViewModel model)
        {
            UpdatePropertyRequestData request = Fill(new UpdatePropertyRequestData(), model);

            request.RemoveImageIds = model.RemoveImageIds;

            return request;
        }

        private static TRequest Fill<TRequest>(TRequest request, PropertyFormViewModel model)
            where TRequest : SavePropertyRequestData
        {
            request.Title = model.Title;
            request.Description = model.Description;
            request.Price = model.Price ?? 0;
            request.Area = model.Area ?? 0;
            request.Bedrooms = model.Bedrooms;
            request.Bathrooms = model.Bathrooms;
            request.PropertyType = model.PropertyType;
            request.ListingType = model.ListingType;
            request.Status = model.Status;
            request.LocationId = model.LocationId ?? 0;
            request.AgentId = model.AgentId ?? 0;
            request.ImageOrder = PropertyImageFormMapper.ToImageOrder(model.ImageOrder);
            request.NewImages = PropertyImageFormMapper.ToUploadData(model.NewImages);

            return request;
        }
    }
}
