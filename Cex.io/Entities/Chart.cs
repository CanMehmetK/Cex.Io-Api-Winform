using System;
using System.Collections.Generic;
using System.Linq;

namespace Kanpinar.Cex
{
    public class Chart
    {       

        public decimal Price { get; set; }
        public Timestamp Timestamp { get; set; }

        public override string ToString()
        {
            return "";// string.Format("Asks: {0}, Bids: {1}, Timestamp: {2}", Asks.Count(), Bids.Count(), Timestamp.localtime);
        }

        internal static IEnumerable<Chart> FromDynamic(dynamic data)
        {
            return ((IEnumerable<dynamic>)data).Select(
                x => new Chart
                {
                    Price = JsonHelpers.ToDecimal(x["price"]),
                    Timestamp = x["tmsp"]

                });
        }



    }




}
