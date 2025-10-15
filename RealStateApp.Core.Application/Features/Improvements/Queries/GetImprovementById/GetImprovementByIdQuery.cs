using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Improvement;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.Improvements.Commands.CreateImprovement;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealStateApp.Core.Application.Features.Improvements.Queries.GetImprovementById
{
    /// <summary>
    /// Obten un improvement con su id
    /// </summary>
    public class GetImprovementByIdQuery : IRequest<ImprovementDto>
    {
        /// <example>2</example>
        [SwaggerParameter(Description ="Id de mejora")]
        public int Id { get; set; }
    }
    public class GetImprovementByIdQueryHandler : IRequestHandler<GetImprovementByIdQuery, ImprovementDto>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public GetImprovementByIdQueryHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }
        public async Task<ImprovementDto> Handle(GetImprovementByIdQuery request, CancellationToken cancellationToken)
        {
            return await GetByIdDto(request.Id);
        }

        private async Task<ImprovementDto> GetByIdDto(int id)
        {
            var improvement = await _improvementRepository.GetByIdAsync(id) ?? throw new ApiException("Improvement not found",(int)HttpStatusCode.NoContent);
            
            return _mapper.Map<ImprovementDto>(improvement);
        }

    }
}
