using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.SaleTypes.Commands.UpdateSaleType
{
    /// <summary>
    /// Parametros para actualizar tipo de venta
    /// </summary>
    public class UpdateSaleTypeCommand : IRequest<SaleTypeDto>
    {
        /// <example>2</example>
        [SwaggerParameter(Description = "Id de tipo de venta")]
        public int Id { get; set; }
        /// <example>Renta</example>
        [SwaggerParameter(Description = "Nombre de tipo de venta")]
        public string Name { get; set; }
        /// <example>Se renta mensual o por noche</example>
        [SwaggerParameter(Description = "Descripcion de tipo de venta")]
        public string Description { get; set; }
    }

    public class UpdateSaleTypeCommandHandler : IRequestHandler<UpdateSaleTypeCommand, SaleTypeDto>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public UpdateSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }
        public async Task<SaleTypeDto> Handle(UpdateSaleTypeCommand request, CancellationToken cancellationToken)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(request.Id) ?? throw new ApiException("SaleType not found",(int)HttpStatusCode.BadRequest);
            var mapping = _mapper.Map<SaleType>(request);
            await _saleTypeRepository.UpdateAsync(mapping, request.Id);
            return _mapper.Map<SaleTypeDto>(mapping);
        }
    }
}
