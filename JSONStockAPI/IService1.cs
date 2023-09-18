using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace JSONStockAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetStock?tckr={tckr}")]
        StockObject GetStock(string tckr);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class StockObject
    {
        [DataMember]
        public string response { get; set; }

        [DataMember]
        public int code { get; set; }

        [DataMember]
        public string tickerSymbol { get; set; }

        [DataMember]
        public string companyName { get; set; }

        [DataMember]
        public float price { get; set; }

        [DataMember]
        public float priceChangeAmount { get; set; }

        [DataMember]
        public float priceChangePercent { get; set; }

        [DataMember]
        public string priceChangeIndicator { get; set; }

        [DataMember]
        public string currency { get; set; }

    }
}
