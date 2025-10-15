using AutoMapper;
using MediatR;
using RealStateApp.Core.Application.Exceptions;
using RealStateApp.Core.Application.Features.Favorites.Commands.CreateFavorite;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services.Account;
using RealStateApp.Core.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Favorites.Commands.CreateFavorite
{
    /// <summary>Agregar propiedad como favorita</summary>
    public class CreateFavoriteCommand : IRequest<int>
    {
        /// <example>2f3b5d5e-c7b0-4f71-9ec1-90a9b0f36569</example>
        [SwaggerSchema(Description ="Id del cliente")]
        public string ClientId { get; set; }
        /// <example>2</example>
        [SwaggerSchema(Description = "Id de propiedad")]
        public int PropertyId { get; set; }
    }
    public class CreateFavoriteCommandHandler : IRequestHandler<CreateFavoriteCommand, int>
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IBaseAccountService _accountService;
        private readonly IMapper _mapper;

        public CreateFavoriteCommandHandler(IBaseAccountService accountService,IFavoriteRepository favoriteRepository,IPropertyRepository propertyRepository, IMapper mapper)
        {
            _accountService = accountService;
            _favoriteRepository = favoriteRepository;
            _propertyRepository = propertyRepository;
            _mapper = mapper;
           
        }
        public async Task<int> Handle(CreateFavoriteCommand request, CancellationToken cancellationToken)
        {
            _ = await _propertyRepository.GetByIdAsync(request.PropertyId) ?? throw new ApiException("Property not found",(int)HttpStatusCode.BadRequest);
            if(!await _accountService.ConfirmCustomerRole(request.ClientId)) throw new ApiException("Id role invalid",(int)HttpStatusCode.BadRequest);
            var mapping = _mapper.Map<Favorite>(request);
            var favorite = await _favoriteRepository.AddAsync(mapping);
            return favorite.Id;
        }
    }
}
