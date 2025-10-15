using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Offer;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services.Account;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Offers.Queries.GetAllOffers
{
    /// <summary>
    /// Obtiene todas las ofertas hechas a una propiedad con el id de la propiedad
    /// </summary>
    public class GetAllOffersQuery : IRequest<List<OfferDto>>
    {
        /// <example>2</example>
        [SwaggerParameter(Description ="Id de la propiedad")]
        public required int PropertyId { get; set; }
    }
    public class GetAllOffersQueryHandler : IRequestHandler<GetAllOffersQuery, List<OfferDto>>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IWebApiAccountService _webApiAccountService;
        private readonly IMapper _mapper;

        public GetAllOffersQueryHandler(IOfferRepository offerRepository, IMapper mapper)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
        }
        public async Task<List<OfferDto>> Handle(GetAllOffersQuery request, CancellationToken cancellationToken)
        {
            var query = _offerRepository.GetAllQuery();
            var offers = await query
            .Where(o => o.PropertyId == request.PropertyId)
            .ToListAsync() ?? throw new ApiException("Offers not found",(int)HttpStatusCode.NoContent);
            var offerList = _mapper.Map<List<OfferDto>>(offers);

            offerList.ForEach(async (offer) =>
            {
                offer.ClientName = await _webApiAccountService.GetCustomerNameAsync(offer.ClientId);
            });

            return offerList;
        }

    }
}
