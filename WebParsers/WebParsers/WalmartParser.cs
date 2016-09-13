using HtmlAgilityPack;
using ShopmateEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebParsers
{
    public class WalmartParser
    {
        #region Code Methods
        // Main Entry point method to search products on Walmart
        public static List<ShopmateProduct> parseWalmartForProductName(String name)
        {
            // building walmart search url
            String webUrl = buildWalmartProductSearchURLFromProductName(name);

            // getting htmldoc from web
            HtmlDocument doc = HTMLParser.getHTMLFromURL(webUrl);

            // Preparing list of products from htmldoc
            List<ShopmateProduct> wmodels = getWalmartProductsFromHtml(doc);

            return wmodels;
        }

        // Core Processing method - Get products out of HTMLDoc for Walmart
        private static List<ShopmateProduct> getWalmartProductsFromHtml(HtmlDocument htmlDocument)
        {
            List<ShopmateProduct> resultProducts = new List<ShopmateProduct>();

            // Getting Container Node
            HtmlNode containerNode = HTMLParser.getNodeByIdFromHtmlDoc(htmlDocument, "tile-container");
            // Getting all children of ContainerNode which are Product Tiles.
            IEnumerable<HtmlNode> productDivs = containerNode.ChildNodes.Where(x => x.Name == "div"); // div because it adds some text and scripts as well which we are not interested in.

            // Running a loop on each product tile
            foreach (HtmlNode productDiv in productDivs)
            {
                try
                {
                    ShopmateProduct smProduct = new ShopmateProduct();


                    // ***** 1. Getting Product Name Anchor tag to get product name and product url *****
                    HtmlNode productNameNode = HTMLParser.getChildrenNodesByExactClassNameFromHtmlNode(productDiv, "js-product-title").First(); // First because there will be only one node with such class, which is an anchor tag

                    // Getting Product Url
                    String productUrlPartial = productNameNode.Attributes["href"].Value;
                    smProduct.productUrl = Constants.walmartProductUrlPrefix + productUrlPartial;

                    // Getting Product Name
                    smProduct.productName = productNameNode.InnerText;
                    smProduct.productName.Replace("<mark>", ""); // Cleaning up the name.
                    smProduct.productName.Replace("</mark>", "");



                    // ***** 2. Getting Product Image *****
                    HtmlNode productImageNode = HTMLParser.getChildrenNodesByExactClassNameFromHtmlNode(productDiv, "product-image").First();
                    smProduct.imageUrl = productImageNode.Attributes["data-default-image"].Value;



                    // ***** 3. Getting Product Price *****
                    HtmlNode productPriceNode1 = HTMLParser.getChildrenNodesByExactClassNameFromHtmlNode(productDiv, "tile-primary").First();
                    HtmlNode productPriceNode2 = HTMLParser.getChildrenNodesByExactClassNameFromHtmlNode(productDiv, "item-price-container").First();

                    smProduct.priceInString = productPriceNode2.InnerText.Replace(" ", "");
                    smProduct.priceInString = formatWalmartPriceInString(smProduct.priceInString);
                    smProduct.priceInString = smProduct.priceInString.Replace("Was", " Was: ");

                    // ***** 4. Getting Price in Decimal ****
                    smProduct.priceInDecimal = HTMLParser.convertStringPriceToDecimal(smProduct.priceInString);
                    
                    // ***** 5. Setting InStock to false if price could not be decimalized. *****
                    smProduct.isInStock = (smProduct.priceInDecimal == 0 || smProduct.priceInString.Contains("Out of Stock")) ? false : true; // when decimalprice is 0 that means that the actual price is not displayed in the search result. So we can not display this result. Hence using advantage of out of stock option.

                    smProduct.store = Constants.walmartStoreName;

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
        //--- Building Walmart Product Search Query ------------- 
        private static String buildWalmartProductSearchURLFromProductName(String name)
        {
            String formattedName = getWalmartFormattedName(name);
            return String.Format(Constants.walmartSearchUrl, formattedName);
        }

        //----- Helper for building walmart product search query --------
        private static String getWalmartFormattedName(String name)
        {
            return name.Replace(" ", "%20");
        }

        //------- Formatting walmart price in String -----------
        private static String formatWalmartPriceInString(String walmartPrice)
        {
            walmartPrice = walmartPrice.Replace("CHECKOUT","");
            walmartPrice = walmartPrice.Replace("Listprice", " ListPrice: ");
            walmartPrice = walmartPrice.Replace("Save", " Save: ");
            walmartPrice = walmartPrice.Replace("Outofstock", " Out of Stock! ");
            walmartPrice = walmartPrice.Replace("Seedetailsincart", "See Price Detail in Cart. ");
            walmartPrice = walmartPrice.Replace("Pickuponly", " Item available only for Pickup. ");

            return walmartPrice;
        }
        #endregion
    }
}
