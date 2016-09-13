using HtmlAgilityPack;
using ShopmateEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebParsers
{
    public class BestBuyParser
    {
        #region Code Methods
        // Main Entry point method
        public static List<ShopmateProduct> parseBestBuyForProductName(String name)
        {
            // building bestBuy search url
            String webUrl = buildBestBuyProductSearchURLFromProductName(name);

            // getting htmldoc from web
            HtmlDocument doc = HTMLParser.getHTMLFromURL(webUrl);

            //Sometimes bestbuy has customized page with no products but brands url, which is not useful for us. Eg: "Camera". So we will prepend the search url with "all " and regenrate search results which will have products.
            if(HTMLParser.getChildrenNodesByContainingClassNameFromHtmlNode(doc.DocumentNode, "list-items").Count() == 0)
            {
                webUrl = buildBestBuyProductSearchURLFromProductName("all " + name);
                doc = HTMLParser.getHTMLFromURL(webUrl);
            }

            // Preparing list of products from htmldoc
            List<ShopmateProduct> wmodels = getBestBuyProductsFromHtml(doc);

            return wmodels;
        }

        // Core Processing method - Get products out of HTMLDoc
        private static List<ShopmateProduct> getBestBuyProductsFromHtml(HtmlDocument htmlDocument)
        {
            List<ShopmateProduct> resultProducts = new List<ShopmateProduct>();

            // Getting Container Node
            HtmlNode containerNode = HTMLParser.getChildrenNodesByContainingClassNameFromHtmlNode(htmlDocument.DocumentNode, "list-items").First(); // First because there will be only one such node.



            // Getting all children of ContainerNode which are Product Tiles.
            //IEnumerable<HtmlNode> productDivs = containerNode.ChildNodes.Where(x => x.Name == "div"); // div because it adds some text and scripts as well which we are not interested in.
            IEnumerable<HtmlNode> productDivs = HTMLParser.getChildrenNodesByExactClassNameFromHtmlNode(containerNode, "list-item");

            // Running a loop on each product tile
            foreach (HtmlNode productDiv in productDivs)
            {
                try
                {
                    ShopmateProduct smProduct = new ShopmateProduct();


                    // ***** 1. Getting Product Name Anchor tag to get product name and product url *****
                    
                    // Getting Product Url
                    String productUrlPartial = productDiv.Attributes["data-url"].Value;
                    smProduct.productUrl = Constants.bestBuyProductUrlPrefix + productUrlPartial;


                    // ***** 2. Getting Product Name *****
                    smProduct.productName = productDiv.Attributes["data-name"].Value;
                    

                    // ***** 3. Getting Product Image *****
                    HtmlNode productImageNode = HTMLParser.getChildrenNodesByNodeTypeFromHtmlNode(productDiv, "img").First();
                    smProduct.imageUrl = productImageNode.Attributes["data-src"].Value;


                    // ***** 4. Getting Product Price *****
                    smProduct.priceInString = productDiv.Attributes["data-price"].Value;
                    //Get Decimal Price.
                    if (!Decimal.TryParse(smProduct.priceInString, out smProduct.priceInDecimal)){}
                    //add '$' to price.
                    smProduct.priceInString = formatBestBuyPrice(smProduct.priceInString);


                    // ***** 5. Setting InStock to false if price could not be decimalized. *****
                    smProduct.isInStock = smProduct.priceInDecimal == 0 ? false : true; // when decimalprice is 0 that means that the actual price is not displayed in the search result. So we can not display this result. Hence using advantage of out of stock option.

                    smProduct.store = Constants.bestBuyStoreName;

                    // adding prodcuct to resultant productlist
                    resultProducts.Add(smProduct);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
            return resultProducts;
        }
        #endregion

        #region UtilityMethods
        private static String buildBestBuyProductSearchURLFromProductName(String name)
        {
            String formattedName = getBestBuyFormattedName(name);
            return String.Format(Constants.bestBuySearchUrl, formattedName);
        }

        private static String getBestBuyFormattedName(String name)
        {
            return name.Replace(" ", "+");
        }

        // Add '$' to prepend
        private static String formatBestBuyPrice(String bestBuyPrice)
        {
            bestBuyPrice = "$" + bestBuyPrice;

            return bestBuyPrice;
        }
        #endregion
    }
}
