using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniHub.Domain.Interface;

namespace UniHub.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("multiple-bar-chart")]
        public IActionResult GetMostCommonItemsByStore()
        {
            var data = _reportService.GetRandomMostCommonItemsByStore();
            return Ok(data);
        }
    }
}
