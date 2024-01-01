using Invest.Domain.Share;
using Invest.Domain.Share.ValueObjects;
using Invest.Domain.Stock.Models;
using Invest.Domain.Stock.ValueObjects;
using System;

namespace Invest.API.ViewModels
{
    public class StockResponse
    {
        public StockResponse(Stock entity)
        {
            this.Id = entity.Id;
            this.PortfolioId = entity.PortfolioId;
            this.Currency = entity.Currency;
            this.StockType = entity.StockType;
            this.Name = entity.Name;
            this.Price = entity.Price;
            this.Quantity = entity.Quantity;
            this.Created = entity.Created;
            this.Total = entity.GetTotal().ConvertString();
        }

        /// <summary>
        /// Id of the stock.
        /// </summary>
        /// <example>1111</example>
        public long Id { get; set; }

        /// <summary>
        /// Id of the Portfolio.
        /// </summary>
        /// <example>1</example>
        public long PortfolioId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example></example>
        public Currency Currency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example></example>
        public StockType StockType { get; set; }

        /// <summary>
        /// Name of the Stock.
        /// </summary>
        /// <example>My Stock 1</example>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example></example>
        public string Price { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example></example>
        public string Quantity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example></example>
        public string Total { get; set; }

        /// <summary>
        /// Created At.
        /// </summary>
        /// <example>2023-07-11T10:15:00-03:00</example>
        public DateTime Created { get; set; }
    }
}
