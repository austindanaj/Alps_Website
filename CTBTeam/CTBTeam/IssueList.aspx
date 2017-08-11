<%@ Page Title="Purchase List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IssueList.aspx.cs" Inherits="CTBTeam.IssueList" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<style type="text/css">
		body {
			background: url('Images/Gradient.jpg') no-repeat center center fixed;
			background-size: cover;
		}
	</style>

	<asp:TextBox ID="successOrFail" runat="server" Text="Success." Visible="false" ReadOnly="true" CssClass="feedback-textbox" />
    
    <asp:Panel ID="pnlHeader" runat="server">
        <asp:LinkButton ID="lnkReportIssue" runat="server" OnClick="btnReportIssue_Click">Report Issue</asp:LinkButton>
        <asp:LinkButton ID="lnkViewIssues" runat="server" OnClick="btnViewIssue_Click">View Issues</asp:LinkButton>
    </asp:Panel>
    
    <asp:Panel ID="pnlViewIssues" runat="server">
        <asp:GridView ID="dgvViewIssues" CssClass="gridview" runat="server" OnSelectedIndexChanged="dgvViewIssues_OnSelectedIndexChanged"   autogenerateselectbutton="True"></asp:GridView>

    </asp:Panel>
    
    <asp:Panel ID="pnlReportIssue" runat="server" Visible="False">
        <asp:DropDownList ID="ddlCategory" runat="server"></asp:DropDownList>
        <asp:DropDownList ID="ddlProject" runat="server"></asp:DropDownList>
        <asp:TextBox ID="txtSummary" runat="server"></asp:TextBox>
        <asp:DropDownList ID="ddlSeverity" runat="server"></asp:DropDownList>
        <asp:DropDownList ID="ddlAssign" runat="server"></asp:DropDownList>
        <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtReporter" runat="server" ReadOnly="True"></asp:TextBox>
        <asp:Button ID="btnReportIssue" runat="server" Text="Submit" OnClick="btnReportIssue_OnClick" />
    </asp:Panel>
    
    <asp:Panel ID="pnlSelectedIssue" runat="server" Visible="False">
        
    </asp:Panel>



</asp:Content>
