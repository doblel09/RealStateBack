using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Helpers;
using RealStateApp.Core.Application.Interfaces.Repositories;

using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Properties.Commands.DeletePropertyById
{
    /// <summary>
    /// Parametros para eliminar una propiedad
    /// </summary>
    public class DeletePropertyByIdCommand : IRequest<bool>
    {
        /// <example>2</example>
        [SwaggerParameter(Description = "Id de propiedad")]
        public required int Id { get; set; }

    }

    public class DeletePropertyByIdCommandHandler : IRequestHandler<DeletePropertyByIdCommand, bool>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public DeletePropertyByIdCommandHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }
        public async Task<bool> Handle(DeletePropertyByIdCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.Id) ?? throw new ApiException("Property not found",(int)HttpStatusCode.BadRequest);

            foreach (var image in property.Images)
            {
                FileManager.DeleteFile(image, property: true);
            }

            await _propertyRepository.DeleteAsync(property);
            return true;
        }
    }
}
