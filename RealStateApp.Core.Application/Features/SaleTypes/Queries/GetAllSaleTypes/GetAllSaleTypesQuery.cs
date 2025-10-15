using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.SaleTypes.Queries.GetAllSaleTypes
{
    public class GetAllSaleTypesQuery : IRequest<List<SaleTypeDto>>
    {

    }
    public class GetAllSaleTypesQueryHandler : IRequestHandler<GetAllSaleTypesQuery, List<SaleTypeDto>>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public GetAllSaleTypesQueryHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }
        public async Task<List<SaleTypeDto>> Handle(GetAllSaleTypesQuery request, CancellationToken cancellationToken)
        {
            var saleTypes = await _saleTypeRepository.GetAllListAsync();
            if (saleTypes.Count == 0) throw new ApiException("SaleTypes not found", (int)HttpStatusCode.NoContent);

            return _mapper.Map<List<SaleTypeDto>>(saleTypes);
        }

    }
}
