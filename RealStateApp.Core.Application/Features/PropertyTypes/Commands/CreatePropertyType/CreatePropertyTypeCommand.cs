using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Commands.CreatePropertyType
{
    /// <summary>
    /// Parametros para crear un tipo de propiedad
    /// </summary>
    public class CreatePropertyTypeCommand : IRequest<int>
    {
        /// <example>Casa</example>
        [SwaggerSchema(Description = "Nombre de tipo de propiedad")]
        public string Name { get; set; }
        /// <example>Indica que la propiedad es una casa</example>
        [SwaggerSchema(Description = "Descripcion de tipo de propiedad")]
        public string Description { get; set; }
    }

    public class CreatePropertyTypeCommandHandler : IRequestHandler<CreatePropertyTypeCommand, int>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public CreatePropertyTypeCommandHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreatePropertyTypeCommand request, CancellationToken cancellationToken)
        {
            var mapping = _mapper.Map<PropertyType>(request);
            var propertyType = await _propertyTypeRepository.AddAsync(mapping);
            return propertyType.Id;
        }
    }
}