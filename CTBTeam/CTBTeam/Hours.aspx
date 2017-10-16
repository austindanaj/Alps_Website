<%@ Page Title="Hours" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Hours.aspx.cs" Inherits="CTBTeam.Hours" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="row">
		<asp:TextBox ID="txtSuccessBox" runat="server" Text="Success." Visible="false" ReadOnly="true" CssClass="feedback-textbox" />
	</div>
	<div class="row">
		<asp:LinkButton ID="btnSwitchView" runat="server" Text="Switch views" OnClick="htmlEvent" />
	</div>
	<div class="row">
		<div class="col-md-50">
			<asp:Label ID="lblWeekOf" runat="server" Text="Week Of: 0/00/0000" CssClass="lbl time-title" />
			<br />
			<asp:CheckBox ID="chkInactive" runat="server" Style="color: white;" Text="Show me inactive projects, old employees, etc." />
			<br />
			<asp:DropDownList ID="ddlselectWeek" CssClass="drp-home" runat="server" OnSelectedIndexChanged="htmlEvent" />
			<asp:Button ID="btnselectWeek" CssClass="btn-home" runat="server" Text="Go" Width="50px" OnClick="htmlEvent" />
		</div>
	</div>
	<br />
	<div class="row">
		<div class="col-md-33" style="text-align: center">
			<asp:Panel ID="pnlAddHours" runat="server">
				<asp:Label ID="Label2" runat="server" CssClass="lbl-home-title" Text="Project Percent" />
				<br />
				<br />
				<asp:DropDownList ID="ddlProjects" CssClass="drp-home" runat="server" />
				<br />
				<br />
				<asp:DropDownList ID="ddlHours" CssClass="drp-home" runat="server" />
				<br />
				<br />
				<asp:Button ID="btnSubmitPercent" runat="server" OnClick="htmlEvent" Text="Submit" CssClass="btn-home" Text-Align="Center" />
				<br />
				<asp:Label ID="lblUserHours" runat="server" ForeColor="White" Font-Size="X-Small" Text="Your Hours: 0/40" />
			</asp:Panel>
		</div>
		<div class="col-md-33" style="text-align: center">
			<asp:Panel ID="pnlVehicleHours" runat="server" Visible="false">
				<asp:Label ID="vehicleHoursTerns" runat="server" CssClass="lbl-home-title" Text="Vehicle Hours" Visible="false" />
				<br />
				<br />
				<asp:DropDownList ID="ddlVehicles" CssClass="drp-home" runat="server" Visible="false" />
				<br />
				<br />
				<asp:DropDownList ID="ddlHoursVehicles" runat="server" CssClass="drp-home" Visible="false" />
				<br />
				<br />
				<asp:Button ID="btnSubmitVehicles" runat="server" OnClick="htmlEvent" Text="Submit" CssClass="btn-home" Text-Align="Center" Visible="false" />
			</asp:Panel>
		</div>
		<div class="col-md-33" style="text-align: center;">
			<asp:Panel ID="pnlDeleteHours" runat="server" Visible="false">
				<asp:Label ID="Label3" runat="server" CssClass="lbl-home-title" Text="Delete hours" />
				<br />
				<br />
				<asp:DropDownList ID="ddlWorkedHours" CssClass="drp-home" runat="server" />
				<br />
				<br />
				<asp:TextBox ID="txtDelete" runat="server" CssClass="txt-purchase" placeholder="Type YES to confirm." />
				<br />
				<br />
				<asp:Button ID="btnDelete" runat="server" OnClick="htmlEvent" Text="Delete" CssClass="btn-home" Style="color: white; font: bold; background-color: #ff0000" Text-Align="Center" Visible="true" />
				<br />
				<br />
			</asp:Panel>
		</div>
	</div>
	<br />
	<div class="row">
		<asp:GridView ID="dgvProject" runat="server" CssClass="table_display table-striped table-bordered table-hover"
 OnRowDataBound="color" />
		<asp:GridView ID="dgvCars" runat="server" CssClass="table_display table-striped table-bordered table-hover"
 Visible="false" />
	</div>
	<br />
</asp:Content>
