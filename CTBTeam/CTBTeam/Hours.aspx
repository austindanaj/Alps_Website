<%@ Page Title="Hours" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Hours.aspx.cs" Inherits="CTBTeam.Hours" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">


        <div class="col-md-6">
            <asp:Label ID="lblTitle" runat="server" Text="CTB Time Tracking" CssClass="lbl main-title"></asp:Label>
        </div>
        <div class="col-md-6" style="text-align: right;">
        </div>
    </div>
    <div>

        <br />
        <asp:Label ID="lblWeekOf" runat="server" Text="Week Of: 1/16/2017" CssClass="lbl time-title"></asp:Label>
        <asp:Panel ID="Panel1" runat="server" Height="48px" Width="1536px">
            <br />
        </asp:Panel>
    </div>
    <br />
    <br />
    <br />

    <div class="row">
        <div class="col-md-4" style="text-align: center;">

            <asp:Label ID="Label1" runat="server" CssClass="lbl-home-title" Text="Project Hours" />
            <br />
            <br />
            <asp:DropDownList ID="ddlNamesProject" CssClass="drp-home" runat="server" OnSelectedIndexChanged="didSelectNameProject" AutoPostBack="true"></asp:DropDownList>
            <br />
            <br />
            <asp:DropDownList ID="ddlProjects" CssClass="drp-home" runat="server"></asp:DropDownList>
            <br />
            <br />
            <asp:TextBox ID="txtHoursProjects" runat="server" CssClass="txt-home" Rows="1" BorderStyle="Solid" placeholder="0"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnSubmitProject" runat="server" OnClick="On_Click_Submit_Projects" Text="Submit" CssClass="btn-home" Text-Align="Center" />

        </div>
        <div class="col-md-6">

            <asp:GridView ID="dgvProject" runat="server" Width="300" HeaderStyle-BackColor="#3AC0F2"
                CssClass="Grid"
                AlternatingRowStyle-CssClass="alt"
                PagerStyle-CssClass="pgr"
                HeaderStyle-ForeColor="White" RowStyle-BackColor="#A1DCF2" AlternatingRowStyle-BackColor="White"
                RowStyle-ForeColor="#3A3A3A" AutoGenerateColumns="true" AllowPaging="true" PageSize="10"
                OnPageIndexChanging="OnPageIndexChanging1">
            </asp:GridView>

        </div>
    </div>
    <br />
    <br />
    <br />
    <div class="row">
        <div class="col-md-4" style="text-align: center;">

            <asp:Label ID="Label4" runat="server" CssClass="lbl-home-title" Text="Vehicle Hours" />
            <br />
            <br />
            <asp:DropDownList ID="ddlNamesCar" CssClass="drp-home" runat="server" OnSelectedIndexChanged="didSelectNameCar" AutoPostBack="true"></asp:DropDownList>
            <br />
            <br />
            <asp:DropDownList ID="ddlCars" CssClass="drp-home" runat="server"></asp:DropDownList>
            <br />
            <br />
            <asp:TextBox ID="txtHoursCars" runat="server" CssClass="txt-home" Rows="1" BorderStyle="Solid" placeholder="0"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnSubmitCar" runat="server" OnClick="On_Click_Submit_Cars" Text="Submit" CssClass="btn-home" Text-Align="Center" />

        </div>


        <div class="col-md-6">

            <asp:GridView ID="dgvCars" runat="server" Width="300" HeaderStyle-BackColor="#3AC0F2"
                CssClass="Grid"
                AlternatingRowStyle-CssClass="alt"
                PagerStyle-CssClass="pgr"
                HeaderStyle-ForeColor="White" RowStyle-BackColor="#A1DCF2" AlternatingRowStyle-BackColor="White"
                RowStyle-ForeColor="#3A3A3A" AutoGenerateColumns="true" AllowPaging="true" PageSize="10"
                OnPageIndexChanging="OnPageIndexChanging2">
            </asp:GridView>

        </div>

    </div>
    <br />
    <br />
    <br />





</asp:Content>
