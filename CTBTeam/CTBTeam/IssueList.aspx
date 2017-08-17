<%@ Page Title="Purchase List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IssueList.aspx.cs" Inherits="CTBTeam.IssueList" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<style type="text/css">
		body {
			background: url('Images/Gradient.jpg') no-repeat center center fixed;
			background-size: cover;
		}
	</style>

	<asp:TextBox ID="successOrFail" runat="server" Text="Success." Visible="false" ReadOnly="true" CssClass="feedback-textbox" />
	<asp:TextBox ID="txtFail" runat="server" Text="Due date must be after today" Visible="false" ReadOnly="true" CssClass="feedback-textbox" style="width:220px;background-color:orangered"/>

	<asp:Panel ID="pnlHeader" runat="server">
		<asp:LinkButton ID="switchView" runat="server" OnClick="btnSwitchView">Report Issue</asp:LinkButton>
	</asp:Panel>

	<asp:Panel ID="pnlViewIssues" runat="server" Visible="false">
		<asp:GridView ID="dgvViewIssues" runat="server" CssClass="gridview" OnSelectedIndexChanged="selectIssue" AutoGenerateSelectButton="True" />
		<br />
		<div class="col-md-50">
			<asp:Button ID="btnBackward" runat="server" Text="←" CssClass="btn-home" OnClick="viewOtherRows" />
		</div>
		<div class="col-md-50">
			<asp:Button ID="btnForward" runat="server" Text="→" CssClass="btn-home" OnClick="viewOtherRows"/>
		</div>
	</asp:Panel>

	<asp:Panel ID="pnlReportIssue" runat="server" Visible="false">
		<asp:GridView ID="dgvCurrentIssue" runat="server" CssClass="gridview" Visible="false" />
		<div class="col-md-50">
			<asp:Panel ID="pnlAdd" runat="server" Visible="false">
				<asp:Label ID="label5" runat="server" Text="Issue Title" CssClass="lbl-home-title" />
				<br />
				<asp:TextBox ID="txtTitle" runat="server" CssClass="txt-purchase" />
				<br>
				<br />
				<asp:Label ID="label1" runat="server" Text="Category" CssClass="lbl-home-title" />
				<br />
				<asp:DropDownList ID="ddlCategory" runat="server" CssClass="drp-purchase">
					<asp:ListItem Text="1: Inquiry/Request" />
					<asp:ListItem Text="2: Change Request" />
					<asp:ListItem Text="3: Problem" />
					<asp:ListItem Text="4: Memo"/>					
				</asp:DropDownList>
				<br>
				<br>
				<asp:Label ID="label2" runat="server" Text="Project" CssClass="lbl-home-title" />
				<br />
				<asp:DropDownList ID="ddlProject" runat="server" CssClass="drp-purchase" />
				<br>
				<br>
				<asp:Label ID="label4" runat="server" Text="Assign To" CssClass="lbl-home-title" />
				<br />
				<asp:DropDownList ID="ddlAssign" runat="server" CssClass="drp-purchase" />
			</asp:Panel>
			<br />
			<br />
			<asp:Label ID="label3" runat="server" Text="Severity" CssClass="lbl-home-title" />
			<br />
			<asp:DropDownList ID="ddlSeverity" runat="server" CssClass="drp-purchase" >
				<asp:ListItem Text="Minor" />
				<asp:ListItem Text="Major" />
			</asp:DropDownList>
			<br />
			<br />
			<asp:Label ID="label7" runat="server" Text="Description" CssClass="lbl-home-title" />
			<br />
			<asp:TextBox ID="txtDescription" runat="server" CssClass="txt-purchase" Rows="6" TextMode="MultiLine" />
			<br />
			<br />
			<asp:Panel ID="pnlSelectedIssue" runat="server" Visible="false">
				<br />
				<br />
				<asp:Label ID="label15" runat="server" Text="Comment" CssClass="lbl-home-title" />
				<br />
				<asp:TextBox ID="txtComment" runat="server" Text="" CssClass="txt-purchase" Rows="6" TextMode="MultiLine" />
				<br />
				<br />
				<asp:DropDownList ID="ddlStatus" runat="server" CssClass="drp-purchase">
					<asp:ListItem Text="Analysis" />
					<asp:ListItem Text="Completed" />
				</asp:DropDownList>
				<br />
				<br />
			</asp:Panel>
			<asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="submitIssue" CssClass="btn-home" />
		</div>


		<div class="col-md-50">
			<asp:CheckBox ID="dueDate" runat="server" Text="No due Date" Checked="true" ForeColor="White" />
			<br />
			<asp:Calendar ID="cldDueDate" runat="server" BackColor="#333333" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="#999999" Height="388px" NextPrevFormat="FullMonth" Width="400px" OnSelectionChanged="cldUncheckBox">
				<DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
				<NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#555555" VerticalAlign="Bottom" />
				<OtherMonthDayStyle ForeColor="#555555" />
				<SelectedDayStyle BackColor="#222222" ForeColor="White" />
				<TitleStyle BackColor="#333333" BorderColor="White" BorderWidth="1px" Font-Bold="True" Font-Size="12pt" ForeColor="#999999" />
				<TodayDayStyle BackColor="#111111" />
			</asp:Calendar>
		</div>
	</asp:Panel>
</asp:Content>