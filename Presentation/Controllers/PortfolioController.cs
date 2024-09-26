using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PortfolioController : Controller
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPortfolio(int id)
        {
            var portfolio = await _portfolioService.GetPortfolioByIdAsync(id);
            if (portfolio == null)
            {
                return NotFound();
            }
            return Ok(portfolio);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPortfolios()
        {
            var portfolios = await _portfolioService.GetAllPortfoliosAsync();
            return Ok(portfolios);
        }

        [Authorize(Roles = "Advisor, Admin")]
        [HttpPost]
        public async Task<IActionResult> AddPortfolio([FromBody] PortfolioDto portfolioDto)
        {
            await _portfolioService.AddPortfolioAsync(portfolioDto);
            return Ok();
        }

        [Authorize(Roles = "Advisor, Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePortfolio(int id, [FromBody] PortfolioDto portfolioDto)
        {
            portfolioDto.Id = id;
            await _portfolioService.UpdatePortfolioAsync(portfolioDto);
            return Ok();
        }
        
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePortfolio(int id)
        {
            await _portfolioService.DeletePortfolioAsync(id);
            return Ok();
        }

        [Authorize(Roles = "Advisor, Admin")]
        [HttpGet("{id}/metrics")]
        public async Task<IActionResult> GetPortfolioMetrics(int id)
        {
            var portfolio = await _portfolioService.GetPortfolioByIdAsync(id);
            if (portfolio == null)
            {
                return NotFound();
            }

            var portfolioMetrics = _portfolioService.GetPortfolioMetrics(portfolio);

            return Ok(portfolioMetrics);
        }

        [Authorize(Roles = "Advisor, Admin")]
        [HttpGet("{id}/market-value")]
        public async Task<IActionResult> GetPortfolioMarketValue(int id)
        {
            var portfolio = await _portfolioService.GetPortfolioByIdAsync(id);
            if (portfolio == null) return NotFound();

            var portfolioMetrics = _portfolioService.GetPortfolioMetrics(portfolio);

            return Ok(new { portfolioMetrics.TotalMarketValue });
        }

        [Authorize(Roles = "Advisor, Admin")]
        [HttpGet("{id}/roi")]
        public async Task<IActionResult> GetPortfolioROI(int id)
        {
            var portfolio = await _portfolioService.GetPortfolioByIdAsync(id);
            if (portfolio == null) return NotFound();

            var portfolioMetrics = _portfolioService.GetPortfolioMetrics(portfolio);

            return Ok(new { ROI = portfolioMetrics.PortfolioROI });
        }

        [Authorize(Roles = "Advisor, Admin")]
        [HttpGet("{id}/risk")]
        public async Task<IActionResult> GetPortfolioRisk(int id)
        {
            var portfolio = await _portfolioService.GetPortfolioByIdAsync(id);
            if (portfolio == null) return NotFound();

            var portfolioMetrics = _portfolioService.GetPortfolioMetrics(portfolio);

            return Ok(new { Risk = portfolioMetrics.PortfolioRisk });
        }

        [Authorize(Roles = "Advisor, Admin")]
        [HttpGet("{id}/transactions")]
        public async Task<IActionResult> GetPortfolioTransactions(
                                        int id,
                                        [FromQuery] string? transactionType,
                                        [FromQuery] DateTime? startDate,
                                        [FromQuery] DateTime? endDate,
                                        [FromQuery] int pageNumber = 1,
                                        [FromQuery] int pageSize = 10)
        {
            var paginatedTransactions = await _portfolioService.GetPortfolioTransactionsAsync(id, transactionType, startDate, endDate, pageNumber, pageSize);

            if (paginatedTransactions == null || !paginatedTransactions.Items.Any())
            {
                return NotFound();
            }

            return Ok(new
            {
                Transactions = paginatedTransactions.Items,
                paginatedTransactions.TotalCount,
                paginatedTransactions.PageNumber,
                paginatedTransactions.PageSize
            });
        }

        [Authorize(Roles = "Advisor, Admin")]
        [HttpGet("{id}/annualized-return")]
        public async Task<IActionResult> GetAnnualizedReturn(int id)
        {
            var annualizedReturn = await _portfolioService.GetAnnualizedReturnAsync(id);
            return Ok(annualizedReturn);
        }

    }
}
