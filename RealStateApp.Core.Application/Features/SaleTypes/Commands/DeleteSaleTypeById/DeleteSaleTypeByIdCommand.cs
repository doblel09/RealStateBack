using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.SaleTypes.Commands.DeleteSaleTypeById
{
    /// <summary>
    /// Elimina un tipo de venta con su id
    /// </summary>
    public class DeleteSaleTypeByIdCommand : IRequest<bool>
    {
        /// <example>2</example>
        public required int Id { get; set; }

    }

    public class DeleteSaleTypeByIdCommandHandler : IRequestHandler<DeleteSaleTypeByIdCommand, bool>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public DeleteSaleTypeByIdCommandHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }
        public async Task<bool> Handle(DeleteSaleTypeByIdCommand request, CancellationToken cancellationToken)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(request.Id) ?? throw new ApiException("SaleType not found",(int)HttpStatusCode.NotFound);
            await _saleTypeRepository.DeleteAsync(saleType);
            return true;
        }
    }
}
