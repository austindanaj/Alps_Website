<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CTBTeam._Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="row">
		<div class="col-md-50">
			<h1 style="font-weight: 700; font-size: 70px;">Project Hours</h1>
			<asp:DropDownList ID="ddlselectWeek" runat="server" CssClass="drp-home" /><br />
			<asp:Button runat="server" OnClick="download" Text="Download" CssClass="btn btn-default" />
		</div>
		<div class="col-md-50">
			<asp:Image ID="Image1" runat="server" Height="300" Width="300" ImageUrl="~/Images/Globe.png" CssClass="image_main" />
		</div>
	</div>
	<br />
	<div class="col-md-50">
		<asp:GridView ID="dgvOffThisWeek" runat="server" CssClass="gridview" />
	</div>
	<div class="row">
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
	</div>
	<div class="row">
		<asp:GridView ID="dgvMonday" runat="server" CssClass="gridview" />
		<asp:GridView ID="dgvTuesday" runat="server" CssClass="gridview" />
		<asp:GridView ID="dgvWednesday" runat="server" CssClass="gridview" />
		<asp:GridView ID="dgvThursday" runat="server" CssClass="gridview" />
		<asp:GridView ID="dgvFriday" runat="server" CssClass="gridview" />
		<br />
		<asp:Button runat="server" ID="toe" OnClick="toetruck" />
	</div>
</asp:Content>
