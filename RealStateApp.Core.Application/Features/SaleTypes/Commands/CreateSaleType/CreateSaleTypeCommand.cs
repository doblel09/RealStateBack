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

namespace RealStateApp.Core.Application.Features.SaleTypes.Commands.CreateSaleType
{
    /// <summary>
    /// Parametros para crear un tipo de venta
    /// </summary>
    public class CreateSaleTypeCommand : IRequest<int>
    {
        /// <example>Renta</example>
        [SwaggerSchema(Description = "Nombre del tipo de venta")]
        public string Name { get; set; }
        /// <example>Precio pagado por la utilización de la tierra mensual o por noche</example>
        [SwaggerSchema(Description = "Descripcion del tipo de venta")]
        public string Description { get; set; }
    }

    public class CreateSaleTypeCommandHandler : IRequestHandler<CreateSaleTypeCommand, int>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public CreateSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateSaleTypeCommand request, CancellationToken cancellationToken)
        {
            var mapping = _mapper.Map<SaleType>(request);
            var saleType = await _saleTypeRepository.AddAsync(mapping);
            return saleType.Id;
        }
    }
}