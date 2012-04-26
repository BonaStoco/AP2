using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BonaStoco.AP1.Web.Messages
{
    public class ShoppingCartData
    {
        public ShoppingCartData()
        {
        }
        public string CardHolder { get; set; }
        public string CardNo { get; set; }
        public long CardTypeId { get; set; }
        public decimal CashFromMixedPayment { get; set; }
        public decimal ChargeAmount { get; set; }
        public decimal CreditFromMixedPayment { get; set; }
        public long CustomerId { get; set; }
        public decimal DiscountCard { get; set; }
        public decimal DiscountTotalAmount { get; set; }
        public decimal DiscPercent { get; set; }
        public long Id { get; set; }
        public decimal NetAmount { get; set; }
        public DateTime NoticedDate { get; set; }
        public string TableNumber { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal TotalAmountOfItems { get; set; }
        public decimal TotalAmountOfItemsBeforeDiscount { get; set; }
        public decimal TotalDiscountItemAmount { get; set; }
        public decimal TotalDiscountItemPercent { get; set; }
        public long TotalGuest { get; set; }
        public long TransactionCcyId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionNumber { get; set; }
        public int TransPaymentType { get; set; }
        public bool WasPaidByCard { get; set; }
    }
}