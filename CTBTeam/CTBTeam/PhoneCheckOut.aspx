<%@ Page Title="Phone Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PhoneCheckOut.aspx.cs" Inherits="CTBTeam.PhoneCheckOut" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

        <div class="row">
    <div>
        <asp:Label ID="lblTitle" runat="server" Text="Phone Checkout" CssClass="lbl-main" ></asp:Label>
    </div>

    <div class="col-md-4">
      
        <asp:Label ID="OS" runat="server" Text="Phone Type:" CssClass="lbl-os"></asp:Label>

        <asp:DropDownList ID="drpOs" runat="server" CssClass="drp-model " AutoPostBack="true" OnSelectedIndexChanged="onSelec"></asp:DropDownList>
       <br /> 
        <br />
        <asp:Label ID="Model" runat="server" Text="Phone:" CssClass="lbl-phones"></asp:Label>
        <asp:DropDownList ID="drpPhone" runat="server" CssClass="drp-phones "  ></asp:DropDownList>

        <br />
        <br />

        <asp:Button ID="CheckIn" runat ="server" Text="Check Out" ></asp:Button>



    </div>

               <div class="col-md-6">
                <div class="form-gr">

                    <br />
                        <br />
                          <asp:ListView ID="List" runat="server"  ></asp:ListView>

                    <div style="text-align: center;">
                           <asp:Button ID="Button1" runat="server" Text="Add Time Off" CssClass="btn btn-primary btn-lg" />
                    <br />
                    <br />
                          <asp:Button ID="Button3" runat="server" Text="Check In" CssClass="btn btn-primary btn-lg" />
                    </div>
                 
                </div>
            </div>

</div>
</asp:Content>
