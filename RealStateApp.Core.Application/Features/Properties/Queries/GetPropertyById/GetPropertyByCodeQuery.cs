using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Properties.Queries.GetPropertyById
{
    /// <summary>
    /// Obtener propiedad por codigo unico
    /// </summary>
    public class GetPropertyByCodeQuery : IRequest<PropertyDto>
    {
        /// <example>921032</example>
        public required string UniqueCode { get; set; }
    }
    public class GetPropertyByCodeQueryHandler : IRequestHandler<GetPropertyByCodeQuery, PropertyDto>
    {

        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public GetPropertyByCodeQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<PropertyDto> Handle(GetPropertyByCodeQuery request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetAllQuery()
            .Where(p => p.UniqueCode == request.UniqueCode)
            .Include(p => p.Improvements)
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Offers)
            .FirstOrDefaultAsync() ?? throw new ApiException("Property not found",(int)HttpStatusCode.NoContent);

            return _mapper.Map<PropertyDto>(property);
        }
    }
}
