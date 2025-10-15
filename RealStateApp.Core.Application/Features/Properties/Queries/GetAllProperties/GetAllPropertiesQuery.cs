using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.Properties.Commands.UpdateProperty;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.ViewModels.Property;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Properties.Queries.GetAllProperties
{
    /// <summary>
    /// Obtiene una lista de todas las propiedades, y puede ser filtrada por parametros
    /// </summary>
    public class GetAllPropertiesQuery : IRequest<List<PropertyDto>>
    {
        /// <summary>
        /// Filtros
        /// </summary>
        
        /// <example>2</example>
        [SwaggerParameter(Description ="Id de tipo de propiedad")]
        public int? PropertyTypeId { get; set; }
        /// <example>3</example>
        [SwaggerParameter(Description = "Id de tipo de venta")]
        public int? SaleTypeId { get; set; }
        /// <example>20b56af7-8256-4916-b9fa-58ef801f91cf</example>
        [SwaggerParameter(Description = "Id de agente")]
        public string? AgentId { get; set; }
        /// <example>2000000</example>
        [SwaggerParameter(Description = "Precio minimo")]
        public decimal? MinPrice { get; set; }
        /// <example>10000000</example>
        [SwaggerParameter(Description = "Precio maximo")]
        public decimal? MaxPrice { get; set; }
        /// <example>312415</example>
        [SwaggerParameter(Description = "Codigo de propiedad")]
        public string? UniqueCode { get; set; }
    }
        public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, List<PropertyDto>>
    {
        
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public GetAllPropertiesQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<List<PropertyDto>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            var listProperties = _propertyRepository.GetAllQueryWithInclude(new List<string> { "PropertyType", "SaleType", "Improvements" });
            
            if (request.PropertyTypeId != null)
            {
                listProperties = listProperties.Where(property => property.PropertyTypeId == request.PropertyTypeId);
            }
            if (request.SaleTypeId != null)
            {
                listProperties = listProperties.Where(property => property.SaleTypeId == request.SaleTypeId);
            }
            if (request.AgentId != null)
            {
                listProperties = listProperties.Where(property => property.AgentId == request.AgentId);
            }
            if (request.MinPrice != null)
            {
                listProperties = listProperties.Where(property => property.Price >= request.MinPrice);
            }
            if (request.MaxPrice != null)
            {
                listProperties = listProperties.Where(property => property.Price <= request.MaxPrice);
            }
            if (request.UniqueCode != null)
            {
                listProperties = listProperties.Where(property => property.UniqueCode == request.UniqueCode);
            }
            var listEntities = await listProperties.ToListAsync() ?? throw new ApiException("Properties not found",(int)HttpStatusCode.NoContent);
            if (listEntities.Count == 0) throw new ApiException("Properties not found", (int)HttpStatusCode.NoContent);
            var listDtos = _mapper.Map<List<PropertyDto>>(listEntities);
            return listDtos;
        }
    }

       
} 
    