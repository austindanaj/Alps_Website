<%@ Page Title="Schedule" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TimeOff.aspx.cs" Inherits="CTBTeam.TimeOff" %>

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
	<div class="form-group">

		<br />

		<br />
		<div class="form-gr">
			<asp:BulletedList ID="bulletList" runat="server" CssClass="bullet-list" BulletStyle="Numbered">
				<asp:ListItem>Sign in</asp:ListItem>
				<asp:ListItem>Select a day from the calender</asp:ListItem>
				<asp:ListItem>Click Add Time Off, or Remove Time Off</asp:ListItem>
			</asp:BulletedList>
			<br />
			<div style="text-align: center;">
				<asp:DropDownList ID="ddlNames" CssClass="drp-home" runat="server" OnSelectedIndexChanged="nameChange" AutoPostBack="true" /><br />
				<asp:DropDownList ID="ddlTimeTakenOff" CssClass="drp-home" runat="server" />
				<br />
				<br />
				<asp:Button ID="btnAddTimeOff" runat="server" OnClick="addTimeOff" Text="Add Time Off" CssClass="btn btn-primary btn-lg" />
				<br />
				<br />
				<asp:Button ID="btnRemoveTimeOff" runat="server" OnClick="removeTimeOff" Text="Remove Time Off" CssClass="btn btn-primary btn-lg" />
			</div>
		</div>
		<div class="row">
			<div class="col-md-50">
				<h6>Start date</h6>
				<asp:Calendar ID="cldTimeOffStart" runat="server" OnSelectionChanged="getCurrentDate" BackColor="#333333" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="#999999" Height="388px" NextPrevFormat="FullMonth" Width="593px">
					<DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
					<NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#555555" VerticalAlign="Bottom" />
					<OtherMonthDayStyle ForeColor="#555555" />
					<SelectedDayStyle BackColor="#222222" ForeColor="White" />
					<TitleStyle BackColor="#333333" BorderColor="White" BorderWidth="1px" Font-Bold="True" Font-Size="12pt" ForeColor="#999999"/>
					<TodayDayStyle BackColor="#111111" />
				</asp:Calendar>
			</div>
			<div class="col-md-50">
				<h6>End Date</h6>
				<asp:Calendar ID="cldTimeOffEnd" runat="server" OnSelectionChanged="getCurrentDate" BackColor="#333333" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="#999999" Height="388px" NextPrevFormat="FullMonth" Width="593px">
					<DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
					<NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#555555" VerticalAlign="Bottom" />
					<OtherMonthDayStyle ForeColor="#555555" />
					<SelectedDayStyle BackColor="#222222" ForeColor="White" />
					<TitleStyle BackColor="#333333" BorderColor="White" BorderWidth="1px" Font-Bold="True" Font-Size="12pt" ForeColor="#999999"/>
					<TodayDayStyle BackColor="#111111" />
				</asp:Calendar>
			</div>
			<br />
			<br />
			<div style="text-align: center;">
				<asp:BulletedList ID="bltList" runat="server" BulletStyle="Square" Font-Size="14pt" ForeColor="#FFFFFF"></asp:BulletedList>
			</div>
			<br />
			<br />
		</div>
	</div>
	<div class="col-md-100">
		<asp:Image ID="Image1" runat="server" ImageUrl="~/2017Schedule.PNG" ImageAlign="Middle" />
	</div>


</asp:Content>
