<%@ Page Title="Hours" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Hours.aspx.cs" Inherits="CTBTeam.Hours" %>

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
	<div class="row">
		<div class="col-md-50">
			<asp:Label ID="lblTitle" runat="server" Text="CTB Time Tracking" CssClass="lbl main-title"></asp:Label>
		</div>
		<div class="col-md-50" style="text-align: left;">
			<br />
			<asp:DropDownList ID="ddlselectWeek" CssClass="drp-home" runat="server" AutoPostBack="true"></asp:DropDownList>
			<asp:Button ID="btnselectWeek" CssClass="btn-home" runat="server" Text="Go" Width="50px" />
		</div>
	</div>
	<div>

		<br />
		<asp:Label ID="lblWeekOf" runat="server" Text="Week Of: 0/00/0000" CssClass="lbl time-title"></asp:Label>
		<asp:Panel ID="Panel1" runat="server" Height="48px" Width="1536px">
			<br />
		</asp:Panel>
	</div>
	<br />
	<br />
	<br />
	<div class="row">
		<div class="col-md-25" style="text-align: center;">

			<asp:Label ID="Label2" runat="server" CssClass="lbl-home-title" Text="Project Percent - Full time" />
			<br />
			<br />
			<asp:DropDownList ID="ddlFullTimeNames" CssClass="drp-home" runat="server" OnSelectedIndexChanged="didSelectNamesPercent" AutoPostBack="true"></asp:DropDownList>
			<br />
			<br />
			<asp:DropDownList ID="ddlAllProjects" CssClass="drp-home" runat="server" OnSelectedIndexChanged="didSelectProjectsPercent" AutoPostBack="true"></asp:DropDownList>
			<br />
			<br />
			<asp:DropDownList ID="ddlPercentage" CssClass="drp-home" runat="server"></asp:DropDownList>
			<br />
			<br />
			<asp:Button ID="Button1" runat="server" OnClick="On_Click_Submit_Percent" Text="Submit" CssClass="btn-home" Text-Align="Center" />

		</div>
		<div class="col-md-50">
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
	</div>
	<br />
	<br />
	<br />
	<div class="row">
		<div class="col-md-25" style="text-align: center;">

			<asp:Label ID="Label1" runat="server" CssClass="lbl-home-title" Text="Project Hours - Terns" />
			<br />
			<br />
			<asp:DropDownList ID="ddlNamesProject" CssClass="drp-home" runat="server" OnSelectedIndexChanged="didSelectNameProject" AutoPostBack="true"></asp:DropDownList>
			<br />
			<br />
			<asp:DropDownList ID="ddlProjects" CssClass="drp-home" runat="server"></asp:DropDownList>
			<br />
			<br />
			<asp:TextBox ID="txtHoursProjects" runat="server" CssClass="txt-home" Rows="1" BorderStyle="Solid" placeholder="0"></asp:TextBox>
			<br />
			<br />
			<asp:Button ID="btnSubmitProject" runat="server" OnClick="On_Click_Submit_Projects" Text="Submit" CssClass="btn-home" Text-Align="Center" />

		</div>
		<div class="col-md-50">
			<asp:GridView ID="dgvProject" runat="server" CssClass="gridview"
				AutoGenerateColumns="true"
				OnPageIndexChanging="OnPageIndexChanging1">
			</asp:GridView>
			<asp:Button runat="server" ID="btnProjectPrevious" Text="←" OnClick="NP_Button_Clicked" CssClass="btn-home" Width="500px" />
			<asp:Button runat="server" ID="btnProjectNext" Text="→" OnClick="NP_Button_Clicked" CssClass="btn-home" Width="500px" />
		</div>
	</div>
	<br />
	<br />
	<br />
	<div class="row">
		<div class="col-md-25" style="text-align: center;">

			<asp:Label ID="Label4" runat="server" CssClass="lbl-home-title" Text="Vehicle Hours - Terns" />
			<br />
			<br />
			<asp:DropDownList ID="ddlNamesCar" CssClass="drp-home" runat="server" OnSelectedIndexChanged="didSelectNameCar" AutoPostBack="true"></asp:DropDownList>
			<br />
			<br />
			<asp:DropDownList ID="ddlCars" CssClass="drp-home" runat="server"></asp:DropDownList>
			<br />
			<br />
			<asp:TextBox ID="txtHoursCars" runat="server" CssClass="txt-home" Rows="1" BorderStyle="Solid" placeholder="0"></asp:TextBox>
			<br />
			<br />
			<asp:Button ID="btnSubmitCar" runat="server" OnClick="On_Click_Submit_Cars" Text="Submit" CssClass="btn-home" Text-Align="Center" />

		</div>


		<div class="col-md-50">

			<asp:GridView ID="dgvCars" runat="server" CssClass="gridview"
				AutoGenerateColumns="true"
				OnPageIndexChanging="OnPageIndexChanging1">
			</asp:GridView>
			<asp:Button runat="server" ID="btnVehiclePrevious" Text="←" OnClick="NP_Button_Clicked" CssClass="btn-home" Width="500px" />
			<asp:Button runat="server" ID="btnVehicleNext" Text="→" OnClick="NP_Button_Clicked" CssClass="btn-home" Width="500px" />
		</div>

	</div>
	<br />
	<br />
	<br />





</asp:Content>
