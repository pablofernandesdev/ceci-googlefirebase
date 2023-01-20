using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.WebApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/address")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addressService"></param>
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        /// <summary>
        /// Get address by zip code
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when get the address</returns>
        /// <response code="200">Returns success when get address</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>   
        [HttpGet]
        [Route("zip-code/{zipCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<AddressResultDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse<AddressResultDTO>>> GetByZipCode([FromRoute] AddressZipCodeDTO model)
        {
            var result = await _addressService.GetAddressByZipCodeAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Add address
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when creating a new address</returns>
        /// <response code="200">Returns success when create address</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [Authorize(Policy = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> Add([FromBody] AddressAddDTO model)
        {
            var result = await _addressService.AddAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update address
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when updating address</returns>
        /// <response code="200">Returns success when update address</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [Authorize(Policy = "Administrator")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse>> Update([FromBody] AddressUpdateDTO model)
        {
            var result = await _addressService.UpdateAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Delete address
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when delete address</returns>
        /// <response code="200">Returns success when deleted address</response>
        /// <response code="400">Returns error if the request fails</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [Authorize(Policy = "Administrator")]
        [HttpDelete]
        [Route("{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]

        public async Task<ActionResult<ResultResponse>> Delete([FromRoute] AddressDeleteDTO model)
        {
            var result = await _addressService.DeleteAsync(model.AddressId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all adresses
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when get all adresses</returns>
        /// <response code="200">Returns success when get all adresses</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>  
        [Authorize(Policy = "Administrator")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDataResponse<IEnumerable<AddressResultDTO>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultDataResponse<IEnumerable<ResultDataResponse<AddressResultDTO>>>>> Get([FromQuery] AddressFilterDTO model)
        {
            var result = await _addressService.GetAsync(model);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get address by id
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Success when get address by id</returns>
        /// <response code="200">Returns success when get address by id</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [Authorize(Policy = "Administrator")]
        [HttpGet]
        [Route("{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<AddressResultDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<ActionResult<ResultResponse<AddressResultDTO>>> GetById([FromRoute] AddressIdentifierDTO model)
        {
            var result = await _addressService.GetByIdAsync(model.AddressId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
