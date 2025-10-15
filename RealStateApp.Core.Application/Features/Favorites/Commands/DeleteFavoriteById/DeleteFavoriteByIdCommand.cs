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

namespace RealStateApp.Core.Application.Features.Favorites.Commands.DeleteFavoriteById
{
    /// <summary>
    /// Eliminar propiedad de favorita usando el id de esta relacion
    /// </summary>
    public class DeleteFavoriteByIdCommand : IRequest<bool>
    {
        /// <example>2</example>
        [SwaggerParameter(Description ="Id de relacion propiedad-cliente")]
        public int Id { get; set; }

    }

    public class DeleteFavoriteByIdCommandHandler : IRequestHandler<DeleteFavoriteByIdCommand, bool>
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IMapper _mapper;

        public DeleteFavoriteByIdCommandHandler(IFavoriteRepository favoriteRepository, IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
        }
        public async Task<bool> Handle(DeleteFavoriteByIdCommand request, CancellationToken cancellationToken)
        {
            var favorite = await _favoriteRepository.GetByIdAsync(request.Id) ?? throw new ApiException("Favorite not found", (int)HttpStatusCode.BadRequest);
            await _favoriteRepository.DeleteAsync(favorite);
            return true;
        }
    }
}
