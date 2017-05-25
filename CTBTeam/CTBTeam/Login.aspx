<%@ Page Title="Sign In" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CTBTeam.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<style type="text/css">
		body {
			background: url('Gradient.jpg') no-repeat center center fixed;
			background-size: cover;
		}
	</style>
	<br />


	<div class="login-page">
		<div class="login">

			<div class="login-form">

				<asp:Label ID="lblLogin" runat="server" Text="Sign In As:" CssClass="username"></asp:Label>
				<br />
				<br />
				<asp:DropDownList ID="ddl" runat="server" CssClass="drp-login"/>
				<br />
				<br />
				<br />
				<asp:TextBox ID="txtUser" runat="server" placeholder="username"/>
				<asp:TextBox ID="txtPass" runat="server" placeholder="password" TextMode="Password"></asp:TextBox>
				<asp:Button ID="btnLogin" runat="server" OnClick="Login_Clicked" Text="Sign In"></asp:Button>
			</div>
		</div>
	</div>





</asp:Content>
