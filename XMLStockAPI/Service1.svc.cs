using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace XMLStockAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public StockObject GetStock(string tckr)
        {
            // connect to finance.yahoo
            string URL = @"https://finance.yahoo.com/quote/" + tckr;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responseReader = sreader.ReadToEnd();
            response.Close();

            // get text of #smartDaConfig element
            int index = responseReader.IndexOf("<div id=\"smartDaConfig\"");
            if (index == -1)
                return NullStock();
            string smartDaConfig = responseReader.Substring(index);
            index = smartDaConfig.IndexOf(">") + 1;
            smartDaConfig = smartDaConfig.Substring(0, index);

            const string FIN_TICKER_SYMBOL = "FIN_TICKER_SYMBOL";
            string tickerSymbol = GetFinValue(smartDaConfig, FIN_TICKER_SYMBOL);

            const string FIN_COMPANY_SHORT_NAME = "FIN_COMPANY_SHORT_NAME";
            string companyName = GetFinValue(smartDaConfig, FIN_COMPANY_SHORT_NAME);

            const string FIN_TICKER_PRICE = "FIN_TICKER_PRICE";
            float price = GetFinFloat(smartDaConfig, FIN_TICKER_PRICE);

            const string FIN_TICKER_PRICE_CHANGE_AMOUNT = "FIN_TICKER_PRICE_CHANGE_AMOUNT";
            float priceChangeAmount = GetFinFloat(smartDaConfig, FIN_TICKER_PRICE_CHANGE_AMOUNT);

            const string FIN_TICKER_PRICE_CHANGE_PERCENT = "FIN_TICKER_PRICE_CHANGE_PERCENT";
            float priceChangePercent = GetFinFloat(smartDaConfig, FIN_TICKER_PRICE_CHANGE_PERCENT);

            const string FIN_TICKER_PRICE_CHANGE_INDICATOR = "FIN_TICKER_PRICE_CHANGE_INDICATOR";
            string priceChangeIndicator = GetFinValue(smartDaConfig, FIN_TICKER_PRICE_CHANGE_INDICATOR);

            const string FIN_CURRENCY_TYPE = "FIN_CURRENCY_TYPE";
            string currency = GetFinValue(smartDaConfig, FIN_CURRENCY_TYPE);

            StockObject stockObject = new StockObject
            {
                response = "OK",
                code = 200,
                tickerSymbol = tickerSymbol,
                companyName = companyName,
                price = price,
                priceChangeAmount = priceChangeAmount,
                priceChangePercent = priceChangePercent,
                priceChangeIndicator = priceChangeIndicator,
                currency = currency
            };

            return stockObject;
        }

        private StockObject NullStock()
        {
            return new StockObject
            {
                response = "Unavailable",
                code = 404,
                tickerSymbol = "Unavailable",
                companyName = "Unavailable",
                price = 0.0f,
                priceChangeAmount = 0.0f,
                priceChangePercent = 0.0f,
                priceChangeIndicator = "Unavailable",
                currency = "Unavailable"
            };
        }

        private string GetFinValue(string smartDaConfig, string finLabel)
        {
            try
            {
                const string QUOT_STR = "&quot;";
                int labelStart = smartDaConfig.IndexOf(QUOT_STR + finLabel + QUOT_STR);
                int labelEnd = labelStart + QUOT_STR.Length + finLabel.Length + QUOT_STR.Length;
                string postLabel = smartDaConfig.Substring(labelEnd);

                int colon = postLabel.IndexOf(":");
                int colonEnd = colon + 1;
                string postColon = postLabel.Substring(colonEnd);

                int valueStart = postColon.IndexOf(QUOT_STR);
                int valuePostQuote = valueStart + QUOT_STR.Length;
                string postValueStart = postColon.Substring(valuePostQuote);

                int valueEnd = postValueStart.IndexOf(QUOT_STR);
                string value = postValueStart.Substring(0, valueEnd);

                return value;
            }
            catch
            {
                return "Null";
            }
        }

        private float GetFinFloat(string smartDaConfig, string finLabel)
        {
            string finValueStr = GetFinValue(smartDaConfig, finLabel);
            if (finValueStr == "Null")
                return 0.0f;
            try
            {
                if (finValueStr.Contains("%"))
                    finValueStr = finValueStr.Substring(0, finValueStr.IndexOf("%"));
                return float.Parse(finValueStr);
            }
            catch
            {
                return 0.0f;
            }
        }
    }
}
