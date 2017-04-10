<%@ Page Title="Phone Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PhoneCheckOut.aspx.cs" Inherits="CTBTeam.PhoneCheckOut" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <asp:Label ID="lblTitle" runat="server" Text="Phone Checkout" CssClass=".lbl-MainPhone" ></asp:Label>
    </div>
    <div>
      
        <asp:Label ID="lblWeekOf" runat="server" Text="Week Of: 1/16/2017" CssClass="lbl time-title"></asp:Label>
        <asp:Panel ID="Panel1" runat="server" Height="48px" Width="1536px">
            <br />

        </asp:Panel>
    </div>


</asp:Content>
