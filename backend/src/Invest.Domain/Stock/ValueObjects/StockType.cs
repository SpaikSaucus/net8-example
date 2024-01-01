using CoreLibrary.ORM;
using System.Collections.Generic;

namespace Invest.Domain.Stock.ValueObjects
{
    public class StockType : ValueObject
    {
        public StockTypeEnum Id { get; set; }
        public string Name { get; set; }

        public enum StockTypeEnum
        {
            UNKNOWN = 0,
            OTHER = 1,
            LIQUIDITY = 2, //example: cash in a bank
            COMMODITIES = 3,
            SHARE = 4,
            ETF = 5,
            CRYPTOCURRENCIES = 6
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Id;
            yield return this.Name;
        }
    }
}
