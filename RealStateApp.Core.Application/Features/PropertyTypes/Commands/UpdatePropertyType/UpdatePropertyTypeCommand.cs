using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Commands.UpdatePropertyType
{
    /// <summary>
    /// Parametros de tipo de propiedades
    /// </summary>
    public class UpdatePropertyTypeCommand : IRequest<PropertyTypeDto>
    {
        /// <example>2</example>
        [SwaggerSchema(Description = "Id de tipo de propiedad")]
        public int Id { get; set; }
        /// <example>Casa</example>
        [SwaggerSchema(Description = "Nombre del tipo de propiedad")]
        public string Name { get; set; }
        /// <example>Indica que la propiedad es una casa</example>
        [SwaggerSchema(Description = "Descripcion del tipo de propiedad")]
        public string Description { get; set; }
    }

    public class UpdatePropertyTypeCommandHandler : IRequestHandler<UpdatePropertyTypeCommand, PropertyTypeDto>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public UpdatePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }
        public async Task<PropertyTypeDto> Handle(UpdatePropertyTypeCommand request, CancellationToken cancellationToken)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(request.Id) ?? throw new ApiException("PropertyType not found",(int)HttpStatusCode.BadRequest);
            var mapping = _mapper.Map<PropertyType>(request);
            await _propertyTypeRepository.UpdateAsync(mapping, request.Id);
            return _mapper.Map<PropertyTypeDto>(mapping);
        }
    }
}
