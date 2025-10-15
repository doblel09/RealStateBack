using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.Improvements.Commands.DeleteImprovementById;
using RealStateApp.Core.Application.Interfaces.Repositories;

using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Improvements.Commands.DeleteImprovementById
{
    /// <summary>
    /// Parametro para eliminar una mejora
    /// </summary>
    public class DeleteImprovementByIdCommand : IRequest<bool>
    {
        /// <example>2</example>
        [SwaggerParameter(Description = "Id de mejora")]
        public int Id { get; set; }
       
    }

    public class DeleteImprovementByIdCommandHandler : IRequestHandler<DeleteImprovementByIdCommand, bool>
    {
        private readonly IImprovementRepository _improvementRepository;
        private readonly IMapper _mapper;

        public DeleteImprovementByIdCommandHandler(IImprovementRepository improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }
        public async Task<bool> Handle(DeleteImprovementByIdCommand request, CancellationToken cancellationToken)
        {
            var improvement = await _improvementRepository.GetByIdAsync(request.Id) ?? throw new ApiException("Improvement not found", (int)HttpStatusCode.BadRequest);
            await _improvementRepository.DeleteAsync(improvement);
            return true;
        }
    }
}
