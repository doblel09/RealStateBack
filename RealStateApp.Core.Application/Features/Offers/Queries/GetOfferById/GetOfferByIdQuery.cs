using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Offer;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Offers.Queries.GetOfferById
{
    /// <summary>
    /// Obtiene una oferta con el id de la oferta
    /// </summary>
    public class GetOfferByIdQuery : IRequest<OfferDto>
    {
        /// <example>2</example>
        public required int Id { get; set; }
    }
    public class GetOfferByIdQueryHandler : IRequestHandler<GetOfferByIdQuery, OfferDto>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IMapper _mapper;

        public GetOfferByIdQueryHandler(IOfferRepository offerRepository, IMapper mapper)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
        }
        public async Task<OfferDto> Handle(GetOfferByIdQuery request, CancellationToken cancellationToken)
        {
            return await GetByIdDto(request.Id);
        }

        private async Task<OfferDto> GetByIdDto(int id)
        {
            var offer = await _offerRepository.GetByIdAsync(id) ?? throw new ApiException("Offer not found",(int)HttpStatusCode.NoContent);
            
            return _mapper.Map<OfferDto>(offer);
        }

    }
}
