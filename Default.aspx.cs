using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebParsers;
using ShopmateEntities;

public partial class _Default : Page
{
    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void search_btn_Click(object sender, EventArgs e)
    {
        mainSearchMethod();
    }

    protected void sortby_rbl_Selection_Changed(object sender, EventArgs e)
    {
        mainSearchMethod(); // To reStart search based on sortby selection.
    }
    #endregion

    #region Core Functionalities

    private void mainSearchMethod()
    {
        try
        {
            setValidationText("");
            String searchString = search_tb.Text;

            //---- Validating search query -----
            bool validationFailed = validateSearchQuery(searchString);

            if (!validationFailed)
            {
                //-------- Getting all Search results from all stores if validation passed ----------
                List<ShopmateProduct> searchResults = searchAll(searchString);

                //-------- Display all Search Results -----------------
                displayAllProductsOnPage(searchResults);
            }
        }
        catch (Exception ex)
        {
            validation_lbl.Text = ex.Message + ex.StackTrace;
        }
    }

    //------ Initiate search on All stores -----------
    private List<ShopmateProduct> searchAll(String searchString)
    {
        List<ShopmateProduct> allProducts = new List<ShopmateProduct>();

        //------------ Getting search results from Walmart ----------
        List<ShopmateProduct> walmartProducts = new List<ShopmateProduct>();
        walmartProducts = WalmartParser.parseWalmartForProductName(searchString);
        walmartProducts.RemoveAll(x => x.isInStock == false);

        //------------ Getting and adding BestBuy Search results to existing result list -----
        List<ShopmateProduct> bestBuyProducts = new List<ShopmateProduct>();
        bestBuyProducts = BestBuyParser.parseBestBuyForProductName(searchString);
        bestBuyProducts.RemoveAll(x => x.isInStock == false);

        //------------ Getting and adding Target Search results to existing result list -----
        List<ShopmateProduct> targetProducts = new List<ShopmateProduct>();
        //targetProducts = TargetParser.parseTargetForProductName(searchString);
        //targetProducts.RemoveAll(x => x.isInStock == false);

        if (sortby_rbl.SelectedValue == "P") // Sorting by Price
        {
            allProducts = getSortByPriceProductsList(walmartProducts, bestBuyProducts, targetProducts);
        }
        else // sortby_rbl.SelectedValue == "MR"  Sorting by Most Relevant
        {
            allProducts = getMostRelavantProductsList(walmartProducts, bestBuyProducts, targetProducts);    
        }

        return allProducts;
    }

    //-------- Display all Processed Search Results -----------------
    private void displayAllProductsOnPage(List<ShopmateProduct> searchResults)
    {
        foreach(ShopmateProduct product in searchResults)
        {
            //----- Creating a tile for one product --------
            Panel panel = createPanelFromShopmateProduct(product);

            rootContainerPanel.Controls.Add(panel); // Adding each results to panel
        }
    }

    //----- Creates a tile for one product --------
    private Panel createPanelFromShopmateProduct(ShopmateProduct product)
    {
        Panel mainPanel = new Panel();
        //mainPanel.CssClass = "product-tile-main-table";

        HyperLink hyperLink = new HyperLink();
        hyperLink.NavigateUrl = product.productUrl;
        hyperLink.Attributes.Add("Target", "_blank");
        
        //Panel panel = new Panel();
        //-- Creating structure of main table --
        Table mainTable = new Table();
        TableRow tableRow = new TableRow();
        TableCell imageCell = new TableCell();
        TableCell contentColumn = new TableCell();
        tableRow.Cells.Add(imageCell);
        tableRow.Cells.Add(contentColumn);
        mainTable.Rows.Add(tableRow);
        
        //-- Creating structure of inner table --
        Table contentTable = new Table();
        TableRow nameRow = new TableRow();
        TableCell nameCell = new TableCell();
        nameRow.Cells.Add(nameCell);
        contentTable.Rows.Add(nameRow);

        TableRow priceRow = new TableRow();
        TableCell priceCell = new TableCell();
        priceRow.Cells.Add(priceCell);
        contentTable.Rows.Add(priceRow);

        TableRow storeRow = new TableRow();
        TableCell storeCell = new TableCell();
        storeRow.Cells.Add(storeCell);
        contentTable.Rows.Add(storeRow);

        contentColumn.Controls.Add(contentTable);

        //---- Product Image ---
        Image image = new Image();
        image.ImageUrl = product.imageUrl;
        image.Height = 125;
        Panel imagePanel = new Panel();
        imagePanel.Height = 125;
        imagePanel.Width = 200;
        imagePanel.Controls.Add(image);
        imagePanel.CssClass = "product-tile-image-container";
        imageCell.Controls.Add(imagePanel);

        //--- Product Name ---
        Label productNameLabel = new Label();
        productNameLabel.Text = product.productName;
        productNameLabel.CssClass = "product-tile-name";
        nameCell.Controls.Add(productNameLabel);

        //--- Product Price ---
        Label priceLabel = new Label();
        priceLabel.Text = product.priceInString;
        priceLabel.CssClass = "product-tile-price";
        priceCell.Controls.Add(priceLabel);

        //--- Product Store ---
        Label storeName = new Label();
        storeName.Text = "Store: " + product.store;
        switch(product.store)
        {
            case Constants.walmartStoreName:
                storeName.CssClass = "product-tile-walmart-store";
                break;
            case Constants.bestBuyStoreName:
                storeName.CssClass = "product-tile-bestbuy-store";
                break;
            case Constants.targetStoreName:
                storeName.CssClass = "product-tile-target-store";
                break;
        }
        storeCell.Controls.Add(storeName);

        hyperLink.Controls.Add(mainTable);

        mainPanel.Controls.Add(hyperLink);
        return mainPanel;
    }
    #endregion

    #region Helper Functions
    //--------- This will block wierd characters as input and will stop processing and will display error -----------
    private bool validateSearchQuery(String name)
    {
        bool validationFailed = false;
        
        if (name.Equals(""))
        {
            setValidationText("Please enter a product name");
            validationFailed = true;
        }
        
        if(!validationFailed && !name.Replace(" ", "").All(Char.IsLetterOrDigit))
        {
            setValidationText("Invalid characters entered such as (! . @ # $ % ^ & * ( ) - _ +). Only alpha numerics are allowed.");
            validationFailed = true;
        }

        return validationFailed;
    }

    //-------- Helper function to set validation text ----------
    private void setValidationText(String validationText)
    {
        validation_lbl.Text = validationText;
    }

    //--------- Products Sort By Price ---------------
    List<ShopmateProduct> getSortByPriceProductsList(List<ShopmateProduct> walmartProducts, List<ShopmateProduct> bestBuyProducts, List<ShopmateProduct> targetProducts)
    {
        List<ShopmateProduct> allProducts = new List<ShopmateProduct>();
        allProducts.AddRange(walmartProducts);
        allProducts.AddRange(bestBuyProducts);
        allProducts.AddRange(targetProducts);

        allProducts = allProducts.OrderBy(x => x.priceInDecimal).ToList();

        return allProducts;
    }

    //--------- Products Sort By Most Relevant ---------------
    List<ShopmateProduct> getMostRelavantProductsList(List<ShopmateProduct> walmartProducts, List<ShopmateProduct> bestBuyProducts, List<ShopmateProduct> targetProducts)
    {
        List<ShopmateProduct> allProducts = new List<ShopmateProduct>();
        int walmartCount = walmartProducts.Count;
        int bestBuyCount = bestBuyProducts.Count;
        int targetCount = targetProducts.Count;

        int walmartCurrCount = 0, bestBuyCurrCount = 0, targetCurrCount = 0;

        int totalCount = walmartCount + bestBuyCount + targetCount;

        //Adding one product at a time from each stores.
        for (int i = 0; i < totalCount; i++, walmartCurrCount++, bestBuyCurrCount++, targetCurrCount++)
        {
            //Adding walmart items one at a time.
            if(walmartCurrCount < walmartCount)
            {
                allProducts.Add(walmartProducts[walmartCurrCount]);
            }

            //Adding bestBuy items one at a time.
            if (bestBuyCurrCount < bestBuyCount)
            {
                allProducts.Add(bestBuyProducts[bestBuyCurrCount]);
            }

            //Adding target items one at a time.
            if (targetCurrCount < targetCount)
            {
                allProducts.Add(targetProducts[targetCurrCount]);
            }
        }

        return allProducts;
    }
    #endregion
}