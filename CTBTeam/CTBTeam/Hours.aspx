<%@ Page Title="Hours" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Hours.aspx.cs" Inherits="CTBTeam.Hours" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div >
        <asp:Label ID="lblTitle" runat="server" Text="CTB Time Tracking" CssClass="lbl main-title"></asp:Label>
    </div>
    <div>
        <asp:Label ID="lblWeekOf" runat="server" Text="Week Of: 1/16/2017" CssClass="lbl time-title"></asp:Label>
        <asp:Panel ID="Panel1" runat="server" Height="48px" Width="1536px">
            <br />

        </asp:Panel>
    </div>


    <br />
    <br />
    <br />    
    <div class="row">
        <div class="col-md-6">
            <asp:Label ID="lblName" runat="server" Text="Name:  " CssClass="lbl-purchase">
               <asp:DropDownList ID="ddlNames" CssClass="drp-purchase" runat="server"></asp:DropDownList>
            </asp:Label>
            <br />
            <asp:Label ID="lblProject" runat="server" Text="Project:  " CssClass="lbl-purchase">
                <asp:DropDownList ID="ddlProjects" CssClass="drp-purchase" runat="server"></asp:DropDownList>
            </asp:Label>
            <br />
            <asp:Label ID="lblHour" runat="server" Text="Number of Hours:  " CssClass="lbl-purchase">
                <asp:TextBox ID="txtHours" runat="server" CssClass="txt-purchase" Rows="1" BorderStyle="Solid" placeholder="0"></asp:TextBox>
            </asp:Label>

            </div>
        </div>
     <div class="row">
        <div class="col-md-4">
            <div class="form-group">
            </div>
          
        </div>
        <div class="col-md-4">
            <div style="text-align: center">
               
                <asp:Button ID="btnSubmit" runat="server" OnClick="On_Click_Submit" Text="Submit" CssClass="btn btn-primary btn-lg" Visible="false" Text-Align="Center" />
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
                   <asp:TextBox ID="txtName" runat="server" CssClass="txt-purchase" placeholder="Batteries"></asp:TextBox>
            </div>
        </div>
    </div>

     <br />
    <br />
      <br />
    <div class="row">

        <div class="col-md-6">
            <asp:GridView ID="dgvProject" runat="server" CssClass="gridview">
            </asp:GridView>

        </div>
    </div>
    <br />
    <br />

    <div class="row">
        <div class="col-md-6">
            <asp:GridView ID="dgvCars" runat="server" CssClass="gridview">
            </asp:GridView>
        </div>

    </div>
    <br />
    <br />
    <br />
  
   



</asp:Content>
