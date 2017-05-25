<%@ Page Title="Admin" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="CTBTeam.Admin" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<style type="text/css">
		body {
			background: url('Gradient.jpg') no-repeat center center fixed;
			
			
			
			background-size: cover;
		}
	</style>
	<div class="row">
		<asp:TextBox ID="successOrFail" runat="server" Text="Success." Visible="false" ReadOnly="true" CssClass="feedback-textbox" />
	</div>
	<div class="row">
		<br />
		<div class="col-md-33">
			<asp:Label ID="lblName" runat="server" Text="Add User" CssClass="username"></asp:Label>
			<br />
			<asp:TextBox Class="txt-admin" ID="txtAlna" runat="server" placeholder="Alna number"></asp:TextBox>
			<br />
			<br />
			<asp:TextBox Class="txt-admin" ID="txtName" runat="server" placeholder="First and Last Name"></asp:TextBox>
			<br />
			<asp:CheckBox ID="chkPartTime" runat="server" Text="Part Time" Style="color: white" />
			<br />
			<asp:Button Class="btn-home" Width="100px" ID="btnName" runat="server" OnClick="User_Clicked" Text="Add User"></asp:Button>
		</div>
		<div class="col-md-33">
			<asp:Label ID="lblProject" runat="server" Text="Add Project" CssClass="username"></asp:Label>
			<br />
			<asp:TextBox Class="txt-admin" ID="txtProject" runat="server" placeholder="Project Name"></asp:TextBox>
			<asp:RadioButtonList ID="category" runat="server" CssClass="radio">
				<asp:ListItem Text="A – Advanced Development Project" />
				<asp:ListItem Text="B – Time Off" />
				<asp:ListItem Text="C – Production Development" />
				<asp:ListItem Text="D – Design in Market (Non-Auto)" />
			</asp:RadioButtonList>
			<asp:Button Class="btn-home" Width="120px" ID="btnProject" runat="server" OnClick="Project_Clicked" Text="Add Project"></asp:Button>
		</div>
		<div class="col-md-33">
			<asp:Label ID="lblCar" runat="server" Text="Add Vehicle" CssClass="username"></asp:Label>
			<br />
			<asp:TextBox Class="txt-admin" ID="txtCar" runat="server" placeholder="Vehicle Name"></asp:TextBox>
			<br />
			<br />
			<asp:Button Class="btn-home" Width="120px" ID="btnCar" runat="server" OnClick="Car_Clicked" Text="Add Vehicle"></asp:Button>
		</div>
	</div>
	<br />
	<div class="row">
		<div class ="col-md-33">
			<asp:Label ID="lblNR" runat="server" Text="Remove User" CssClass="username"></asp:Label>
			<br />
			<asp:TextBox Class="txt-admin" ID="txtRemoveUser" runat="server" placeholder="Alna_num" />
			<br />
			<br />
			<asp:Button Class="btn-home" Width="115px" ID="btnRemoveUser" runat="server" OnClick="remove" Text="Remove User"></asp:Button>
		</div>
		<div class="col-md-33">
			<asp:Label ID="lblPR" runat="server" Text="Remove Project" CssClass="username"></asp:Label>
			<asp:TextBox Class="txt-admin" ID="txtRemoveProject" runat="server" placeholder="Project ID"></asp:TextBox>
			<br />
			<br />
			<asp:Button Class="btn-home" Width="128px" ID="btnRemoveProject" runat="server" OnClick="remove" Text="Remove Project"></asp:Button>
		</div>
		<div class="col-md-33">
			<asp:Label ID="lblCR" runat="server" Text="Remove Vehicle" CssClass="username"></asp:Label>
			<br />
			<asp:TextBox Class="txt-admin" ID="txtRemoveVehicle" runat="server" placeholder="Vehicle ID"></asp:TextBox>
			<br />
			<br />
			<asp:Button Class="btn-home" Width="140px" ID="btnRemoveVehicle" runat="server" OnClick="remove" Text="Remove Vehicle"></asp:Button>
		</div>
	</div>
	<div class="row">
		<br />
		<div class="col-md-33">
			<asp:GridView ID="dgvUsers" runat="server" Width="350" HeaderStyle-BackColor="#3AC0F2"
				CssClass="gridview"
				AlternatingRowStyle-CssClass="alt"
				AutoGenerateColumns="true">
			</asp:GridView>
		</div>
		<div class="col-md-33">
			<asp:GridView ID="dgvProjects" runat="server" Width="300" HeaderStyle-BackColor="#3AC0F2"
				CssClass="gridview"
				AlternatingRowStyle-CssClass="alt"
				AutoGenerateColumns="true">
			</asp:GridView>
		</div>
		<div class="col-md-33">
			<asp:GridView ID="dgvCars" runat="server" Width="200" HeaderStyle-BackColor="#3AC0F2"
				CssClass="gridview"
				AutoGenerateColumns="true">
			</asp:GridView>
		</div>
		<br />
	</div>
</asp:Content>