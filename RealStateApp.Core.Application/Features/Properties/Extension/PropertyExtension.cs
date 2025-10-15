using AutoMapper;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Core.Application.Features.Properties.Extension
{
    /// <summary>
    /// Provides extension methods for the Property entity.
    /// </summary>
    public static class PropertyExtension
    {
        /// <summary>
        /// Converts a Property entity to a PropertyDto using the provided IMapper instance.
        /// </summary>
        /// <param name="property">The Property entity to convert.</param>
        /// <param name="mapper">The IMapper instance used for mapping.</param>
        /// <returns>A PropertyDto representation of the Property entity, or null if the input is null.</returns>
        public static PropertyDto? ToDto(this Property property, IMapper mapper)
        {
            return property == null ? null : mapper.Map<PropertyDto>(property);
        }

        /// <summary>
        /// Converts a list of Property entities to a list of PropertyDto using the provided IMapper instance.
        /// </summary>
        /// <param name="properties">The list of Property entities to convert.</param>
        /// <param name="mapper">The IMapper instance used for mapping.</param>
        /// <returns>A list of PropertyDto representations of the Property entities, or null if the input is null.</returns>
        public static List<PropertyDto>? ToDto(this List<Property> properties, IMapper mapper)
        {
            return properties == null ? null : mapper.Map<List<PropertyDto>>(properties);
        }
    }
}
