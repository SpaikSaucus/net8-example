using Asp.Versioning;
using CoreLibrary.Security;
using Invest.API.ViewModels;
using Invest.Application.UserCases.FindAll.Queries;
using Invest.Application.UserCases.FindOne.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Invest.API.Controllers.V2
{
    /// <summary>
    /// This Endpoint use Explicit ApiVersion attribute so only will be served when the parameter api-version match.
    /// </summary>
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class PortfoliosController(IMediator mediator, ICurrentUserAccessor currentUser) : ControllerBase
    {
        private readonly IMediator mediator = mediator;
        private readonly ICurrentUserAccessor currentUser = currentUser;

        /// <summary>
        /// Returns one Portfolio according to ID
        /// </summary>
        /// <param name="id" example="1">Identifier from Portfolio</param>
        /// <returns>Information result for one Portfolio</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/v2/portfolios/1
        ///         header Authorization: Bearer XXXXXXXXXXXXXXX
        /// </remarks>
        /// <response code="200">Request successful</response>
        /// <response code="401">The request is not validly authenticated</response>
        /// <response code="403">The client is not authorized for using this operation</response>
        /// <response code="404">The resource was not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PortfolioResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public async Task<IActionResult> GetOne(long id)
        {
            var userGuid = Guid.Parse(this.currentUser.User.Guid);
            var portfolio = await this.mediator.Send(new PortfolioGetQuery() { Id = id, UserGuid = userGuid });
            if (portfolio == null) return this.NotFound();

            return this.Ok(new PortfolioResponse(portfolio));
        }

        /// <summary>
        /// Returns paginated Portfolio results
        /// </summary>
        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     POST /api/v1/portfolios/findAll?sort=name,asc;id,desc&amp;offset=0&amp;limit=200
        /// </remarks>
        /// <response code="200">Request successful</response>
        /// <response code="401">The request is not validly authenticated</response>
        /// <response code="403">The client is not authorized for using this operation</response>
        /// <response code="404">The resource was not found</response>
        [HttpPost("findAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PortfolioPageResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
        public async Task<IActionResult> GetAll([FromQuery] PortfolioPageRequest req)
        {
            var query = new PortfolioGetAllQuery()
            {
                Guid = Guid.Parse(this.currentUser.User.Guid),
                Limit = req.Limit,
                Offset = req.Offset,
                Sort = req.Sort
            };

            var pageDto = await this.mediator.Send(query);

            return this.Ok(new PortfolioPageResponse(pageDto));
        }
    }
}
