<%@ Page Title="Purchase List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="CTBTeam.List" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<style type="text/css">
		body {
			background: url('Gradient.jpg') no-repeat center center fixed;
			background-size: cover;
		}
	</style>

	<asp:TextBox ID="successOrFail" runat="server" Text="Success." Visible="false" ReadOnly="true" CssClass="feedback-textbox" />
	<div class="row">
		<br />
		<div class="col-md-33">
			<asp:Label ID="lblName" runat="server" Text="Item Name: " CssClass="lbl-purchase">
				<asp:TextBox ID="txtName" runat="server" CssClass="txt-purchase" placeholder="Batteries"></asp:TextBox>
			</asp:Label>
			<br />
			<asp:Label ID="lblQuant" runat="server" Text="Quantity: " CssClass="lbl-purchase">
				<asp:TextBox ID="txtQuant" runat="server" CssClass="txt-purchase" placeholder="10"></asp:TextBox>
			</asp:Label>
			<br />
			<asp:Label ID="lblDesc" runat="server" Text="Description: " CssClass="lbl-purchase">
				<asp:TextBox ID="txtDesc" runat="server" CssClass="txt-purchase-mult" Rows="6" TextMode="MultiLine" BorderStyle="Solid" placeholder="Coin cell batteries for Global A and Project B" />
			</asp:Label>
		</div>
		<div class="col-md-33">
			<asp:Label ID="lblPrice" runat="server" Text="Price: " CssClass="lbl-purchase">
				<asp:TextBox ID="txtPrice" runat="server" CssClass="txt-purchase" placeholder="62.99"></asp:TextBox>
			</asp:Label>
			<br />
			<asp:Label ID="lblPrio" runat="server" Text="Priority: " CssClass="lbl-purchase">
				<asp:DropDownList ID="ddlPriority" runat="server" CssClass="drp-purchase" />
			</asp:Label>
			<br />
			<asp:Label ID="lblLink" runat="server" Text="Link: " CssClass="lbl-purchase">
				<asp:TextBox ID="txtLink" runat="server" CssClass="txt-purchase" placeholder="www.websitetobuy.com/pagelink"></asp:TextBox>
				<asp:Button ID="btnSubmit" runat="server" Text="Add Item" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
			</asp:Label>
			<br />
		</div>
		<div class="col-md-33">
			<asp:Label ID="lblPurchase" runat="server" CssClass="lbl-purchase" Text="Purchase: ">
				<asp:TextBox ID="txtPurchase" runat="server" CssClass="txt-purchase" placeholder="ID of what you bought"/>
				<asp:Button ID="btnPurchase" runat="server" CssClass=" btn btn-primary" OnClick="purchase" Text="Purchase"/>
			</asp:Label>
			<br />
		</div>
	</div>
	<br />
	<asp:GridView ID="grdList" runat="server" CssClass="gridview" Width="1170px" />


</asp:Content>
