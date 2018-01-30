﻿using System;
using System.Linq;

namespace Kanpinar.Cex
{
    public class Hashrate
    {

        public decimal Last5Minutes { get; internal set; }

        public decimal Last15Minutes { get; internal set; }

        public decimal LastHour { get; internal set; }

        public decimal LastDay { get; internal set; }

        public decimal Previous5Minutes { get; internal set; }

        public decimal Previous15Minutes { get; internal set; }

        public decimal PreviousHour { get; internal set; }

        public decimal PreviousDay { get; internal set; }

        public override string ToString()
        {
            return string.Format(
                "Last5Minutes: {0}, Last15Minutes: {1}, LastHour: {2}, LastDay: {3}, Previous5Minutes: {4}, Previous15Minutes: {5}, PreviousHour: {6}, PreviousDay: {7}",
                Last5Minutes,
                Last15Minutes,
                LastHour,
                LastDay,
                Previous5Minutes,
                Previous15Minutes,
                PreviousHour,
                PreviousDay);
        }

        internal static T CreateFromDynamic<T>(dynamic data, T val)
            where T : Hashrate
        {
            val.Last5Minutes = JsonHelpers.ToDecimal(data["last5m"]);
            val.Last15Minutes = JsonHelpers.ToDecimal(data["last15m"]);
            val.LastHour = JsonHelpers.ToDecimal(data["last1h"]);
            val.LastDay = JsonHelpers.ToDecimal(data["last1d"]);
            val.Previous5Minutes = JsonHelpers.ToDecimal(data["prev5m"]);
            val.Previous15Minutes = JsonHelpers.ToDecimal(data["prev15m"]);
            val.PreviousHour = JsonHelpers.ToDecimal(data["prev1h"]);
            val.PreviousDay = JsonHelpers.ToDecimal(data["prev1d"]);

            return val;
        }

        internal static Hashrate FromDynamic(dynamic data)
        {
            return CreateFromDynamic(data, new Hashrate());
        }

    }
}
