using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using RealStateApp.Core.Application.Dtos.Property;
using RealStateApp.Core.Application.Features.Properties.Commands.CreateProperty;
using RealStateApp.Core.Application.Features.Properties.Commands.DeletePropertyById;
using RealStateApp.Core.Application.Features.Properties.Commands.UpdateProperty;
using RealStateApp.Core.Application.Features.Properties.Queries.GetAllProperties;
using RealStateApp.Core.Application.Features.Properties.Queries.GetPropertyById;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.Property;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RealStateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    
    [SwaggerTag("Controlador de propiedades")]
    public class PropertyController : BaseApiController
    {
        

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(List<PropertyDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listar propiedades",
            Description = "Obtiene un listado de todas la propiedades")]
        public async Task<IActionResult> List()
        {
            
                return Ok(await Mediator.Send(new GetAllPropertiesQuery()));

            
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(
            Summary = "Listar propiedad por id",
            Description = "Obtiene una propiedad por su id")]
        public async Task<IActionResult> GetById(int id)
        {
           return Ok(await Mediator.Send(new GetPropertyByIdQuery { Id = id }));
        }

        [HttpGet("getbycode/{uniqueCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(
            Summary = "Listar propiedad por codigo unico",
            Description = "Obtiene una propiedad por su codigo unico")]
        public async Task<IActionResult> GetByCode(string uniqueCode)
        {           
            return Ok(await Mediator.Send(new GetPropertyByCodeQuery { UniqueCode = uniqueCode }));
        }


        [HttpPost]
        [Authorize(Roles = $"Admin,Agent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes(MediaTypeNames.Multipart.FormData)]
        [SwaggerOperation(
            Summary = "Crear propiedad",
            Description = "Recibe los parametros para crear una propiedad")]
        public async Task<IActionResult> Create([FromForm] CreatePropertyCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await Mediator.Send(command);
            return Created(string.Empty, new { Success = true });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"Admin,Agent")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Multipart.FormData)]
        [SwaggerOperation(
            Summary = "Actualizar propiedad",
            Description = "Recibe los parametros para actualizar una propiedad")]
        public async Task<IActionResult> Update(int id,[FromForm] UpdatePropertyCommand command)
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
        [Authorize(Roles = $"Admin,Agent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Eliminar propiedad",
            Description = "Elimina una propiedad con su id")]
        public async Task<IActionResult> Delete(int id)
        {
                await Mediator.Send(new DeletePropertyByIdCommand { Id = id });
                return NoContent();
        }
    }


}
