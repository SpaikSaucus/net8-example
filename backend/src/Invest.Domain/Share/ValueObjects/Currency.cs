using CoreLibrary.ORM;
using System.Collections.Generic;

namespace Invest.Domain.Share.ValueObjects
{
    /// <summary>
    /// https://www.iban.com/currency-codes
    /// </summary>
    public class Currency : ValueObject
    {
        public CurrencyEnum Id { get; set; }
        public string Name { get; set; }

        public enum CurrencyEnum
        {
            UNKNOWN = 0,
            USD = 1,
            EUR = 2,
            ARS = 3,
            BRL = 4
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Id;
            yield return this.Name;
        }
    }
}
