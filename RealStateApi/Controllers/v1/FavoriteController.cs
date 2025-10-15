using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Favorite;
using RealStateApp.Core.Application.Features.Favorites.Commands.CreateFavorite;
using RealStateApp.Core.Application.Features.Favorites.Commands.DeleteFavoriteById;
using RealStateApp.Core.Application.Features.Favorites.Queries.GetAllFavorites;
using RealStateApp.Core.Application.Features.Favorites.Queries.GetFavoriteById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;


namespace RealStateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Controlador de propiedades favoritas")]
    public class FavoriteController : BaseApiController
    {
       
        [HttpGet("favorites/customer/{clientId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FavoriteDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = $"Admin,Developer")]
        [SwaggerOperation(
            Summary = "Listado de propedidades favoritas de cliente",
            Description = "Obtiene todas las propiedades marcadas como favoritas por un cliente con su id")]
        public async Task<IActionResult> Get(string clientId)
        {
                return Ok(await Mediator.Send(new GetAllFavoritesQuery { ClientId = clientId }));
            
        }


        [HttpGet("favorites/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FavoriteDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listar una propiedad favorita de cliente por id",
            Description = "Obtiene una propiedad marcada como favorita por un cliente con el id de su relacion")]
        public async Task<IActionResult> Get(int id)
        {
                return Ok(await Mediator.Send(new GetFavoriteByIdQuery { Id = id}));
            
        }


        [HttpPost]
        [Authorize(Roles = $"Admin,Developer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Agregar  propiedad como favorita",
            Description = "Agrega una propiedad como favorita con el id de la propiedad")]
        public async Task<IActionResult> Create([FromBody]CreateFavoriteCommand command)
        {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                await Mediator.Send(command);
                return Created(string.Empty, new { Success = true });
        }
         
        


        [HttpDelete("{id}")]
        [Authorize(Roles = $"Admin,Developer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Eliminar propiedad favorita",
            Description = "Eliminar propiedad como favorita con el id de su relacion")]
        public async Task<IActionResult> Delete(DeleteFavoriteByIdCommand command)
        {
                await Mediator.Send(command);
                return NoContent();
        }
    } 
}
