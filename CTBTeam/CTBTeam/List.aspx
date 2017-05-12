<%@ Page Title="Purchase List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="CTBTeam.List" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
         <style type="text/css">
        body {
            background: url('Gradient.jpg') no-repeat center center fixed;
            -webkit-background-size: cover;
            -moz-background-size: cover;
            -o-background-size: cover;
            background-size: cover;
           
        }
    </style>
    <br />
    <br />
    <div class="row">
        <div class="col-md-50">
            <asp:Label ID="lblName" runat="server" Text="Item Name:  " CssClass="lbl-purchase">
                <asp:TextBox ID="txtName" runat="server" CssClass="txt-purchase" placeholder="Batteries"></asp:TextBox>
            </asp:Label>
            <br />
            <asp:Label ID="lblQuant" runat="server" Text="Quantity:  " CssClass="lbl-purchase">
                <asp:TextBox ID="txtQuant" runat="server" CssClass="txt-purchase" placeholder="10"></asp:TextBox>
            </asp:Label>
            <br />
            <asp:Label ID="lblDesc" runat="server" Text="Description:  " CssClass="lbl-purchase">
                <asp:TextBox ID="txtDesc" runat="server" CssClass="txt-purchase-mult" Rows="6" TextMode="MultiLine" BorderStyle="Solid" placeholder="Coin cell batteries for Global A and Project B"></asp:TextBox>
            </asp:Label>
            <br />
            <asp:Label ID="lblPrice" runat="server" Text="Price:  " CssClass="lbl-purchase">
                <asp:TextBox ID="txtPrice" runat="server" CssClass="txt-purchase" placeholder="62.99"></asp:TextBox>
            </asp:Label>
            <br />
            <asp:Label ID="lblPrio" runat="server" Text="Priority:  " CssClass="lbl-purchase">
                <asp:DropDownList ID="drpPrio" runat="server" CssClass="drp-purchase">
                    <asp:ListItem Text="Low"></asp:ListItem>
                    <asp:ListItem Text="Medium"></asp:ListItem>
                    <asp:ListItem Text="High"></asp:ListItem>
                </asp:DropDownList>

            </asp:Label>
            <br />
            <asp:Label ID="lblLink" runat="server" Text="Link:  " CssClass="lbl-purchase">
                <asp:TextBox ID="txtLink" runat="server" CssClass="txt-purchase" placeholder="www.websitetobuy.com/pagelink"></asp:TextBox>
            </asp:Label>
            <br />
            <div style="text-align: center;">
                <asp:Button ID="btnSubmit" runat="server" Text="Add Item" CssClass="btn btn-primary btn-lg" OnClick="btnSubmit_Click" />
            </div>
        </div>
        <br />
        <br />
        <div class="col-md-50">
            <asp:GridView ID="grdList" runat="server" CssClass="gridview"></asp:GridView>
        </div>
    </div>



</asp:Content>
