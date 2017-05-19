<%@ Page Title="Toe Truck" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ToeTruck.aspx.cs" Inherits="CTBTeam._Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

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
	<div class="row" >
		<div class="col-md-50">
			<asp:Image ID="toeTruck" runat="server" ImageUrl="~/ToeTruck.THEFILETYPE" CssClass="image_main" />
		</div>
		<div class="col-md-50">
			Get a copy <br />
			<asp:Button ID="download" runat="server" OnClick="downloadToeTruck" CssClass="btn btn-default" Text="Download" />
		</div>
	</div>
</asp:Content>