using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Features.Improvements.Commands.CreateImprovement
{
    /// <summary>
    /// Parametros para crear una mejora
    /// </summary>
    public class CreateImprovementCommand : IRequest<int>
    {

        /// <example>Jacuzzi</example>
        [SwaggerSchema(Description = "Nombre de mejora")]
        public string Name { get; set; }
        /// <example>Indica que la propiedad tiene jacuzzi</example>
        [SwaggerSchema(Description = "Descripcion de mejora")]
        public string Description { get; set; }
    }

    public class CreateImprovementCommandHandler : IRequestHandler<CreateImprovementCommand, int>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public CreateImprovementCommandHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;   
        }
        public async Task<int> Handle(CreateImprovementCommand request, CancellationToken cancellationToken)
        {
            var mapping = _mapper.Map<Improvement>(request);
            var improvement = await _improvementRepository.AddAsync(mapping);
            return improvement.Id;
        }

    }
}
