using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using System.Net;

namespace RealStateApp.Core.Application.Features.Properties.Queries.GetPropertyById
{
    /// <summary>
    /// Obtener propiedad por id
    /// </summary>
    public class GetPropertyByIdQuery : IRequest<PropertyDto>
    {
        /// <example>2</example>
        public required int Id { get; set; }
    }
    public class GetPropertyByIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, PropertyDto>
    {

        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public GetPropertyByIdQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<PropertyDto> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetAllQuery()
            .Where(p => p.Id == request.Id)
            .Include(p => p.Improvements)
            .Include(p => p.PropertyType)
            .Include(p => p.SaleType)
            .Include(p => p.Offers)
            .FirstOrDefaultAsync() ?? throw new ApiException("Property not found",(int)HttpStatusCode.NoContent);

            return _mapper.Map<PropertyDto>(property);
        }
    }
}
