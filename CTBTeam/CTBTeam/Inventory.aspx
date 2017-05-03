<%@ Page Title="Inventory" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inventory.aspx.cs" Inherits="CTBTeam.Inventory" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

        <div class="row">
    <div>
        <asp:Label ID="lblTitle" runat="server" Text="Inventory" CssClass="lbl-main" ></asp:Label>
    </div>

      <div>
          <asp:Label ID="lblSearch" runat="server" Text="Search" CssClass="lbl-os"></asp:Label>
          <asp:TextBox ID="search" runat="server" CssClass="txt-purchase"
              ></asp:TextBox>
      </div>
            <div>
                <br />
                <asp:Button ID="Add" runat="server" Text="Add Item" CssClass="btn btn-primary btn-lg" />

            </div>
            <div>
                <asp:Label ID="Manufacture" runat="server" Text="Manufacture ID:" CssClass="lbl-os"></asp:Label>
                <asp:TextBox ID="txtManufacture" runat="server" placeholder="Manufacture ID" CssClass="txt-purchase"></asp:TextBox>
                <br />
                  <asp:Label ID="Quantity" runat="server" Text="Quantity" CssClass="lbl-os"></asp:Label>
                <asp:TextBox ID="txtquantity" runat="server" placeholder="Quantity" CssClass="txt-purchase" ></asp:TextBox>
            </div>
</div>
</asp:Content>
