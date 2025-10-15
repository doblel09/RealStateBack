using System.Net;
using MediatR;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Interfaces.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Features.Properties.Commands.DeletePropertyImage
{
    public class DeletePropertyImagesCommand : IRequest<bool>
    {
        /// <example>2</example>
        [SwaggerParameter(Description = "Id de propiedad")]
        public required int PropertyId { get; set; }
        /// <example>/Images/User/file/ce1e16de-7edb-44ac-ae22-f58dc13bcd9b.jpg</example>
        [SwaggerParameter(Description = "Id de imagen")]
        public required List<string> Images { get; set; }
    }
    
    public class DeletePropertyImageCommandHandler : IRequestHandler<DeletePropertyImagesCommand, bool>
    {
        private readonly IPropertyRepository _propertyRepository;
        public DeletePropertyImageCommandHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }
        public async Task<bool> Handle(DeletePropertyImagesCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId)
                ?? throw new ApiException("Property not found", (int)HttpStatusCode.BadRequest);

            foreach (var image in request.Images)
            {
                if (!property.Images.Contains(image))
                {
                    throw new ApiException($"Image '{image}' not found in property", (int)HttpStatusCode.NotFound);
                }

                // Remove the image from the property
                property.Images.Remove(image);
                // Delete the image file from the server
                FileManager.DeleteFile(image, property: true);
                await _propertyRepository.UpdateAsync(property);
            }

            return true;
        }
    }
}
