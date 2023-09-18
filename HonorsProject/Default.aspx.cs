using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace HonorsProject
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void JsonButton_Click(object sender, EventArgs e)
        {
            string tckr = TickerSymbolTextBox.Text;

            try
            {
                string URL = @"http://localhost:63390/Service1.svc/GetStock?tckr=" + tckr;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader sreader = new StreamReader(dataStream);
                string responseReader = sreader.ReadToEnd();
                response.Close();

                StockObject stockObject = JsonConvert.DeserializeObject<StockObject>(responseReader);
                ParseStockObject(stockObject);
            }
            catch
            {
                SomethingWrong();
            }

        }

        protected void XmlButton_Click(object sender, EventArgs e)
        {
            string tckr = TickerSymbolTextBox.Text;

            try
            {
                
                string URL = @"http://localhost:63377/Service1.svc/GetStock?tckr=" + tckr;

                XmlDocument document = new XmlDocument();
                document.Load(URL);
                XmlNode root = document.DocumentElement;

                XmlNodeList xnl;

                xnl = root.SelectNodes("//*[local-name()='code']");
                if (xnl.Count > 0)
                {
                    if (xnl.Item(0).InnerText == "404")
                    {
                        SomethingWrong();
                        return;
                    }
                } else
                {
                    SomethingWrong();
                    return;
                }

                xnl = root.SelectNodes("//*[local-name()='companyName']");
                if (xnl.Count > 0)
                    CompanyNameTextBox.Text = xnl.Item(0).InnerText;

                xnl = root.SelectNodes("//*[local-name()='price']");
                if (xnl.Count > 0)
                    PriceTextBox.Text = xnl.Item(0).InnerText;

                xnl = root.SelectNodes("//*[local-name()='tickerSymbol']");
                if (xnl.Count > 0)
                    TickerSymbolResultTextBox.Text = xnl.Item(0).InnerText;

                xnl = root.SelectNodes("//*[local-name()='priceChangeAmount']");
                if (xnl.Count > 0)
                {
                    if (xnl.Item(0).InnerText[0] == '-')
                    {
                        PriceChangeAmountTextBox.Text = xnl.Item(0).InnerText;
                    }
                    else
                    {
                        PriceChangeAmountTextBox.Text = "+" + xnl.Item(0).InnerText;
                    }
                }

                xnl = root.SelectNodes("//*[local-name()='priceChangePercent']");
                if (xnl.Count > 0)
                {
                    if (xnl.Item(0).InnerText[0] == '-')
                    {
                        PriceChangePercentTextBox.Text = xnl.Item(0).InnerText + "%";
                    }
                    else
                    {
                        PriceChangePercentTextBox.Text = "+" + xnl.Item(0).InnerText + "%";
                    }
                }

                xnl = root.SelectNodes("//*[local-name()='priceChangeIndicator']");
                if (xnl.Count > 0)
                    PriceChangeIndicatorTextBox.Text = xnl.Item(0).InnerText;

                xnl = root.SelectNodes("//*[local-name()='currency']");
                if (xnl.Count > 0)
                    CurrencyTextBox.Text = xnl.Item(0).InnerText;

            }
            catch
            {
                SomethingWrong();
            }
        }

        protected void ParseStockObject(StockObject stockObject)
        {
            try
            {
                if (stockObject.code == 404)
                {
                    SomethingWrong();
                    return;
                }
                CompanyNameTextBox.Text = stockObject.companyName;
                PriceTextBox.Text = stockObject.price.ToString();
                TickerSymbolResultTextBox.Text = stockObject.tickerSymbol;
                CurrencyTextBox.Text = stockObject.currency;
                if (stockObject.priceChangeAmount < 0)
                {
                    PriceChangeAmountTextBox.Text = stockObject.priceChangeAmount.ToString();
                } else
                {
                    PriceChangeAmountTextBox.Text = "+" + stockObject.priceChangeAmount.ToString();
                }
                if (stockObject.priceChangePercent < 0)
                {
                    PriceChangePercentTextBox.Text = stockObject.priceChangePercent.ToString() + "%";
                }
                else
                {
                    PriceChangePercentTextBox.Text = "+" + stockObject.priceChangePercent.ToString() + "%";
                }
                PriceChangeIndicatorTextBox.Text = stockObject.priceChangeIndicator;
            }
            catch
            {
                SomethingWrong();
            }

        }

        protected void SomethingWrong()
        {
            try
            {
                string GOOD_TCKR = "NFLX";
                string URL = @"http://localhost:63390/Service1.svc/GetStock?tckr=" + GOOD_TCKR;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader sreader = new StreamReader(dataStream);
                string responseReader = sreader.ReadToEnd();
                response.Close();

                BadTicker();
            }
            catch
            {
                BadRequest();
            }
        }

        protected void BadTicker()
        {
            CompanyNameTextBox.Text = "Invalid Ticker Symbol";
            PriceTextBox.Text = "Try: NFLX";
            TickerSymbolResultTextBox.Text = "";
            CurrencyTextBox.Text = "";
            PriceChangeAmountTextBox.Text = "";
            PriceChangePercentTextBox.Text = "";
            PriceChangeIndicatorTextBox.Text = "";
        }

        protected void BadRequest()
        {
            CompanyNameTextBox.Text = "Something went wrong";
            PriceTextBox.Text = "";
            TickerSymbolResultTextBox.Text = "";
            CurrencyTextBox.Text = "";
            PriceChangeAmountTextBox.Text = "";
            PriceChangePercentTextBox.Text = "";
            PriceChangeIndicatorTextBox.Text = "";
        }

    }

    

    public class StockObject
    {
        public string response { get; set; }

        public int code { get; set; }

        public string tickerSymbol { get; set; }

        public string companyName { get; set; }

        public float price { get; set; }

        public float priceChangeAmount { get; set; }

        public float priceChangePercent { get; set; }

        public string priceChangeIndicator { get; set; }

        public string currency { get; set; }

    }
}