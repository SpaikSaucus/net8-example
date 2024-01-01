using System.Collections.Generic;

namespace Invest.API.ViewModels.Share
{
    public class PageResponse<T> where T : class
    {
        /// <summary>
        /// Total result.
        /// </summary>
        /// <example>1</example>
        public int Total { get; set; }

        /// <summary>
        /// Page result (0..N).
        /// </summary>
        /// <example>0</example>
        public uint Offset { get; set; }

        /// <summary>
        /// Limit per page.
        /// </summary>
        /// <example>200</example>
        public ushort Limit { get; set; }

        public IEnumerable<T> Items { get; set; }
    }
}
