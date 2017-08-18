<%@ Page Title="Schedule" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TimeOff.aspx.cs" Inherits="CTBTeam.TimeOff" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="row">
		<asp:TextBox ID="txtSuccessBox" runat="server" Text="Success." Visible="false" ReadOnly="true" CssClass="feedback-textbox" />
		<br />
		<div class="col-md-25">
			<asp:BulletedList ID="bulletList" runat="server" CssClass="bullet-list" BulletStyle="Numbered">
				<asp:ListItem>Click a valid start date</asp:ListItem>
				<asp:ListItem>Click an valid end date</asp:ListItem>
				<asp:ListItem>Click Add Time Off to add the dates as time off</asp:ListItem>
			</asp:BulletedList>
			<br />
			<br />
			<br />
			<h2>Remove time off</h2>
			<div style="text-align: center;">
				<asp:DropDownList ID="ddlTimeTakenOff" CssClass="drp-home" runat="server" />
				<br />
				<br />
				<asp:Button ID="btnRemoveTimeOff" runat="server" OnClick="removeTimeOff" Text="Remove Time Off" CssClass="btn btn-primary btn-lg" />
			</div>
		</div>
		<div class="col-md-75">
			<div class="row">
				<div class="col-md-50">
					<h2>Start date</h2>
					<asp:Calendar ID="cldTimeOffStart" runat="server" BackColor="#333333" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="#999999" Height="388px" NextPrevFormat="FullMonth" Width="400px">
						<DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
						<NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#555555" VerticalAlign="Bottom" />
						<OtherMonthDayStyle ForeColor="#555555" />
						<SelectedDayStyle BackColor="#222222" ForeColor="White" />
						<TitleStyle BackColor="#333333" BorderColor="White" BorderWidth="1px" Font-Bold="True" Font-Size="12pt" ForeColor="#999999" />
						<TodayDayStyle BackColor="#111111" />
					</asp:Calendar>
				</div>
				<div class="col-md-50">
					<h2>End Date</h2>
					<asp:Calendar ID="cldTimeOffEnd" runat="server" BackColor="#333333" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="#999999" Height="388px" NextPrevFormat="FullMonth" Width="400px">
						<DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
						<NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#555555" VerticalAlign="Bottom" />
						<OtherMonthDayStyle ForeColor="#555555" />
						<SelectedDayStyle BackColor="#222222" ForeColor="White" />
						<TitleStyle BackColor="#333333" BorderColor="White" BorderWidth="1px" Font-Bold="True" Font-Size="12pt" ForeColor="#999999" />
						<TodayDayStyle BackColor="#111111" />
					</asp:Calendar>
				</div>
			</div>
			<br />
			<div style="text-align: center;">
				<asp:CheckBox ID="chkBusinessTrip" runat="server" Text="This is a business trip" style="color:white" />
				<br />
				<asp:Button ID="btnAddTimeOff" runat="server" OnClick="addTimeOff" Text="Add Time Off" CssClass="btn btn-primary btn-lg" />
			</div>
		</div>
	</div>
	<br />
	<div class="col-md-100">
		<asp:GridView ID="gv" runat="server" CssClass="gridview" Style="width: 1100px;"/>
	</div>
</asp:Content>