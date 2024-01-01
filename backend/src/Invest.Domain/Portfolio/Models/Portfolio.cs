using System;
using System.Collections.Generic;
using System.Linq;

namespace Invest.Domain.Portfolio.Models
{
    public class Portfolio
    {
        public Portfolio() { }
        public Portfolio(long id, Guid userGuid, string name) 
        {
            this.Id = id;
            this.UserGuid = userGuid;
            this.Name = name;
            this.Created = DateTime.UtcNow;
        }

        public long Id { get; set; }
        public Guid UserGuid { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }

        public User.Models.User User { get; set; }

        private ICollection<Stock.Models.Stock> _stocks;
        public ICollection<Stock.Models.Stock> Stocks
        {
            get
            {
                return this._stocks?.Where(x => x.Quantity != "0").ToList();
            }
            set
            {
                this._stocks = value;
            }
        }
    }
}
