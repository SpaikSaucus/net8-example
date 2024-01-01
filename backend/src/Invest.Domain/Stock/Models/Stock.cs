using Invest.Domain.Share;
using Invest.Domain.Share.ValueObjects;
using Invest.Domain.Stock.ValueObjects;
using System;
using static Invest.Domain.Share.ValueObjects.Currency;
using static Invest.Domain.Stock.ValueObjects.StockType;

namespace Invest.Domain.Stock.Models
{
    public class Stock
    {
        public long Id { get; set; }
        public long PortfolioId { get; set; }
        public StockTypeEnum StockTypeId { get; set; }
        public CurrencyEnum CurrencyId { get; set; }

        public string Name { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public DateTime Created { get; set; }

        public Portfolio.Models.Portfolio Portfolio { get; set; }
        public Currency Currency { get; set; }
        public StockType StockType { get; set; }

        public decimal GetTotal()
        {
            return decimal.Truncate(this.Price.ConvertDecimal() * this.Quantity.ConvertDecimal());
        }

        public decimal GetTotal(CurrencyEnum currency, decimal conversion)
        {
            if (conversion <= 0) throw new ArgumentException();

            if (this.CurrencyId == currency)
                return this.GetTotal();
            else 
                return decimal.Truncate(this.GetTotal() * conversion);
        }

        public bool SyncOn
        {
            get
            {
                switch (this.StockTypeId)
                {
                    case StockTypeEnum.CRYPTOCURRENCIES:
                        return true;
                    case StockTypeEnum.SHARE:
                        return true;
                    case StockTypeEnum.ETF:
                        return true;
                    case StockTypeEnum.COMMODITIES:
                        return this.Name.ToLower().Contains("gold");
                    default:
                        return false;
                }
            }
        }

        public string SyncIntegrationNamed
        {
            get
            {
                if (this.StockTypeId == StockTypeEnum.CRYPTOCURRENCIES)
                    return "CryptoPriceNamed";
                else if (this.StockTypeId == StockTypeEnum.SHARE)
                    return "NasdaqStockPriceNamed";
                else if (this.StockTypeId == StockTypeEnum.ETF)
                    return "NasdaqEtfPriceNamed";
                else if (this.StockTypeId == StockTypeEnum.COMMODITIES && this.Name.ToLower().Contains("gold"))
                    return "GoldPriceNamed";

                return string.Empty;
            }
        }
    }
}
