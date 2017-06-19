<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CTBTeam._Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<style type="text/css">
		body {
			background: url('Images/Gradient.jpg') no-repeat center center fixed;
			background-size: cover;
		}
	</style>
	<div class="row">
		<div class="col-md-50">
			<h1 style="font-weight: 700; font-size: 70px;">Project Hours</h1>
			<asp:DropDownList ID="ddlselectWeek" runat="server" CssClass="drp-home" />
			<p>
				<asp:Button runat="server" OnClick="download" Text="Download" CssClass="btn btn-default" />
			</p>
		</div>
		<div class="col-md-50">
			<asp:Image ID="Image1" runat="server" Height="300" Width="300" ImageUrl="~/Images/Globe.png" CssClass="image_main" />
		</div>
	</div>
	<br />
	<asp:Image ID="Image2" runat="server" ImageUrl="~/Images/CTBProjects.png" Width="100%" Height="650" />
	<br />
	<div class="col-md-50">
		<asp:Button runat="server" ID="toe" OnClick="toetruck" />
	</div>
	<br />
</asp:Content>

