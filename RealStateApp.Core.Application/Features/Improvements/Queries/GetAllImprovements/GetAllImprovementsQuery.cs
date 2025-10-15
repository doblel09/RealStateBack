using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using System.Net;

namespace RealStateApp.Core.Application.Features.Improvements.Queries.GetAllImprovements
{
    /// <summary>
    /// Obtiene una lista de todas las mejoras
    /// </summary>
    public class GetAllImprovementsQuery : IRequest<List<ImprovementDto>>
    {
      
    }
    public class GetAllImprovementsQueryHandler : IRequestHandler<GetAllImprovementsQuery, List<ImprovementDto>>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public GetAllImprovementsQueryHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }
        public async Task<List<ImprovementDto>> Handle(GetAllImprovementsQuery request, CancellationToken cancellationToken)
        {
            var improvements = await _improvementRepository.GetAllListAsync();
            if (improvements.Count == 0) throw new ApiException("Improvements not found", (int)HttpStatusCode.NoContent);
            return _mapper.Map<List<ImprovementDto>>(improvements);
        }

    }
}
