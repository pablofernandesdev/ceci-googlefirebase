using CeciGoogleFirebase.Domain.DTO.Auth;
using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.ValidationCode;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.WebApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidationCodeService _validationCodeService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authService"></param>
        /// <param name="validationCodeService"></param>
        public AuthController(IAuthService authService,
            IValidationCodeService validationCodeService)
        {
            _authService = authService;
            _validationCodeService = validationCodeService;
        }

        /// <summary>
        /// User authentication
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Basic user data and authentication token</returns>
        /// <response code="200">Returns success request autentication</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<AuthResultDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse<AuthResultDTO>>> Auth([FromBody] LoginDTO model)
        {
            var result = await _authService.AuthenticateAsync(model, IpAddress());
            if (result.Data != null)
            {
                SetTokenCookie(result.Data.RefreshToken);
            }
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update user authentication token
        /// </summary>
        /// <returns>Basic user data and authentication token</returns>
        /// <response code="200">Returns success request autentication</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [Route("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<AuthResultDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse<AuthResultDTO>>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authService.RefreshTokenAsync(refreshToken, IpAddress());
            if (result.Data != null)
            {
                SetTokenCookie(result.Data.RefreshToken);
            }
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Revoke user authentication token
        /// </summary>
        /// <returns>Success revoking auth token</returns>
        /// <response code="200">Returns success revoking auth token</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [Route("revoke-token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> RevokeToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _authService.RevokeTokenAsync(refreshToken, IpAddress());
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Request user password recovery
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when requesting password recovery</returns>
        /// <response code="200">Returns when requesting password recovery</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [Route("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            var result = await _authService.ForgotPasswordAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Requests a validation code for the logged in user
        /// </summary>
        /// <returns>Success when requesting validation code</returns>
        /// <response code="200">Returns when requesting validation code</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="500">Internal server error</response>   
        [Authorize]
        [HttpPost]
        [Route("send-validation-code")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> SendValidationCode()
        {
            var result = await _validationCodeService.SendAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Validates the login user's validation code
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success validating validation code</returns>
        /// <response code="200">Returns when validating validation code</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="500">Internal server error</response>   
        [Authorize]
        [HttpPost]
        [Route("validate-validation-code")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> ValidateValidationCode([FromBody] ValidationCodeValidateDTO model)
        {
            var result = await _validationCodeService.ValidateCodeAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        // helper methods
        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
