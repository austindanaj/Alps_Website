﻿<%@ Page Title="Hours" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Hours.aspx.cs" Inherits="CTBTeam.Hours" %>

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
             <asp:Label ID="Label1" Text-Align="Center" runat="server" Text="Project Hours"/>
                 <br />
            <asp:Label ID="lblName1" runat="server" Text="Name:  " CssClass="lbl-purchase">
               <asp:DropDownList ID="ddlNamesProject" CssClass="drp-purchase" runat="server" OnSelectedIndexChanged="didSelectNameProject" AutoPostBack="true"></asp:DropDownList>
            </asp:Label>
            <br />
            <asp:Label ID="lblProject" runat="server" Text="Project:  " CssClass="lbl-purchase">
                <asp:DropDownList ID="ddlProjects" CssClass="drp-purchase" runat="server"></asp:DropDownList>
            </asp:Label>
            <br />
            <asp:Label ID="lblHour" runat="server" Text="Hours:  " CssClass="lbl-purchase">
                <asp:TextBox ID="txtHoursProjects" runat="server" CssClass="txt-purchase" Rows="1" BorderStyle="Solid" placeholder="0"></asp:TextBox>
            </asp:Label>
            <br />
             <asp:Button ID="btnSubmitProject" runat="server" OnClick="On_Click_Submit_Projects" Text="Submit" CssClass="btn btn-primary btn-lg" Text-Align="Center" />

            </div>
        <div class="col-md-6">
             <asp:Label ID="lblName2" runat="server" Text="Name:  " CssClass="lbl-purchase">
               <asp:DropDownList ID="ddlNamesCar" CssClass="drp-purchase" runat="server" OnSelectedIndexChanged="didSelectNameCar" AutoPostBack="true"></asp:DropDownList>
            </asp:Label>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Vehicle:  " CssClass="lbl-purchase">
                <asp:DropDownList ID="ddlCars" CssClass="drp-purchase" runat="server" ></asp:DropDownList>
            </asp:Label>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Hours:  " CssClass="lbl-purchase">
                <asp:TextBox ID="txtHoursCars" runat="server" CssClass="txt-purchase" Rows="1" BorderStyle="Solid" placeholder="0"></asp:TextBox>
            </asp:Label>
            <br />
             <asp:Button ID="btnSubmitCar" runat="server" OnClick="On_Click_Submit_Cars" Text="Submit" CssClass="btn btn-primary btn-lg" Text-Align="Center" />
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
