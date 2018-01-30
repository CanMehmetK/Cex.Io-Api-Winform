﻿using System;
using System.Linq;

namespace Kanpinar.Cex
{
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable InconsistentNaming
    public class Balance
    {

        public Balance()
        {
            BF1 = SymbolBalance.Zero;
            BTC = SymbolBalance.Zero;
            DVC = SymbolBalance.Zero;
            GHS = SymbolBalance.Zero;
            IXC = SymbolBalance.Zero;
            LTC = SymbolBalance.Zero;
            NMC = SymbolBalance.Zero;
            USD = SymbolBalance.Zero;
            EUR = SymbolBalance.Zero;
            ETH = SymbolBalance.Zero;
            XRP = SymbolBalance.Zero;
        }

        public SymbolBalance BF1 { get; internal set; }

        public SymbolBalance BTC { get; internal set; }

        public SymbolBalance DVC { get; internal set; }

        public SymbolBalance GHS { get; internal set; }

        public SymbolBalance IXC { get; internal set; }

        public SymbolBalance LTC { get; internal set; }

        public SymbolBalance NMC { get; internal set; }

        public SymbolBalance USD { get; internal set; }

        public SymbolBalance EUR { get; internal set; }

        public SymbolBalance ETH { get; internal set; }

        public SymbolBalance XRP { get; internal set; }

        public Timestamp Timestamp { get; internal set; }

        public SymbolBalance this[Symbol s]
        {

            #region Symbol Indexer

            get
            {
                switch (s)
                {
                    case Symbol.BF1:
                        return BF1;

                    case Symbol.BTC:
                        return BTC;

                    case Symbol.DVC:
                        return DVC;

                    case Symbol.GHS:
                        return GHS;

                    case Symbol.IXC:
                        return IXC;

                    case Symbol.LTC:
                        return LTC;

                    case Symbol.NMC:
                        return NMC;

                    case Symbol.USD:
                        return USD;

                    case Symbol.EUR:
                        return EUR;

                    case Symbol.ETH:
                        return ETH;

                    case Symbol.XRP:
                        return XRP;

                    default:
                        throw new IndexOutOfRangeException(string.Format("{0} does not exist", s.Name()));
                }
            }

            internal set
            {
                switch (s)
                {
                    case Symbol.BF1:
                        BF1 = value;
                        break;

                    case Symbol.BTC:
                        BTC = value;
                        break;

                    case Symbol.DVC:
                        DVC = value;
                        break;

                    case Symbol.GHS:
                        GHS = value;
                        break;

                    case Symbol.IXC:
                        IXC = value;
                        break;

                    case Symbol.LTC:
                        LTC = value;
                        break;

                    case Symbol.NMC:
                        NMC = value;
                        break;

                    case Symbol.EUR:
                        EUR = value;
                        break;

                    case Symbol.USD:
                        USD = value;
                        break;

                    case Symbol.ETH:
                        ETH = value;
                        break;

                    case Symbol.XRP:
                        XRP = value;
                        break;

                    default:
                        throw new IndexOutOfRangeException(string.Format("{0} does not exist", s.Name()));
                }
            }

            #endregion

        }

        internal static Balance FromDynamic(dynamic data)
        {
            var balance = new Balance();
            var json = data as JsonObject;
            if (json == null) return balance;

            foreach (var sym in Enum.GetValues(typeof(Symbol)).Cast<Symbol>())
            {
                if (json.ContainsKey(sym.Name()))
                {
                    var symJson = json[sym.Name()] as JsonObject;
                    if (symJson != null)
                    {
                        balance[sym] = SymbolBalance.FromDynamic(symJson);
                    }
                }
            }

            balance.Timestamp = data["timestamp"];

            return balance;
        }

    }

    public class SymbolBalance
    {

        private const decimal DecZero = 0.0m;

        public static readonly SymbolBalance Zero = new SymbolBalance();

        public SymbolBalance(decimal available = DecZero, decimal bonus = DecZero, decimal orders = DecZero)
        {
            Available = available != DecZero ? available : DecZero;
            Bonus = bonus != DecZero ? bonus : DecZero;
            Orders = orders != DecZero ? orders : DecZero;
        }

        public decimal Available { get; private set; }

        public decimal Bonus { get; private set; }

        public decimal Orders { get; private set; }

        public override string ToString()
        {
            return string.Format("Available: {0}, Bonus: {1}, Orders: {2}", Available, Bonus, Orders);
        }

        internal static SymbolBalance FromDynamic(JsonObject data)
        {
            var available = data.ContainsKey("available") ? JsonHelpers.ToDecimal(data["available"]) : 0m;
            var bonus = data.ContainsKey("bonus") ? JsonHelpers.ToDecimal(data["bonus"]) : 0m;
            var orders = data.ContainsKey("orders") ? JsonHelpers.ToDecimal(data["orders"]) : 0m;

            return new SymbolBalance(available, bonus, orders);
        }

    }
    // ReSharper restore InconsistentNaming
    // ReSharper restore MemberCanBePrivate.Global
}