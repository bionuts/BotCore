using System;
using System.Collections.Generic;
using System.Text;

namespace BCore.DataModel
{
    public class OrderPayload
    {
        public bool IsSymbolCautionAgreement { get; set; }
        public bool CautionAgreementSelected { get; set; }
        public bool IsSymbolSepahAgreement { get; set; }
        public bool SepahAgreementSelected { get; set; }
        public int orderCount { get; set; }
        public decimal orderPrice { get; set; }
        public int FinancialProviderId { get; set; }
        public int minimumQuantity { get; set; }
        public int maxShow { get; set; }
        public int orderId { get; set; }
        public string isin { get; set; }
        public string orderSide { get; set; }
        public int orderValidity { get; set; }
        public string orderValiditydate { get; set; }
        public bool shortSellIsEnabled { get; set; }
        public int shortSellIncentivePercent { get; set; }
    }
}
