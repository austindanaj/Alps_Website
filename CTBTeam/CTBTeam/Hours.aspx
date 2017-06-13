<%@ Page Title="Hours" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Hours.aspx.cs" Inherits="CTBTeam.Hours" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<style type="text/css">
		body {
			background: url('Gradient.jpg') no-repeat center center fixed;
			background-size: cover;
		}
	</style>
	<div class="row">
		<asp:TextBox ID="successOrFail" runat="server" Text="Success." Visible="false" ReadOnly="true" CssClass="feedback-textbox" />
	</div>
	<div class="row">
		<div class="col-md-50">
			<asp:Label ID="lblWeekOf" runat="server" Text="Week Of: 0/00/0000" CssClass="lbl time-title"></asp:Label>
		</div>
		<div class="col-md-50" style="text-align: left;">
			<asp:CheckBox ID="chkInactive" runat="server" Style="color: white;" Text="Show me inactive projects, old employees, etc." />
			<asp:DropDownList ID="ddlselectWeek" CssClass="drp-home" runat="server" OnSelectedIndexChanged="htmlEvent" />
			<asp:Button ID="btnselectWeek" CssClass="btn-home" runat="server" Text="Go" Width="50px" OnClick="htmlEvent" />
		</div>
	</div>
	<br />
	<div class="row">
		<div class="col-md-25" style="text-align: center;">
			<asp:Label ID="Label3" runat="server" CssClass="lbl-home-title" Text="Delete hours for this week" />
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

			<asp:Label ID="lblTotalHours" runat="server" ForeColor="White" Font-Size="X-Small" Text="Hours: 0/0" />
			<br />
			<br />
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
		</div>
		<div class="col-md-50">
			<div class="row">
				<asp:Chart ID="chartPercent" runat="server" BackColor="Transparent" EnableViewState="true"
					BorderlineWidth="0" Height="360px" Palette="None" PaletteCustomColors="Maroon"
					Width="700px" BorderlineColor="64, 0, 64">
					<Titles>
						<asp:Title ShadowOffset="10" Name="Project Percent" />
					</Titles>
					<Legends>
						<asp:Legend Alignment="Center" Docking="Right" IsTextAutoFit="False" Name="Default"
							BackColor="Transparent" ForeColor="White" LegendStyle="Column" />
					</Legends>
					<Series>
						<asp:Series Name="Default" />
					</Series>
					<ChartAreas>
						<asp:ChartArea Name="ChartArea1" BorderWidth="0" BackColor="Transparent" />
					</ChartAreas>
				</asp:Chart>
			</div>
			<br />
			<div class="row">
				<asp:GridView ID="dgvProject" runat="server" CssClass="gridview" Style="width: 940px;" />
				<%--<asp:Button runat="server" ID="btnProjectPrevious" Text="←" OnClick="Arrow_Button_Clicked" CssClass="btn-home" Width="500px" />
				<asp:Button runat="server" ID="btnProjectNext" Text="→" OnClick="Arrow_Button_Clicked" CssClass="btn-home" Width="500px" />--%>
			</div>
			<br />
			<div class="row">
				<asp:GridView ID="dgvCars" runat="server" CssClass="gridview" Style="width: 940px;" />
				<%-- <asp:Button runat="server" ID="btnVehiclePrevious" Text="←" OnClick="Arrow_Button_Clicked" CssClass="btn-home" Width="500px" />
				<asp:Button runat="server" ID="btnVehicleNext" Text="→" OnClick="Arrow_Button_Clicked" CssClass="btn-home" Width="500px" />--%>
			</div>
		</div>
	</div>
	<br />
</asp:Content>
