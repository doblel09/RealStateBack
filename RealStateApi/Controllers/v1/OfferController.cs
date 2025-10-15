using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Offer;
using RealStateApp.Core.Application.Features.Favorites.Queries.GetAllFavorites;
using RealStateApp.Core.Application.Features.Offers.Commands.CreateOffer;
using RealStateApp.Core.Application.Features.Offers.Commands.DeleteOfferById;
using RealStateApp.Core.Application.Features.Offers.Commands.UpdateOffer;
using RealStateApp.Core.Application.Features.Offers.Queries.GetAllOffers;
using RealStateApp.Core.Application.Features.Offers.Queries.GetOfferById;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Offer;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace RealStateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Controlador de ofertas")]

    public class OfferController : BaseApiController
    {
        
        
        [HttpGet("offers/{propertyId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OfferDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = $"Admin,Developer")]
        [SwaggerOperation(
            Summary = "Listado de ofertas",
            Description = "Obtiene todas las ofertas hechas a una propiedad con su id")]
        public async Task<IActionResult> List(int propertyId)
        {
                return Ok(await Mediator.Send(new GetAllOffersQuery { PropertyId = propertyId}));
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OfferDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = $"Admin,Developer")]
        [SwaggerOperation(
            Summary = "Listar oferta",
            Description = "Obtiene una oferta con el id")]
        public async Task<IActionResult> GetById([FromQuery] GetOfferByIdQuery query)
        {
                return Ok(await Mediator.Send(query)); 
        }


        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = $"Admin")]
        [SwaggerOperation(
            Summary = "Crear oferta",
            Description = "Crea una oferta a una propiedad")]
        public async Task<IActionResult> Create([FromBody] CreateOfferCommand command)
        {
                await Mediator.Send(command);
                return Created(string.Empty, new { Success = true });
            
        }

        [HttpPut("id")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OfferDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = $"Admin")]
        [SwaggerOperation(
            Summary = "Actualizar oferta",
            Description = "Actualiza una oferta con su id")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOfferCommand command)
        {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (id != command.Id)
                {
                    return BadRequest();
                }

                return Ok(await Mediator.Send(command));
            
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = $"Admin,Developer")]
        [SwaggerOperation(
            Summary = "Eliminar oferta",
            Description = "Elimina una oferta con su id")]
        public async Task<IActionResult> Delete(int id)
        {
            
                await Mediator.Send(new DeleteOfferByIdCommand { Id = id});
                return NoContent();
            
        }
    } 
}
