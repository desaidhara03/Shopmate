using HtmlAgilityPack;
using ShopmateEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebParsers
{
    public class TargetParser
    {
        #region Code Methods
        // Main Entry point method to search products on Walmart
        public static List<ShopmateProduct> parseTargetForProductName(String name)
        {
            // building target search url
            String webUrl = buildTargetProductSearchURLFromProductName(name);

            // getting htmldoc from web
            HtmlDocument doc = HTMLParser.getHTMLFromURL(webUrl);

            // Preparing list of products from htmldoc
            List<ShopmateProduct> tmodels = getTargetProductsFromHtml(doc);

            return tmodels;
        }

        // Core Processing method - Get products out of HTMLDoc for Walmart
        private static List<ShopmateProduct> getTargetProductsFromHtml(HtmlDocument htmlDocument)
        {
            List<ShopmateProduct> resultProducts = new List<ShopmateProduct>();

            // Getting Container Node
            HtmlNode containerNode1 = HTMLParser.getNodeByIdFromHtmlDoc(htmlDocument, "viewport"); // First because there will be only one such node.

            ///////////////// TODO START Following lookup is not working ///////////

            HtmlNode containerNode2 = HTMLParser.getChildrenNodesByExactClassNameFromHtmlNode(containerNode1, "products products-tile").First(); 

            // Getting all children of ContainerNode which are Product Tiles.

            IEnumerable<HtmlNode> productDivs = HTMLParser.getChildrenNodesByContainingClassNameFromHtmlNode(containerNode2, "product");

            // Running a loop on each product tile
            foreach (HtmlNode productDiv in productDivs)
            {
                try
                {
                    ShopmateProduct smProduct = new ShopmateProduct();


                    // ***** 1. Getting Product Name Anchor tag to get product name and product url *****
                    HtmlNode productNameNode = HTMLParser.getChildrenNodesByContainingClassNameFromHtmlNode(productDiv, "details--title").First().ChildNodes.First(); // First because there will be only one node with such class, which is an anchor tag. TODO: we should try to fetch the anchor tag

                    // Getting Product Url
                    smProduct.productUrl = productNameNode.Attributes["href"].Value;

                    // Getting Product Name
                    smProduct.productName = productNameNode.InnerText;


                    // ***** 2. Getting Product Image *****
                    HtmlNode productImageNode = HTMLParser.getChildrenNodesByNodeTypeFromHtmlNode(productDiv, "img").First();
                    if (productImageNode.Attributes["src"] != null)
                        smProduct.imageUrl = productImageNode.Attributes["src"].Value;
                    else
                        smProduct.imageUrl = productImageNode.Attributes["original"].Value; // TODO: No longer needed.


                    // ***** 3. Getting Product Price *****
                    HtmlNode productPriceNode = HTMLParser.getChildrenNodesByContainingClassNameFromHtmlNode(productDiv, "price").First();
                    smProduct.priceInString = productPriceNode.InnerText;
                    //smProduct.priceInString = formatTargetPrice(smProduct.priceInString);


                    // ***** 4. Getting Price in Decimal ****
                    //smProduct.priceInDecimal = HTMLParser.convertStringPriceToDecimal(smProduct.priceInString); TODO: Need to parse. Take eg smart tv search result.


                    // ***** 5. Setting InStock to false if price could not be decimalized. *****
                    smProduct.isInStock = true; // TODO: Parse Decimal price first.
                                                //smProduct.isInStock = smProduct.priceInDecimal == 0 ? false : true; // when decimalprice is 0 that means that the actual price is not displayed in the search result. So we can not display this result. Hence using advantage of out of stock option.


                    smProduct.store = Constants.targetStoreName;

                    // adding prodcuct to resultant productlist
                    resultProducts.Add(smProduct);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }
            return resultProducts;
        }
        #endregion

        #region UtilityMethods
        //--- Building Target Product Search Query ------------- 
        private static String buildTargetProductSearchURLFromProductName(String name)
        {
            String formattedName = getTargetFormattedName(name);
            return String.Format(Constants.targetSearchUrl, formattedName);
        }

        //----- Helper for building Target product search query --------
        private static String getTargetFormattedName(String name)
        {
            return name.Replace(" ", "+");
        }

        //------- Formatting Target price -----------
        private static String formatTargetPrice(String targetPrice)
        {
            return targetPrice;
        }
        #endregion
    }
}
