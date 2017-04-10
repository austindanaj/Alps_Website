<%@ Page Title="Phone Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PhoneCheckOut.aspx.cs" Inherits="CTBTeam.PhoneCheckOut" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <asp:Label ID="lblTitle" runat="server" Text="Phone Checkout" CssClass="lbl-main" ></asp:Label>
    </div>
    <div>
      
        <asp:Label ID="OS" runat="server" Text="Phone Type:" CssClass="lbl-os"></asp:Label>
        <asp:DropDownList ID="drpOs" runat="server" CssClass="drp-Model "></asp:DropDownList>
       <br /> 
        <br />
        <asp:Label ID="Model" runat="server" Text="Phone:" CssClass="lbl-phones"></asp:Label>
        <asp:DropDownList ID="drpPhone" runat="server" CssClass="drp-phones " ></asp:DropDownList>
    </div>


</asp:Content>
