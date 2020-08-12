using System;
using System.Collections.Generic;
using System.Text;

namespace BCore.DataModel
{
    class GetOpenOrder
    {
        public GetOpenOrderData?[] Data { get; set; }
        public string MessageDesc { get; set; }
        public bool IsSuccessfull { get; set; }
        public int? MessageCode { get; set; }
        public int Version { get; set; }
    }

    public class GetOpenOrderData
    {
        public string symbol { get; set; }
        public string nsccode { get; set; }
        public int symbolid { get; set; }
        public int lasttradeprice { get; set; }
        public string time { get; set; }
        public string dtime { get; set; }
        public int orderprice { get; set; }
        public int customerid { get; set; }
        public string ProviderName { get; set; }
        public int Providerid { get; set; }
        public string orderid { get; set; }
        public string ordervl { get; set; }
        public int ordervlid { get; set; }
        public string gtdate { get; set; }
        public string gtdateMiladi { get; set; }
        public string orderside { get; set; }
        public int ordersideid { get; set; }
        public int qunatity { get; set; }
        public int ExpectedQuantity { get; set; }
        public int excuted { get; set; }
        public string status { get; set; }
        public int visible { get; set; }
        public string customername { get; set; }
        public string orderFrom { get; set; }
        public int minimumQuantity { get; set; }
        public int maxShow { get; set; }
        public int HostOrderId { get; set; }
        public string OrderEntryDate { get; set; }
        public string orderDateTime { get; set; }
    }
}
