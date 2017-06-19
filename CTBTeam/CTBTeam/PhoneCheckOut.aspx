<%@ Page Title="Phone Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PhoneCheckOut.aspx.cs" Inherits="CTBTeam.PhoneCheckOut" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<style type="text/css">
		body {
			background: url('Images/Gradient.jpg') no-repeat center center fixed;
			background-size: cover;
		}
	</style>
	<div class="row">
		<div>
			<asp:Label ID="lblTitle" runat="server" Text="Phone Checkout" CssClass="lbl-main"></asp:Label>
		</div>

		<div class="col-md-25">
			<asp:Label ID="Model" runat="server" Text="Phone:" CssClass="lbl-phones"></asp:Label>
			<asp:DropDownList ID="ddlPhones" runat="server" CssClass="drp-phones " AutoPostBack="true" />
			<br />
			<asp:Label ID="Cars" runat="server" CssClass="lbl-os" Text="Vehicle:"></asp:Label>
			<asp:DropDownList ID="ddlVehicles" runat="server" CssClass="drp-phones"></asp:DropDownList>
			<br />
			<asp:Label ID="Test" runat="server" Text="Purpose:" CssClass="lbl-os"></asp:Label>
			<asp:CheckBoxList ID="chkPurpose" CellPadding="5" CellSpacing="5" RepeatColumns="2" RepeatDirection="Vertical" RepeatLayout="Table" TextAlign="Right" runat="server" Style="color: White;">
				<asp:ListItem>Leakage</asp:ListItem>
				<asp:ListItem>Range</asp:ListItem>
				<asp:ListItem>Passive</asp:ListItem>
				<asp:ListItem>Extended Coverage</asp:ListItem>
				<asp:ListItem>8-blocks</asp:ListItem>
				<asp:ListItem>Calibration</asp:ListItem>
				<asp:ListItem>Other</asp:ListItem>
			</asp:CheckBoxList>
			<br />
			<asp:Label ID="To" runat="server" Text="I need the phone until:" CssClass="lbl-os" />
			<asp:DropDownList ID="ddlEnd" runat="server" CssClass="drp-phones" />
			<br />
			<br />
			<asp:Button ID="CheckOut" runat="server" Text="Check Out" CssClass="btn btn-primary btn-lg" OnClick="insert"/>
			<br />
			<br />
			<asp:Label ID="CheckIn" runat="server" Text="Check In (using ID):" CssClass="lbl-os" />
			<asp:DropDownList ID="ddlCheckIn" runat="server" CssClass="drp-phones" />
			<br />
			<br />
			<asp:Button ID="Button1" runat="server" Text="Check In" CssClass="btn btn-primary btn-lg" OnClick="checkIn"/>

		</div>

		<div class="col-md-50">
			<div class="form-gr">
				<br />
				<br />
				<asp:GridView ID="gvTable" runat="server" CssClass="gridview" HeaderStyle-BackColor="#000099"
					AlternatingRowStyle-CssClass="alt"
					PagerStyle-CssClass="pgr"
					HeaderStyle-ForeColor="White" RowStyle-BackColor="#A1DCF2" AlternatingRowStyle-BackColor="White"
					RowStyle-ForeColor="#3A3A3A" AutoGenerateColumns="true" AllowPaging="true" PageSize="25" Width="940px"/>
			</div>
		</div>
	</div>
</asp:Content>
