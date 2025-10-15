using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Features.Offers.Commands.UpdateOffer;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealStateApp.Core.Application.Dtos.Offer;
using RealStateApp.Core.Application.Exceptions;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Features.Offers.Commands.UpdateOffer
{
    /// <summary>
    /// Actualizar oferta
    /// </summary>
    public class UpdateOfferCommand : IRequest<OfferDto>
    {
        /// <example>4</example>
        [SwaggerSchema(Description ="Id de la oferta")]
        public int Id { get; set; }
        /// <example>1500000.00</example>
        [SwaggerSchema(Description = "Monto de la oferta")]
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        /// <example>Accepted</example>
        [SwaggerSchema(Description = "Estado de la oferta: Pending, Rejected, Accepted")]
        public string Status { get; set; } // Enum: Pending, Rejected, Accepted
        /// <example>2</example>
        [SwaggerSchema(Description = "Id de la propiedad")]
        public int PropertyId { get; set; }
        /// <example>2f3b5d5e-c7b0-4f71-9ec1-90a9b0f36569</example>
        [SwaggerSchema(Description = "Id del cliente")]
        public string ClientId { get; set; }
        /// <example>20b56af7-8256-4916-b9fa-58ef801f91cf</example>
        [SwaggerSchema(Description = "Id del agente")]
        public string AgentId { get; set; }
    }

    public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand, OfferDto>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IMapper _mapper;

        public UpdateOfferCommandHandler(IOfferRepository offerRepository, IMapper mapper)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
        }
        public async Task<OfferDto> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
        {
            OfferStatus status = GetEnumValue.Compare(request.Status) ?? throw new ApiException("Status invalid",(int)HttpStatusCode.BadRequest);
            
            var offer = await _offerRepository.GetByIdAsync(request.Id) ?? throw new ApiException("Offer not found", (int)HttpStatusCode.BadRequest);
            var mapping = _mapper.Map<Offer>(request);
            mapping.Status = status;
            mapping.Date = offer.Date;
            await _offerRepository.UpdateAsync(mapping, request.Id);
            return _mapper.Map<OfferDto>(mapping);
        }

        
    }
}
