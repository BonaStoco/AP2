using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BonaStoco.AP1.Web.Messages
{
    public class UploadSalesMessage : ITenanIdentity
    {
        public int TenanId { get; set; }
        public int LocationId { get; set; }
        public SalesItem[] SalesItems { get; set; }
        public string CompanyReserved { get; set; }
    }

    public class SalesItem
    {
        public string TransactionNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal SalesAmount { get; set; }
        public string CcyCode { get; set; }
    }

    public static class UploadSalesMessageMethodExtention
    {
        public static UploadSalesMessage ToUploadSalesMessagee(this string content)
        {
            IList<SalesItem> items = new List<SalesItem>();

            string[] rows = content.Split('\r', '\n');
            foreach (string row in rows)
            {
                string[] itemArr = row.Split(',');
                if (itemArr.Length < 3)
                    continue;

                SalesItem salesItem = new SalesItem()
                {
                    TransactionNumber = itemArr[0],
                    TransactionDate = DateTime.Parse(itemArr[1]),
                    SalesAmount = Decimal.Parse(itemArr[2]),
                    CcyCode = itemArr.Length < 4 ? "IDR" : itemArr[3]
                };

                items.Add(salesItem); 
            }

            UploadSalesMessage result = new UploadSalesMessage()
            {
                SalesItems = items.ToArray()
            };

            return result;
        }
    }
}