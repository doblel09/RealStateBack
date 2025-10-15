using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.SaleType;
using RealStateApp.Core.Application.Features.Improvements.Commands.CreateImprovement;
using RealStateApp.Core.Application.Features.SaleTypes.Commands.CreateSaleType;
using RealStateApp.Core.Application.Features.SaleTypes.Commands.DeleteSaleTypeById;
using RealStateApp.Core.Application.Features.SaleTypes.Commands.UpdateSaleType;
using RealStateApp.Core.Application.Features.SaleTypes.Queries.GetAllSaleTypes;
using RealStateApp.Core.Application.Features.SaleTypes.Queries.GetSaleTypeById;
using RealStateApp.Core.Application.Interfaces.Services;
using RealStateApp.Core.Application.ViewModels.SaleType;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealStateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Mantenimiento de tipos de venta")]
    public class SaleTypeController : BaseApiController
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SaleTypeDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(
            Summary = "Listado de tipos de venta",
            Description = "Obtiene todas los tipos de venta")]
        public async Task<IActionResult> List()
        {
            return Ok(await Mediator.Send(new GetAllSaleTypesQuery()));
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaleTypeDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = $"Admin,Developer")]
        [SwaggerOperation(
            Summary = "Listar tipo de venta",
            Description = "Obtiene un tipo de venta filtrando su id")]
        public async Task<IActionResult> GetById(int id)
        {
           
            return Ok(await Mediator.Send(new GetSaleTypeByIdQuery { Id = id}));
            
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Authorize(Roles = $"Admin")]
        [SwaggerOperation(
            Summary = "Crear tipo de venta",
            Description = "Recibe los parametros para crear un tipo de venta")]
        public async Task<IActionResult> Create([FromBody] CreateSaleTypeCommand command)
        {
           
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                await Mediator.Send(command);
                return Created(string.Empty, new { Success = true });
           

        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaleTypeDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Authorize(Roles = $"Admin")]
        [SwaggerOperation(
            Summary = "Actualizar tipo de venta",
            Description = "Recibe los parametros para actualizar una mejora")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSaleTypeCommand command)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            if (id != command.Id) return BadRequest();

           
                return Ok(await Mediator.Send(command));
            
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = $"Admin")]
        [SwaggerOperation(
            Summary = "Eliminar tipo de venta mejora",
            Description = "Elimina un tipo de venta con su id")]
        public async Task<IActionResult> Delete(int id)
        {
           
                await Mediator.Send(new DeleteSaleTypeByIdCommand { Id = id });
                return NoContent();
            
        }
    }
}
