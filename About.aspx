<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="About" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %> ShopHopper</h1>
    </hgroup>

    <article>
        <p>        
            ShopHopper is a great place which makes the shopping easy by giving the best deal of a product, by comparing the prices on various websites and provides best one stop shopping solution.
        </p>

        <p>        
            ShopHopper has a simple search page, which takes product name as an input from the user, searches all major online retailers for that product and will display consolidated results on one page, which can be sorted by price or by store. That way user can know which retailer provides that product with the least price. Each product tile will contain the product name, price, image, store name. By clicking on the tile, will take the user to particular retailer’s site, where use can make purchase. Thus, user saves lot of time for searching the best price all over the internet.
        </p>
    </article>

    <%--<aside>
        <h3>Aside Title</h3>
        <p>        
            Use this area to provide additional information.
        </p>
        <ul>
            <li><a runat="server" href="~/">Home</a></li>
            <li><a runat="server" href="~/About">About</a></li>
            <li><a runat="server" href="~/Contact">Contact</a></li>
        </ul>
    </aside>--%>
</asp:Content>