<%@ Page Title="Purchase List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="CTBTeam.List" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<asp:TextBox ID="txtSuccessBox" runat="server" Text="Success." Visible="false" ReadOnly="true" CssClass="feedback-textbox" />
	<div class="row">
		<div class="col-md-25">
			<h2>Add new item</h2>
			<br />
			<asp:TextBox ID="txtName" runat="server" CssClass="txt-purchase" placeholder="Item name"></asp:TextBox>
			<br />
			<asp:TextBox ID="txtQuant" runat="server" CssClass="txt-purchase" placeholder="Quantity" />
			<br />
			<asp:TextBox ID="txtDesc" runat="server" CssClass="txt-purchase-mult" Rows="6" TextMode="MultiLine" BorderStyle="Solid" placeholder="Description" />
			<br />
			<asp:TextBox ID="txtPrice" runat="server" CssClass="txt-purchase" placeholder="Price" />
			<br />
			<asp:DropDownList ID="ddlPriority" runat="server" CssClass="drp-purchase">
				<asp:ListItem Text="-- Select a priority level --" />
				<asp:ListItem Text="Low - 1" />
				<asp:ListItem Text="Medium - 2" />
				<asp:ListItem Text="High - 3" />
			</asp:DropDownList>
			<br />
			<asp:TextBox ID="txtLink" runat="server" CssClass="txt-purchase" placeholder="URL" />
			<br />
			<asp:Button ID="btnSubmit" runat="server" Text="Add Item" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
			<br />
			<br />
			<h2>Purchase item</h2>
			<br />
			<asp:TextBox ID="txtPurchase" runat="server" CssClass="txt-purchase" placeholder="ID of what you bought" />
			<br />
			<asp:Button ID="btnPurchase" runat="server" CssClass=" btn btn-primary" OnClick="purchase" Text="Purchase" />
			<br />
		</div>
		<br />
		<div class="col-md-75">
			<asp:GridView ID="grdList" runat="server" CssClass="gridview" Width="1170px" />
		</div>
	</div>
</asp:Content>
