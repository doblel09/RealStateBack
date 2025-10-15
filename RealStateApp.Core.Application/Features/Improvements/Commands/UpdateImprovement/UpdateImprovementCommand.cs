using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.Improvements.Commands.CreateImprovement;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Improvements.Commands.UpdateImprovement
{
    /// <summary>
    /// Parametros para actuizar una mejora
    /// </summary>
    public class UpdateImprovementCommand : IRequest<UpdateImprovementResponse>
    {
        /// <example>2</example>
        [SwaggerSchema(Description = "Id de mejora")]
        public int Id { get; set; }
        /// <example>Jacuzzi</example>
        [SwaggerSchema(Description = "Id de mejora")]
        public string Name { get; set; }
        /// <example>Indica que hay un jacuzzi en una propiedad</example>
        [SwaggerSchema(Description = "Descripcion de mejora")]
        public string Description { get; set; }
    }

    public class UpdateImprovementCommandHandler : IRequestHandler<UpdateImprovementCommand, UpdateImprovementResponse>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public UpdateImprovementCommandHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }
        public async Task<UpdateImprovementResponse> Handle(UpdateImprovementCommand request, CancellationToken cancellationToken)
        {
            var improvement = await _improvementRepository.GetByIdAsync(request.Id) ?? throw new ApiException("Improvement not found",(int)HttpStatusCode.BadRequest);
            var mapping = _mapper.Map<Improvement>(request);
            await _improvementRepository.UpdateAsync(mapping, request.Id);
            return _mapper.Map<UpdateImprovementResponse>(mapping);
        }
    }
}
