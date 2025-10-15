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

namespace RealStateApp.Core.Application.Features.SaleTypes.Queries.GetSaleTypeById
{
    /// <summary>
    /// Obtiene el tipo de venta con su id
    /// </summary>
    public class GetSaleTypeByIdQuery : IRequest<SaleTypeDto>
    {
        /// <example>2</example>
        public required int Id { get; set; }
    }
    public class GetSaleTypeByIdQueryHandler : IRequestHandler<GetSaleTypeByIdQuery, SaleTypeDto>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public GetSaleTypeByIdQueryHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }
        public async Task<SaleTypeDto> Handle(GetSaleTypeByIdQuery request, CancellationToken cancellationToken)
        {
            return await GetByIdDto(request.Id);
        }

        private async Task<SaleTypeDto> GetByIdDto(int id)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(id) ?? throw new ApiException("SaleType not found",(int)HttpStatusCode.NoContent);

            return _mapper.Map<SaleTypeDto>(saleType);
        }

    }
}
