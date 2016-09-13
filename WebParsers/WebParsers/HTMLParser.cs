using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WebParsers
{
    public class HTMLParser
    {
        public static HtmlDocument getHTMLFromURL(String url)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument htmlDocument = htmlWeb.Load(url);
            return htmlDocument;
        }

        public static HtmlNode getNodeByIdFromHtmlDoc(HtmlDocument htmlDocument, String id)
        {
            return htmlDocument.DocumentNode.Descendants().Where(x => x.Id == id).First();
        }

        public static IEnumerable<HtmlNode> getChildrenNodesOfNodeByIdFromHtmlDoc(HtmlDocument htmlDocument, String id)
        {
            return htmlDocument.DocumentNode.Descendants().Single(x => x.Id == id).ChildNodes;
        }

        public static IEnumerable<HtmlNode> getChildrenNodesByExactClassNameFromHtmlNode(HtmlNode parentNode, String className)
        {
            IEnumerable<HtmlNode> nodes = parentNode.Descendants().Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Equals(className));

            return nodes;
        }

        public static IEnumerable<HtmlNode> getChildrenNodesByContainingClassNameFromHtmlNode(HtmlNode parentNode, String className)
        {
            IEnumerable<HtmlNode> nodes = parentNode.Descendants().Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains(className));

            return nodes;
        }

        public static IEnumerable<HtmlNode> getChildrenNodesByNodeTypeFromHtmlNode(HtmlNode parentNode, String nodeTypeName)
        {
            IEnumerable<HtmlNode> nodes = parentNode.Descendants().Where(x => x.Name == nodeTypeName);

            return nodes;
        }

        public static IEnumerable<HtmlNode> getChildrenNodesByContainingClassNameAndTypeFromHtmlNode(HtmlNode parentNode, String className, String type)
        {
            IEnumerable<HtmlNode> nodes = parentNode.Descendants().Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains(className) && x.NodeType.Equals(type));

            return nodes;
        }

        //------- convert String price to Decimal -----------
        public static Decimal convertStringPriceToDecimal(String sPrice)
        {
            Decimal dPrice = 999999999;
            sPrice.Replace(" ", "");

            // Filtering strings like $26.99Was$29.99 to $26.99
            int pos = Regex.Match(sPrice, "[a-z]").Index;
            if (pos > 0)
            {
                sPrice = sPrice.Substring(0, pos - 1); // -1 because RegEx returns 1 based index position.
            }
            else
            {
                // Sometimes walmart has price range with '-' so getting the minimum of that range.
                pos = sPrice.IndexOf("-");
                if(pos > 0)
                {
                    sPrice = sPrice.Substring(0, pos);
                }
            }

            sPrice = sPrice.Replace("$", "");

            if (!Decimal.TryParse(sPrice, out dPrice))
            {
            }
            return dPrice;
        }
    }
}
