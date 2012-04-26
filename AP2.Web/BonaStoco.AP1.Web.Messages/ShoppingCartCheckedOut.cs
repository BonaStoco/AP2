using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BonaStoco.AP1.Web.Messages
{
    public class ShoppingCartCheckedOut : ITenanIdentity
    {
        public int TenanId
        {
            get { return (int)CompanyId; }
        }
        public long ShoppingCartId { get; set; }
        public ShoppingCartData ShoppingCart { get; set; }
        public long CompanyId { get; set; }
        public string CompanyLocationId { get; set; }
        public ShoppingCartCheckedOut() { }

        public ShoppingCartCheckedOut(ShoppingCartData data)
        {
            ShoppingCart = data;
        }
    }

    public static class ShoppingCartCheckedOutMethodExtention
    {
        public static IList<ShoppingCartCheckedOut> ToShoppingCartCheckedOut(this string content, int tenanId, int locationId)
        {
            List<ShoppingCartCheckedOut> result = new List<ShoppingCartCheckedOut>();

            string[] rows = content.Split('\r', '\n');
            foreach (string row in rows)
            {
                string[] itemArr = row.Split(',');
                if (itemArr.Length < 3)
                    continue;

                ShoppingCartCheckedOut sc = new ShoppingCartCheckedOut()
                {
                    CompanyId = tenanId,
                    CompanyLocationId = locationId.ToString()
                };

                ShoppingCartData scData = new ShoppingCartData()
                {
                    TransactionNumber = itemArr[0],
                    TransactionDate = DateTime.Parse(itemArr[1]),
                    NetAmount = Decimal.Parse(itemArr[2])
                };

                sc.ShoppingCart = scData;

                result.Add(sc);
            }

            return result;
        }
    }
}