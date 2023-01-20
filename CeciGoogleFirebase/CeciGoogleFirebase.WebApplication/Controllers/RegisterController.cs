using CeciGoogleFirebase.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CeciGoogleFirebase.Domain.DTO.User;
using CeciGoogleFirebase.Domain.DTO.Commons;
using Microsoft.AspNetCore.Authorization;
using CeciGoogleFirebase.Domain.DTO.Register;
using CeciGoogleFirebase.Domain.DTO.Address;
using System.Collections.Generic;

namespace CeciGoogleFirebase.WebApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/register")]
    [ApiController]
    [Authorize]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registerService"></param>
        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }


        /// <summary>
        /// User self registration
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when creating a new user</returns>
        /// <response code="200">Returns success when creating a new item</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [AllowAnonymous]
        [HttpPost]
        [Route("self-registration")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> SelfRegistration([FromBody] UserSelfRegistrationDTO model)
        {
            var result = await _registerService.SelfRegistrationAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// User registration from login with Google Firebase
        /// </summary>
        /// <returns>Success when creating a new user</returns>
        /// <response code="200">Returns success when creating a new item</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [Route("sign-in-provider")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> RegisterWithLoginProvider()
        {
            var result = await _registerService.RegisterWithLoginProviderAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update user logged
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when updating user logged</returns>
        /// <response code="200">Returns success when updating user logged</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpPut]
        [Route("logged-user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> UpdateLoggedInUser([FromBody] UserLoggedUpdateDTO model)
        {
            var result = await _registerService.UpdateLoggedUserAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get logged in user
        /// </summary>
        /// <returns>Success when get logged in user</returns>
        /// <response code="200">Returns success when get logged in user</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpGet]
        [Route("logged-user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<UserResultDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse<UserResultDTO>>> GetLoggedInUser()
        {
            var result = await _registerService.GetLoggedInUserAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Redefine user password
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when redefine user password</returns>
        /// <response code="200">Returns success when redefine user password</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [Route("redefine-password")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> RedefinePassword([FromBody] UserRedefinePasswordDTO model)
        {
            var result = await _registerService.RedefinePasswordAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Add logged user address
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when add logged user address</returns>
        /// <response code="200">Returns success when add logged user address</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [Route("logged-user-address")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> AddLoggedInUserAddressAsync([FromBody] AddressLoggedUserAddDTO model)
        {
            var result = await _registerService.AddLoggedUserAddressAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update logged user address
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when updating logged user address</returns>
        /// <response code="200">Returns success when updating logged user address</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpPut]
        [Route("logged-user-address")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> UpdateLoggedInUserAddress([FromBody] UserLoggedUpdateDTO model)
        {
            var result = await _registerService.UpdateLoggedUserAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Delete logged user address
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when deleting logged user address</returns>
        /// <response code="200">Returns success when deleting logged user address</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpDelete]
        [Route("logged-user-address/{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> DeleteLoggedInUserAddress([FromRoute] AddressDeleteDTO model)
        {
            var result = await _registerService.InactivateLoggedUserAddressAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get logged in user addresses
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when get logged in user addresses</returns>
        /// <response code="200">Returns success when get logged in user addresses</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpGet]
        [Route("logged-user-address")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDataResponse<IEnumerable<AddressResultDTO>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultDataResponse<IEnumerable<AddressResultDTO>>>> GetLoggedInUserAddresss([FromRoute] AddressFilterDTO model)
        {
            var result = await _registerService.GetLoggedUserAddressesAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get logged in user address
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when get logged in user address</returns>
        /// <response code="200">Returns success when get logged in user address</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpGet]
        [Route("logged-user-address/{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<AddressResultDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse<AddressResultDTO>>> GetLoggedInUserAddress([FromRoute] AddressIdentifierDTO model)
        {
            var result = await _registerService.GetLoggedUserAddressAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
