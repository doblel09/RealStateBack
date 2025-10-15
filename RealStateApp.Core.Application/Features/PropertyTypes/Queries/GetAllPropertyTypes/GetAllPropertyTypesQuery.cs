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

namespace RealStateApp.Core.Application.Features.PropertyTypes.Queries.GetAllPropertyTypes
{
    public class GetAllPropertyTypesQuery : IRequest<List<PropertyTypeDto>>
    {

    }
    public class GetAllPropertyTypesQueryHandler : IRequestHandler<GetAllPropertyTypesQuery, List<PropertyTypeDto>>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public GetAllPropertyTypesQueryHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }
        public async Task<List<PropertyTypeDto>> Handle(GetAllPropertyTypesQuery request, CancellationToken cancellationToken)
        {
            var propertyTypes = await _propertyTypeRepository.GetAllListAsync();
            if (propertyTypes.Count == 0) throw new ApiException("PropertyType not found", (int)HttpStatusCode.NoContent);

            return _mapper.Map<List<PropertyTypeDto>>(propertyTypes);
        }

    }
}
