using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.User;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.WebApplication.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/report")]
    [ApiController]
    [Authorize(Policy = "Administrator")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportService"></param>
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Get the users report according to the filter
        /// </summary>
        /// <returns>Success when get the users report</returns>
        /// <response code="200">Returns success when get the users report</response>
        /// <response code="401">Not authorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal server error</response>   
        [HttpGet]
        [Route("excel/users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ResultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResultResponse))]
        public async Task<IActionResult> GenerateUsersReport([FromQuery] UserFilterDTO filter)
        {
            var result = await _reportService.GenerateUsersReport(filter);
            if (result.Data != null)
            {
                return File(new MemoryStream(result.Data), "application/octet-stream", "users_report.xlsx");
            }

            return StatusCode((int)result.StatusCode, result);
        }
    }
}
