using System;
using System.Collections.Generic;
using System.Linq;

namespace Kanpinar.Cex
{

    public class Trade
    {

        public decimal Amount { get; internal set; }

        public Timestamp Date { get; internal set; }

        public decimal Price { get; internal set; }

        public TradeId Id { get; internal set; }
        public  OrderType Type { get; internal set; }
        public override string ToString()
        {
            return string.Format("Id: {0}, Price: {1}, Amount: {2}, Date: {3}", Id, Price, Amount, Date,Type.ToString());
        }


        internal static IEnumerable<Trade> FromDynamic(dynamic data)
        {
            return ((IEnumerable<dynamic>) data).Select(
                x => new Trade
                {
                    Amount = JsonHelpers.ToDecimal(x["amount"]),
                    Date = x["date"],
                    Price = JsonHelpers.ToDecimal(x["price"]),
                    Type = x["type"]=="buy"? OrderType.Buy: OrderType.Sell,
                    Id = x["tid"]
                });
        }

    }
}
