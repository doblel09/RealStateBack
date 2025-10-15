using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.PropertyType;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Queries.GetPropertyTypeById
{
    public class GetPropertyTypeByIdQuery : IRequest<PropertyTypeDto>
    {
        public int Id { get; set; }
    }
    public class GetPropertyTypeByIdQueryHandler : IRequestHandler<GetPropertyTypeByIdQuery, PropertyTypeDto>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public GetPropertyTypeByIdQueryHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }
        public async Task<PropertyTypeDto> Handle(GetPropertyTypeByIdQuery request, CancellationToken cancellationToken)
        {
            return await GetByIdDto(request.Id);
        }

        private async Task<PropertyTypeDto> GetByIdDto(int id)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(id) ?? throw new ApiException("PropertyType not found",(int)HttpStatusCode.NoContent);

            return _mapper.Map<PropertyTypeDto>(propertyType);
        }

    }
}
