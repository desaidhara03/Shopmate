<%@ Page Title="Shopmate" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div id="header_banner">
            <hgroup class="title">
                <h1>Your Shopmate - make shopping simple</h1>
            </hgroup>
        </div>
        <div class="content-wrapper">
            <div id="search_control">
                <asp:TextBox ID="search_tb" runat="server" ToolTip="Enter any Product Name" CssClass="search_tb"></asp:TextBox>
                <asp:Button ID="search_btn" runat="server" Text="Search" CssClass="search_btn" OnClick="search_btn_Click" />
                <asp:RadioButtonList ID="sortby_rbl" runat="server" CssClass="sortby_rbl" RepeatLayout="Flow" OnSelectedIndexChanged="sortby_rbl_Selection_Changed" AutoPostBack="True">
                    <asp:ListItem Text="Most Relevant" Value="MR" Selected="True" />
                    <asp:ListItem Text="Price" Value="P" />
                </asp:RadioButtonList>
                <asp:Label ID="validation_lbl" CssClass="validation_lbl" runat="server" Text=""></asp:Label>
            </div>
        </div>

    </section>
</asp:Content>


<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:Panel ID="rootContainerPanel" runat="server">
        <!-- All results will come here -->
    </asp:Panel>
</asp:Content>
