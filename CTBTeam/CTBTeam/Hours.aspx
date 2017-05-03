<%@ Page Title="Hours" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Hours.aspx.cs" Inherits="CTBTeam.Hours" %>

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
    <div class="row">


        <div class="col-md-6">
            <asp:Label ID="lblTitle" runat="server" Text="CTB Time Tracking" CssClass="lbl main-title"></asp:Label>
        </div>
        <div class="col-md-6" style="text-align: right;">
                 <%-- The css you want still needs to be selected for the next two lines --%>
        <asp:TextBox ID="searchbox" runat="server" TextMode="SingleLine" maxlength="30" tooltip="Enter what you would like to search" placeholder="Search" />
        <asp:DropDownList ID="searchDropDown" runat="server" />
        <asp:Button ID="searchboxbutton" runat="server" Text="Search" OnClick="On_Click_Search_DB" />
        </div>
    </div>
    <div>

        <br />
        <asp:Label ID="lblWeekOf" runat="server" Text="Week Of: 0/00/0000" CssClass="lbl time-title"></asp:Label>
        <asp:Panel ID="Panel1" runat="server" Height="48px" Width="1536px">
            <br />
        </asp:Panel>
    </div>
    <br />
    <br />
    <br />
    <div class="row">
        <div class="col-md-4" style="text-align: center;">

            <asp:Label ID="Label2" runat="server" CssClass="lbl-home-title" Text="Project Percent" />
            <br />
            <br />
            <asp:DropDownList ID="ddlAllNames" CssClass="drp-home" runat="server" OnSelectedIndexChanged="didSelectNamesPercent" AutoPostBack="true"></asp:DropDownList>
            <br />
            <br />
            <asp:DropDownList ID="ddlAllProjects" CssClass="drp-home" runat="server" OnSelectedIndexChanged="didSelectProjectsPercent" AutoPostBack="true"></asp:DropDownList>
            <br />
            <br />
             <asp:DropDownList ID="ddlPercentage" CssClass="drp-home" runat="server"></asp:DropDownList>
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" OnClick="On_Click_Submit_Percent" Text="Submit" CssClass="btn-home" Text-Align="Center" />

        </div>
        <div class="col-md-6">
                <asp:Chart ID="chartPercent" runat="server" BackColor="Transparent" EnableViewState="true"
                BorderlineWidth="0" Height="360px" Palette="None" PaletteCustomColors="Maroon"
                Width="700px" BorderlineColor="64, 0, 64" >
                <Titles>
                    <asp:Title ShadowOffset="10" Name="Project Percent" />
                </Titles>
                <Legends>
                    <asp:Legend Alignment="Center" Docking="Right" IsTextAutoFit="False" Name="Default"
                         BackColor="Transparent" ForeColor="White" LegendStyle="Column"  />
                </Legends>
                <Series>
                    <asp:Series Name="Default" 
                        />
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1" BorderWidth="0" BackColor="Transparent" />
                   
                </ChartAreas>
            </asp:Chart>

          

        </div>
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
              <asp:GridView ID="dgvProject" runat="server" CssClass="Grid" HeaderStyle-BackColor="#000099"
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

            <asp:GridView ID="dgvCars" runat="server" CssClass="Grid" HeaderStyle-BackColor="#000099"
                    AlternatingRowStyle-CssClass="alt"
                    PagerStyle-CssClass="pgr"
                    HeaderStyle-ForeColor="White" RowStyle-BackColor="#A1DCF2" AlternatingRowStyle-BackColor="White"
                    RowStyle-ForeColor="#3A3A3A" AutoGenerateColumns="true" AllowPaging="true"  PageSize="10"
                OnPageIndexChanging="OnPageIndexChanging2">
            </asp:GridView>

        </div>

    </div>
    <br />
    <br />
    <br />





</asp:Content>
