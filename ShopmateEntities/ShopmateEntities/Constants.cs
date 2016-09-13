using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopmateEntities
{
    // unique constant values to be used project wide to avoid typo errors.
    public class Constants
    {
        // ---- Walmart Constants ----
        public const String walmartStoreName = "Walmart";
        public const String walmartProductUrlPrefix = "http://www.walmart.com";
        public const String walmartSearchUrl = "http://www.walmart.com/search/?query={0}&grid=false"; // {0} must be replaced by argument. grid=false will force the search results to be in list.

        // ---- BestBuy Constants ----
        public const String bestBuyStoreName = "BestBuy";
        public const String bestBuyProductUrlPrefix = "http://www.bestbuy.com";
        public const String bestBuySearchUrl = "http://www.bestbuy.com/site/searchpage.jsp?st={0}&_dyncharset=UTF-8&id=pcat17071&type=page&sc=Global&cp=1&nrp=&sp=&qp=&list=y&iht=y&usc=All+Categories&ks=960&keys=keys"; // {0} must be replaced by argument.  list=y will force the search results to be in list.

        // ---- Target Constants ----
        public const String targetStoreName = "Target";
        public const String targetProductUrlPrefix = "http://www.target.com";
        public const String targetSearchUrl = "http://www.target.com/s?searchTerm={0}"; // {0} must be replaced by argument. grid=false will force the search results to be in list.
    }
}
