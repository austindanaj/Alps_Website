<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CTBTeam._Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<style type="text/css">
		body {
			background: url('Gradient.jpg') no-repeat center center fixed;
			background-size: cover;
		}
	</style>
	<div class="row">
		<div class="col-md-50">
			<h1 style="font-weight: 700; font-size: 70px;">Project Hours</h1>
			<p>
				<asp:Button runat="server" OnClick=" View_More_onClick" Text="Click Here" CssClass="btn btn-primary btn-lg" />
			</p>
		</div>
		<div class="col-md-50">
			<asp:Image ID="Image1" runat="server" Height="300" Width="300" ImageUrl="~/Globe.png" CssClass="image_main" />
		</div>
	</div>
	<br />
	<asp:Image ID="Image2" runat="server" ImageUrl="~/CTBProjects.png" Width="100%" Height="650" />

	<div class="row">
		<div class="col-md-25">
			<h2>Current Hours</h2>
			<p>
				<asp:Button runat="server" OnClick="download_database" Text="Download" CssClass="btn btn-default" />
			</p>
		</div>
		<div class="col-md-25">
			<h2>Time Log</h2>
			<p>
				<asp:Button runat="server" OnClick="download_timelog" Text="Download" CssClass="btn btn-default" />
			</p>
		</div>
		<div class="col-md-25">
			<h2>Hex Generator</h2>
			<p>
				<asp:Button runat="server" OnClick="download_file_hexGenerator"
					Text="Download" CssClass="btn btn-default" />
			</p>
		</div>
		<div class="col-md-25">
			<h2>Phone Log</h2>
			<p>
			</p>
			<p>
				<asp:Button runat="server" OnClick="download_Phones_file"
					Text="Download" CssClass="btn btn-default" />
			</p>
		</div>
	</div>
</asp:Content>

