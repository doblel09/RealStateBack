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

namespace RealStateApp.Core.Application.Features.Offers.Commands.DeleteOfferById
{
    public class DeleteOfferByIdCommand : IRequest<bool>
    {
        public required int Id { get; set; }

    }

    public class DeleteOfferByIdCommandHandler : IRequestHandler<DeleteOfferByIdCommand, bool>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IMapper _mapper;

        public DeleteOfferByIdCommandHandler(IOfferRepository offerRepository, IMapper mapper)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
        }
        public async Task<bool> Handle(DeleteOfferByIdCommand request, CancellationToken cancellationToken)
        {
            var offer = await _offerRepository.GetByIdAsync(request.Id) ?? throw new ApiException("Offer not found",(int)HttpStatusCode.BadRequest);
            await _offerRepository.DeleteAsync(offer);
            return true;
        }
    }
}
