using Asp.Versioning;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Dtos.Account;
using RealStateApp.Core.Application.Dtos.Agent;
using RealStateApp.Core.Application.Interfaces.Services.Account;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;


namespace RealStateApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Controlador de usuarios")]
    public class AccountController(IWebApiAccountService accountService) : BaseApiController
    {
        private readonly IWebApiAccountService _accountService = accountService;

        [HttpPost("authenticate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticationResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Iniciar sesion",
            Description = "Inicia sesion para obtener token")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request));
        }

        [HttpPost("register-developer-user")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Registro de usuario developer",
            Description = "Registra un usuario tipo developer")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            var response = await _accountService.RegisterUserAsync(request, origin, "Developer");
            return Created(string.Empty, new { Success = true });
        }

        [HttpPost("register-admin-user")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Registro de admin",
            Description = "Registra un usuario tipo admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdminAsync([FromBody] RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            var response = await _accountService.RegisterUserAsync(request, origin, "Admin");
            return Created(string.Empty, new { Success = true });
        }

        [HttpPost("register-customer-user")]
        [Consumes(MediaTypeNames.Multipart.FormData)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Registro de cliente",
            Description = "Registra un usuario tipo cliente")]
        public async Task<IActionResult> RegisterCustomerAsync([FromForm] RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            var response = await _accountService.RegisterUserAsync(request, origin, "Customer");
            return Created(string.Empty, new { Success = true });
        }

        [HttpPost("register-agent-user")]
        [Consumes(MediaTypeNames.Multipart.FormData)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Registro de agente",
            Description = "Registra un usuario tipo agente")]
        public async Task<IActionResult> RegisterAgentAsync([FromForm] RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            var response = await _accountService.RegisterUserAsync(request, origin, "Agent");
            return Created(string.Empty, new { Success = true });
        }

        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User not found");
            }
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            var token = authHeader.StartsWith("Bearer ") ? authHeader.Substring("Bearer ".Length) : authHeader;


            var user = await _accountService.GetUserById(userId, token);

            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        [HttpPut("update-user")]
        [Authorize]
        [Consumes(MediaTypeNames.Multipart.FormData)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserApiRequest request)
        {
            if (!ModelState.IsValid) { 
                return BadRequest();
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User not found");
            }

            await _accountService.UpdateUser(request, userId);

            return Ok();
        }

        //[HttpGet("confirm-email")]
        //[SwaggerOperation(
        //    Summary = "Confirmar email",
        //    Description = "Confirmar correo para activar usuario")]
        //public async Task<IActionResult> RegisterAsync([FromQuery] string userId, [FromQuery] string token)
        //{
        //    return Ok(await _accountService.ConfirmAccountAsync(userId, token));
        //}


        //[HttpPost("forgot-password")]
        //[Consumes(MediaTypeNames.Application.Json)]
        //[SwaggerOperation(
        //    Summary = "Olvidar clave",
        //    Description = "Envia un correo para reiniciar clave")]
        //public async Task<IActionResult> ForgotPasswordAsync([FromBody]ForgotPasswordRequest request)
        //{
        //    var origin = Request.Headers["origin"];
        //    return Ok(await _accountService.ForgotPasswordAsync(request, origin));
        //}

        //[HttpPost("reset-password")]
        //[Consumes(MediaTypeNames.Application.Json)]
        //[SwaggerOperation(
        //    Summary = "Reiniciar clave",
        //    Description = "Actualiza la clave de tu usuario")]
        //public async Task<IActionResult> ResetPasswordAsync([FromBody]ResetPasswordRequest request)
        //{
        //    return Ok(await _accountService.ResetPasswordAsync(request));
        //}
    }
}
