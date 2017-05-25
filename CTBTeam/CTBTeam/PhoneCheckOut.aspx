<%@ Page Title="Phone Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PhoneCheckOut.aspx.cs" Inherits="CTBTeam.PhoneCheckOut" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<style type="text/css">
		body {
			background: url('Gradient.jpg') no-repeat center center fixed;
			
			
			
			background-size: cover;
		}
	</style>
	<div class="row">
		<div>
			<asp:Label ID="lblTitle" runat="server" Text="Phone Checkout" CssClass="lbl-main"></asp:Label>
		</div>

		<div class="col-md-25">

			<asp:Label ID="OS" runat="server" Text="Phone Type:" CssClass="lbl-os"></asp:Label>

			<asp:DropDownList ID="drpOs" runat="server" CssClass="drp-model " AutoPostBack="true" OnSelectedIndexChanged="onSelec"></asp:DropDownList>
			<br />
			<br />
			<asp:Label ID="Model" runat="server" Text="Phone:" CssClass="lbl-phones"></asp:Label>
			<asp:DropDownList ID="drpPhone" runat="server" CssClass="drp-phones " AutoPostBack="true" OnSelectedIndexChanged="onSelectPhone"></asp:DropDownList>

			<br />
			<br />
			<asp:Label ID="Person" runat="server" CssClass="lbl-os" Text="Person:"></asp:Label>
			<asp:DropDownList ID="drpPerson" runat="server" CssClass="drp-phones" OnSelectedIndexChanged="onSelectPerson" AutoPostBack="true"></asp:DropDownList>
			<br />
			<br />
			<asp:Label ID="Cars" runat="server" CssClass="lbl-os" Text="Vehicle:"></asp:Label>
			<asp:DropDownList ID="Vehicle" runat="server" CssClass="drp-phones"></asp:DropDownList>
			<br />
			<br />
			<asp:Label ID="Test" runat="server" Text="Purpose:" CssClass="lbl-os"></asp:Label>
			<br />
			<asp:CheckBoxList ID="cbl"
				AutoPostBack="True"
				CellPadding="5"
				CellSpacing="5"
				RepeatColumns="2"
				RepeatDirection="Vertical"
				RepeatLayout="Table"
				TextAlign="Right"
				runat="server"
				Style="color: White;">
				<asp:ListItem>Leakage</asp:ListItem>
				<asp:ListItem>Range</asp:ListItem>
				<asp:ListItem>Passive</asp:ListItem>
				<asp:ListItem>Coverage</asp:ListItem>
				<asp:ListItem>8-Blocks</asp:ListItem>
				<asp:ListItem>Calibration</asp:ListItem>
				<asp:ListItem>Other</asp:ListItem>

			</asp:CheckBoxList>
			<br />
			<asp:Label ID="From" runat="server" Text="From:" CssClass="lbl-os"></asp:Label>
			<asp:DropDownList ID="drpFrom" runat="server" CssClass="drp-phones"></asp:DropDownList>
			<br />
			<asp:Label ID="To" runat="server" Text="To:" CssClass="lbl-os"></asp:Label>
			<asp:DropDownList ID="drpTo" runat="server" CssClass="drp-phones"></asp:DropDownList>
			<br />
			<br />
			<asp:Button ID="CheckOut" runat="server" Text="Check Out" CssClass="btn btn-primary btn-lg" OnClick="clickCheckout"></asp:Button>
			<br />
			<br />
			<asp:Label ID="CheckIn" runat="server" Text="Check In:" CssClass="lbl-os"></asp:Label>

			<asp:DropDownList ID="drpCheckIn" runat="server" CssClass="drp-phones "></asp:DropDownList>
			<br />
			<br />
			<asp:Button ID="Button1" runat="server" Text="Check In" CssClass="btn btn-primary btn-lg" OnClick="clickCheckin" />

		</div>

		<div class="col-md-50">
			<div class="form-gr">

				<br />
				<br />
				<asp:GridView ID="gvTable" runat="server" CssClass="gridview" HeaderStyle-BackColor="#000099"
					AlternatingRowStyle-CssClass="alt"
					PagerStyle-CssClass="pgr"
					HeaderStyle-ForeColor="White" RowStyle-BackColor="#A1DCF2" AlternatingRowStyle-BackColor="White"
					RowStyle-ForeColor="#3A3A3A" AutoGenerateColumns="true" AllowPaging="true" PageSize="25"
					OnPageIndexChanging="OnPageIndexChanging2">
				</asp:GridView>

				<div style="text-align: center;">

					<br />
					<br />
				</div>

			</div>
		</div>

	</div>
</asp:Content>
