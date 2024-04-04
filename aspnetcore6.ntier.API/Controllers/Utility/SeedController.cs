using aspnetcore6.ntier.API.Responses;
using aspnetcore6.ntier.Services.DTO.General;
using aspnetcore6.ntier.Services.Interfaces.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore6.ntier.API.Controllers.Utility
{
    [ApiController]
    [Route("api/seed")]
    public class SeedController : ControllerBase
    {
        private readonly IDataSeedService _dataSeedService;

        public SeedController(IDataSeedService dataSeedService)
        {
            _dataSeedService = dataSeedService;
        }

        [HttpGet("development")]
        public async Task<ActionResult<ApiBaseResponse>> SeedDevelopmentData()
        {
            await _dataSeedService.DevelopmentDataSeed();
            var response = new ApiBaseResponse("Development environment data seed was successful!");
            return Ok(response);
        }

        [HttpGet("test")]
        public async Task<ActionResult<ApiBaseResponse>> SeedTestData()
        {
            await _dataSeedService.TestDataSeed();
            var response = new ApiBaseResponse("Test environment data seed was successful!");
            return Ok(response);
        }

        [HttpGet("uat")]
        public async Task<ActionResult<ApiBaseResponse>> SeedUatData()
        {
            await _dataSeedService.UatDataSeed();
            var response = new ApiBaseResponse("UAT environment data seed was successful!");
            return Ok(response);
        }

        [HttpGet("production")]
        public async Task<ActionResult<IEnumerable<DepartmentDTO>>> SeedProductionData()
        {
            await _dataSeedService.ProductionDataSeed();
            var response = new ApiBaseResponse("Production environment data seed was successful!");
            return Ok(response);
        }

    }
}
