using System;
using System.Collections.Generic;
using System.Linq;

namespace Kanpinar.Cex
{
    public class OrderBook
    {

        public IEnumerable<OrderBookOrder> Asks { get; internal set; }

        public IEnumerable<OrderBookOrder> Bids { get; internal set; }

        public Timestamp Timestamp { get; set; }

        public string sell_total { get; internal set; }
        public string buy_total { get; internal set; }

        public override string ToString()
        {
            return string.Format("Asks: {0}, Bids: {1}, Timestamp: {2}", Asks.Count(), Bids.Count(), Timestamp.localtime);
        }

        internal static OrderBook FromDynamic(dynamic data)
        {
            return new OrderBook
            {
                Asks = ((IEnumerable<object>)data["asks"]).Select(ParseOrder).ToList(),
                Bids = ((IEnumerable<object>)data["bids"]).Select(ParseOrder).ToList(),
                Timestamp = data["timestamp"],
                buy_total = data["buy_total"],
                sell_total = data["sell_total"],
            };
        }

        private static OrderBookOrder ParseOrder(dynamic orderData)
        {
            var price = orderData[0];
            var quantity = orderData[1];
            return new OrderBookOrder(
                Convert.ToDecimal(price),
                Convert.ToDecimal(quantity)
                );
        }

    }

}
