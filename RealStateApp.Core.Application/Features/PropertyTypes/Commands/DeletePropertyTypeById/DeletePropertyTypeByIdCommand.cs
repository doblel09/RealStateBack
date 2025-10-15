using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.PropertyTypes.Commands.DeletePropertyTypeById
{
    /// <summary>
    /// Parametro para eliminar tipo de propiedades
    /// </summary>
    public class DeletePropertyTypeByIdCommand : IRequest<bool>
    {
        /// <example>2</example>
        [SwaggerParameter(Description = "Id de tipo de propiedad")]
        public int Id { get; set; }

    }

    public class DeletePropertyTypeByIdCommandHandler : IRequestHandler<DeletePropertyTypeByIdCommand, bool>
    {
        private readonly IPropertyTypeRepository _propertyTypeRepository;
        private readonly IMapper _mapper;

        public DeletePropertyTypeByIdCommandHandler(IPropertyTypeRepository propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }
        public async Task<bool> Handle(DeletePropertyTypeByIdCommand request, CancellationToken cancellationToken)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(request.Id) ?? throw new ApiException("PropertyType not found",(int)HttpStatusCode.BadRequest);
            await _propertyTypeRepository.DeleteAsync(propertyType);
            return true;
        }
    }
}
