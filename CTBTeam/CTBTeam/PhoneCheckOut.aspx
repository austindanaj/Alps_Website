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
        <asp:Label ID="Person" runat="server" CssClass="lbl-os" Text="Person:" ></asp:Label>
        <asp:TextBox ID ="getPerson" runat="server"  ></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="Cars" runat="server" CssClass="lbl-os" Text="Vehicle:"></asp:Label>
        <asp:DropDownList ID="Vehicle" runat="server" CssClass="drp-phones"></asp:DropDownList>
        <br />
        <br />
        <asp:Label ID="Test" runat="server" Text="Purpose:" CssClass="lbl-os"></asp:Label>
        <br />
        <asp:CheckBoxList ID="cbl" 
           AutoPostBack="True"
           CellPadding="5"
           CellSpacing="5"
           RepeatColumns="2"
           RepeatDirection="Vertical"
           RepeatLayout="Table"
           TextAlign="Right"
     
           runat="server">
         <asp:ListItem>Leakage</asp:ListItem>
         <asp:ListItem>Range</asp:ListItem>
         <asp:ListItem>Passive</asp:ListItem>
         <asp:ListItem>Coverage</asp:ListItem>
         <asp:ListItem>8-Blocks</asp:ListItem>
         <asp:ListItem>Calibration</asp:ListItem>

      </asp:CheckBoxList>
        <br />
        <asp:Button ID="CheckOut" runat ="server" Text="Check Out" CssClass="btn btn-primary btn-lg" OnClick="clickCheckout" ></asp:Button>
        <br />
        <br />
        <asp:Label ID="CheckIn" runat="server" Text="Check In:" CssClass="lbl-os"></asp:Label>
 
        <asp:DropDownList ID="drpCheckIn" runat="server" CssClass="drp-phones "  ></asp:DropDownList>
            <br />
        <br />
           <asp:Button ID="Button1" runat="server" Text="Check In" CssClass="btn btn-primary btn-lg" />

    </div>

               <div class="col-md-6">
                <div class="form-gr">

                    <br />
                        <br />
                        <asp:GridView ID="gvTable" runat="server"  CssClass="gridview"></asp:GridView>

                    <div style="text-align: center;">
                    
                    <br />
                    <br />
                    </div>
                 
                </div>
            </div>

</div>
</asp:Content>
