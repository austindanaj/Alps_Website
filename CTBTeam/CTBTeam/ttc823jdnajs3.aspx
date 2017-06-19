<%@ Page Title="Toe Truck" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ttc823jdnajs3.aspx.cs" Inherits="CTBTeam.ttc823jdnajs3" %>

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
			<asp:Image ID="toeTruck" runat="server" ImageUrl="~/Images/ToeTruck.png" CssClass="image_main" />
		</div>
		<div class="col-md-50">
			<h2>Before you continue, answer these questions:</h2>
			<br />
			<h4>(foldl + 2 '(8 6 4 4))</h4>
			<asp:TextBox ID="q1" runat="server" />
			<br />
			<h4>1 + 2 + 3 + 4 + ... (type your answer in lowercase)</h4>
			<asp:TextBox ID="q2" runat="server" />
			<br />
			<h4>TMs are not closed under</h4>
			<asp:TextBox ID="q3" runat="server" />
			<br />
			<h5>The best possible sort time for an algorithm is</h5>
			<asp:TextBox ID="q4" runat="server" />
			<br />
			<asp:Button ID="download" runat="server" OnClick="downloadToeTruck" CssClass="btn btn-default" Text="Continue" />
		</div>
	</div>
</asp:Content>
