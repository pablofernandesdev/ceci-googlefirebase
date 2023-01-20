using CeciGoogleFirebase.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CeciGoogleFirebase.Domain.DTO.User;
using CeciGoogleFirebase.Domain.DTO.Commons;
using Microsoft.AspNetCore.Authorization;
using CeciGoogleFirebase.Domain.DTO.Import;

namespace CeciGoogleFirebase.WebApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/user")]
    [ApiController]
    [Authorize(Policy = "Administrator")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IImportService _importService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="importService"></param>
        public UserController(IUserService userService,
            IImportService importService)
        {
            _userService = userService;
            _importService = importService;
        }

        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when creating a new user</returns>
        /// <response code="200">Returns success when creating a new item</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> Add([FromBody] UserAddDTO model)
        {
            var result = await _userService.AddAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when updating user</returns>
        /// <response code="200">Returns success when updating user</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> Update([FromBody] UserUpdateDTO model)
        {
            var result = await _userService.UpdateAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update user role
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when updating user role</returns>
        /// <response code="200">Returns success when updating user role</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpPut]
        [Route("update-role")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> UpdateRole([FromBody] UserUpdateRoleDTO model)
        {
            var result = await _userService.UpdateRoleAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when delete user</returns>
        /// <response code="200">Returns success when deleted user</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpDelete]
        [Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> Delete([FromRoute] UserDeleteDTO model)
        {
            var result = await _userService.DeleteAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Success when get all users</returns>
        /// <response code="200">Returns success when get all users</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDataResponse<IEnumerable<UserResultDTO>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultDataResponse<IEnumerable<UserResultDTO>>>> Get([FromQuery] UserFilterDTO filter)
        {
            var result = await _userService.GetAsync(filter);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <returns>Success when get user by id</returns>
        /// <response code="200">Returns success when get user by id</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpGet]
        [Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<UserResultDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse<UserResultDTO>>> GetById([FromRoute] UserIdentifierDTO model)
        {
            var result = await _userService.GetByIdAsync(model.UserId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Import users
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when import users</returns>
        /// <response code="200">Returns success when import users</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpPost]
        [Route("import")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> Import([FromForm] FileUploadDTO model)
        {
            var result = await _importService.ImportUsersAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
