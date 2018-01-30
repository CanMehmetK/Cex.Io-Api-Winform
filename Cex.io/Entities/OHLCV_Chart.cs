using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kanpinar.Cex
{
    public class OHLCV
    {

        public string Date { get; set; }
        public IEnumerable<OHLCVData> data1m { get; set; }
        public IEnumerable<OHLCVData> data1h { get; set; }
        public IEnumerable<OHLCVData> data1d { get; set; }

        // http://plnkr.co/edit/nevaIQ?p=preview
        // http://plnkr.co/edit/sDXdYBRxvFUcVETvcGlM?p=preview
        public override string ToString()
        {
            return "";// string.Format("Asks: {0}, Bids: {1}, Timestamp: {2}", Asks.Count(), Bids.Count(), Timestamp.localtime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="m_h_d"> Minute=m / Hour=h / Day=d </param>
        /// <returns></returns>       
        internal static OHLCV FromDynamic(dynamic data)
        {

            return new OHLCV()
            {
                Date = data["time"].ToString(),
                data1m = ((List<string[]>)JsonConvert.DeserializeObject<List<string[]>>(data["data1m"])).Select(ParseOHLCVData).ToList(),
                data1h = ((List<string[]>)JsonConvert.DeserializeObject<List<string[]>>(data["data1h"])).Select(ParseOHLCVData).ToList(),
                data1d = ((List<string[]>)JsonConvert.DeserializeObject<List<string[]>>(data["data1d"])).Select(ParseOHLCVData).ToList()
            };
        }

        private static OHLCVData ParseOHLCVData(string[] OHLCVData)
        {   //  time         Open        High        Low         Close       Volume
            // [1507680000 , 4793.9996 , 4793.9996 , 4787.3213 , 4787.3229 , 1.03986482]            
            return new OHLCVData()
            {
                date = OHLCVData[0] ,
                open = decimal.Parse(OHLCVData[1].Replace(".", ",")),
                high = decimal.Parse(OHLCVData[2].Replace(".", ",")),
                low = decimal.Parse(OHLCVData[3].Replace(".", ",")),
                close = decimal.Parse(OHLCVData[4].Replace(".", ",")),
                volume = decimal.Parse(OHLCVData[5].Replace(".", ","))
            };
        }
    }

    public class OHLCVData
    {
        public string date { get; set; }
        public decimal open { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal close { get; set; }
        public decimal volume { get; set; }
    }



}
