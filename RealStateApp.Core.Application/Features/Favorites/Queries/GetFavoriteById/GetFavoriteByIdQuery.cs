using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Dtos.Favorite;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Favorites.Queries.GetFavoriteById
{
    /// <summary>
    /// Obtiene una propiedad favorita con el id de su relacion
    /// </summary>
    public class GetFavoriteByIdQuery : IRequest<FavoriteDto>
    {
        /// <example>2</example>
        [SwaggerParameter(Description ="Id de la relacion")]
        public int Id { get; set; }
    }
    public class GetFavoriteByIdQueryHandler : IRequestHandler<GetFavoriteByIdQuery, FavoriteDto>
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IMapper _mapper;

        public GetFavoriteByIdQueryHandler(IFavoriteRepository favoriteRepository, IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
        }
        public async Task<FavoriteDto> Handle(GetFavoriteByIdQuery request, CancellationToken cancellationToken)
        {
            return await GetByIdDto(request.Id);
        }

        private async Task<FavoriteDto> GetByIdDto(int id)
        {
            var favorite = await _favoriteRepository.GetByIdAsync(id) ?? throw new ApiException("Favorite not found", (int)HttpStatusCode.NoContent);

            return _mapper.Map<FavoriteDto>(favorite);
        }

    }
}
