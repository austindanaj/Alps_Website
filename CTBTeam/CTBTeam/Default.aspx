<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CTBTeam._Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="row">
		<div class="col-md-25">
			<h1 style="font-weight: 700; font-size: 50px;">Project Hours</h1>
			<asp:DropDownList ID="ddlselectWeek" runat="server" CssClass="drp-home" /><br />
			<asp:Button runat="server" OnClick="download" Text="Download" CssClass="btn btn-default" />
		</div>
		<div class="col-md-50">
			<asp:Chart ID="chartPercent" runat="server" BackColor="Transparent" EnableViewState="true"
				BorderlineWidth="0" Height="360px" Palette="None" PaletteCustomColors="Maroon"
				Width="700px" BorderlineColor="64, 0, 64">
				<Titles>
					<asp:Title ShadowOffset="10" Name="Project Percent" />
				</Titles>
				<Legends>
					<asp:Legend Alignment="Center" Docking="Right" IsTextAutoFit="False" Name="Default" BackColor="Transparent" ForeColor="White" LegendStyle="Column" />
				</Legends>
				<Series>
					<asp:Series Name="Default" />
				</Series>
				<ChartAreas>
					<asp:ChartArea Name="ChartArea1" BorderWidth="0" BackColor="Transparent" />
				</ChartAreas>
			</asp:Chart>
		</div>
		<div class="col-md-25">
			<asp:Image ID="Image1" runat="server" Height="300" Width="300" ImageUrl="~/Images/Globe.png" CssClass="image_main" />
		</div>
	</div>
	<br />
	<div class="row">
		<div class="col-md-50">
			<h1 style="font-weight: 700; font-size: 50px;">Intern schedule</h1>
			<asp:DropDownList ID="ddlSelectScheduleDay" runat="server" CssClass="drp-home" OnSelectedIndexChanged="changeScheduleDay" AutoPostBack="true">
				<asp:ListItem Text="Monday" />
				<asp:ListItem Text="Tuesday" />
				<asp:ListItem Text="Wednesday" />
				<asp:ListItem Text="Thursday" />
				<asp:ListItem Text="Friday" />
			</asp:DropDownList>
		</div>
	</div>
	<br />
	<div class="row">
		<div class="col-md-80">
			<asp:GridView ID="dgvSchedule" runat="server" CssClass="gridview" OnRowDataBound="color" />
		</div>
		<div class="col-md-20">
			<asp:GridView ID="dgvOffThisWeek" runat="server" CssClass="gridview" />
		</div>
	</div>
	<br />
	<asp:Button runat="server" ID="toe" OnClick="toetruck" />
</asp:Content>
