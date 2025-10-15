using AutoMapper;
using MediatR;
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

namespace RealStateApp.Core.Application.Features.Offers.Commands.CreateOffer
{
    /// <summary>Hacer oferta a una propiedad</summary>
    public class CreateOfferCommand : IRequest<int>
    {
        /// <example>1500000.00</example>
        [SwaggerSchema(Description ="Monto de la oferta")]
        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
        /// <example>Pending</example>
        [SwaggerSchema(Description = "Estado de la oferta - por defecto se crea Pending")]
        public OfferStatus Status { get; set; }

        /// <example>2</example>
        [SwaggerSchema(Description = "Id de la propiedad que se hace la oferta")]
        public int PropertyId { get; set; }
        /// <example>2f3b5d5e-c7b0-4f71-9ec1-90a9b0f36569</example>
        [SwaggerSchema(Description = "Id del cliente que hace la oferta")]
        public string ClientId { get; set; }
        /// <example>20b56af7-8256-4916-b9fa-58ef801f91cf</example>
        [SwaggerSchema(Description = "Id del agente encargado de la propiedad")]
        public string AgentId { get; set; }
    }

    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, int>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public CreateOfferCommandHandler(IOfferRepository offerRepository,IPropertyRepository propertyRepository, IMapper mapper)
        {
            _offerRepository = offerRepository;
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {
            _ = await _propertyRepository.GetByIdAsync(request.PropertyId) ?? throw new ApiException("Property not found",(int)HttpStatusCode.BadRequest);
            var mapping = _mapper.Map<Offer>(request);
            var offer = await _offerRepository.AddAsync(mapping);
            return offer.Id;
        }

    }
}
