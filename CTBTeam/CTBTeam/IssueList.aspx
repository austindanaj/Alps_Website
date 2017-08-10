<%@ Page Title="Issue List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IssueList.aspx.cs" Inherits="CTBTeam.IssueList" %>

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
    <asp:TextBox ID="successOrFail" runat="server" Text="Success." Visible="false" ReadOnly="true" CssClass="feedback-textbox" />
    <asp:Button ID="btnEmail" runat="server" Text="Send Email" OnClick="btnSendEmail_Click" />


</asp:Content>
