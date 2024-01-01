using Invest.API.ViewModels.Share;
using Invest.Application.Shared.DTOs;
using Invest.Domain.Portfolio.Models;
using System.Linq;

namespace Invest.API.ViewModels
{
    /// <summary>
    /// Information result for search porfolios.
    /// </summary>
    public class PortfolioPageResponse : PageResponse<PortfolioResponse>
    {
        public PortfolioPageResponse(PageDto<Portfolio> dto)
        {
            this.Items = dto.Items.Select(x => new PortfolioResponse(x));
            this.Offset = dto.Offset;
            this.Total = dto.Total;
            this.Limit = dto.Limit;
        }
    }
}
