<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="CTBTeam.Schedule" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="row">
		<div class="col-md-80">
			<h1 style="font-weight: 700; font-size: 50px;">Intern schedule</h1>
			<asp:DropDownList ID="ddlSelectScheduleDay" runat="server" CssClass="ddl-time" OnSelectedIndexChanged="changeScheduleDay" AutoPostBack="true">
				<asp:ListItem Text="Monday" />
				<asp:ListItem Text="Tuesday" />
				<asp:ListItem Text="Wednesday" />
				<asp:ListItem Text="Thursday" />
				<asp:ListItem Text="Friday" />
			</asp:DropDownList>
		</div>
		<br />
		<div class="col-md-20">
			<h1 style="float: right; text-align: left;">Add schedule entry for yourself</h1>
		</div>
	</div>
	<div class="row">
		<div class="col-md-80">
			<asp:GridView ID="dgvSchedule" runat="server" CssClass="gridview" OnRowDataBound="color" />
		</div>
		<div class="col-md-20">
			<div class="row">
				<asp:DropDownList ID="ddlDay" runat="server" CssClass="ddl-time">
					<asp:ListItem Text="Monday" />
					<asp:ListItem Text="Tuesday" />
					<asp:ListItem Text="Wednesday" />
					<asp:ListItem Text="Thursday" />
					<asp:ListItem Text="Friday" />
				</asp:DropDownList>
			</div>
			<div class="row">
				<asp:TextBox ID="txtStartTime" runat="server" CssClass="txt-time" placeholder="HH:mm" MaxLength="5" />
				<asp:DropDownList ID="ddlStartAmPm" runat="server" CssClass="ddl-time">
					<asp:ListItem Text="am" />
					<asp:ListItem Text="pm" />
				</asp:DropDownList>
			</div>
			<div class="row">
				<asp:TextBox ID="txtEndTime" runat="server" CssClass="txt-time" placeholder="HH:mm" MaxLength="5" />
				<asp:DropDownList ID="ddlEndAmPm" runat="server" CssClass="ddl-time">
					<asp:ListItem Text="am" />
					<asp:ListItem Text="pm" />
				</asp:DropDownList>
			</div>
			<div class="row">
				<asp:Button ID="btnConfirmTime" runat="server" CssClass="btn-home" Text="Save" OnClick="saveOrDelete" />
			</div>
			<br />
			<br />
			<asp:Panel ID="pnlDelete" runat="server">
				<h1 style="float: right; text-align: left;">Delete schedule entry for yourself</h1>
				<div class="row">
					<asp:DropDownList ID="ddlScheduledHours" runat="server" CssClass="ddl-time" />
				</div>
				<div class="row">
					<asp:Button ID="btnDeleteTime" runat="server" CssClass="btn-home" Text="Delete" OnClick="saveOrDelete" />
				</div>
			</asp:Panel>
		</div>
	</div>
	<br />
</asp:Content>
