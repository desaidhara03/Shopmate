using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopmateEntities
{
    public class ShopmateProduct
    {
        public int id;
        public String productName;
        public String imageUrl;
        public String priceInString;
        public Decimal priceInDecimal;
        public String productUrl;
        public String modelNo;
        public int quantity;
        public Boolean isInStock = true;
        public String store;
    }
}
