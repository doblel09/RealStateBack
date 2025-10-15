using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.Dtos.Favorite;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Favorites.Queries.GetAllFavorites
{
    /// <summary>
    /// Obtiene todas las propiedades favoritas de un cliente usando su id
    /// </summary>
    public class GetAllFavoritesQuery : IRequest<List<FavoriteDto>>
    {
        /// <example>2f3b5d5e-c7b0-4f71-9ec1-90a9b0f36569</example>
        public string ClientId { get; set; }
    }
    public class GetAllFavoritesQueryHandler : IRequestHandler<GetAllFavoritesQuery, List<FavoriteDto>>
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IMapper _mapper;

        public GetAllFavoritesQueryHandler(IFavoriteRepository favoriteRepository, IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
        }
        public async Task<List<FavoriteDto>> Handle(GetAllFavoritesQuery request, CancellationToken cancellationToken)
        {
            var favorites = _favoriteRepository.GetAllQuery();
            var properties = await favorites.Where(f => f.ClientId == request.ClientId).Include(f => f.Property).Include(f => f.Property.PropertyType).Include(f => f.Property.SaleType).Include(f => f.Property.Improvements).ToListAsync();
            if (properties.Count == 0) throw new ApiException("Favorite properties not found", (int)HttpStatusCode.NoContent);
            return _mapper.Map<List<FavoriteDto>>(properties);
        }
    }
}
