using Invest.Domain.Portfolio.Models;
using System;
using System.Collections.Generic;

namespace Invest.API.ViewModels
{
    /// <summary>
    /// </summary>
    public class PortfolioResponse
    {
        public PortfolioResponse(Portfolio entity)
        {
            this.Id = entity.Id;
            this.UserGuid = entity.UserGuid;
            this.Created = entity.Created;
        }

        /// <summary>
        /// Id of the authorization.
        /// </summary>
        /// <example>1111</example>
        public long Id { get; set; }
        /// <summary>
        /// Guid of the owner.
        /// </summary>
        /// <example>4bac8878-d319-4a8d-9648-87da3fbf2cc7</example>
        public Guid UserGuid { get; set; }
        /// <summary>
        /// Created At.
        /// </summary>
        /// <example>2023-07-11T10:15:00-03:00</example>
        public DateTime Created { get; set; }
        /// <summary>
        /// List Stocks.
        /// </summary>
        public List<StockResponse> Stocks { get; set; }
    }
}
