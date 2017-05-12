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

        <div class="row">
            <div class="col-md-50">

                <asp:Calendar ID="cldTimeOff" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="388px" NextPrevFormat="FullMonth" Width="593px" OnSelectionChanged="Calendar_SelectionChanged">
                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                    <TodayDayStyle BackColor="#CCCCCC" />
                </asp:Calendar>
                <br />
                <br />
                <div style="text-align: left;">
                    <asp:BulletedList ID="bltList" runat="server" BulletStyle="Square" Font-Size="14pt" ForeColor="#1A5276" CssClass="blt-default"></asp:BulletedList>
                </div>
                <br />
                <br />

            </div>
            <div class="col-md-50">
                <div class="form-gr">
                    <asp:BulletedList ID="bulletList" runat="server" CssClass="bullet-list" BulletStyle="Numbered">
                        <asp:ListItem>Sign in</asp:ListItem>
                        <asp:ListItem>Select a day from the calender</asp:ListItem>
                        <asp:ListItem>Click Add Time Off, or Remove Time Off</asp:ListItem>
                    </asp:BulletedList>
                    <br />
                    <div style="text-align: center;">
                        <asp:DropDownList ID="ddlNames" CssClass="drp-home" runat="server"></asp:DropDownList>
                        <br />
                        <br />
                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Add Time Off" CssClass="btn btn-primary btn-lg" />
                        <br />
                        <br />
                        <asp:Button ID="Button3" runat="server" OnClick="Button2_Click" Text="Remove Time Off" CssClass="btn btn-primary btn-lg" />
                    </div>

                </div>
            </div>
        </div>

    </div>
    <div>
        <asp:Image ID="Image1" runat="server" ImageUrl="~/2017Schedule.PNG" ImageAlign="Middle" />
    </div>


</asp:Content>
