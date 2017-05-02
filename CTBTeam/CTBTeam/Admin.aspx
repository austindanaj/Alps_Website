<%@ Page Title="Admin" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="CTBTeam.Admin" %>

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
        <div class="col-md-4">


            <asp:Label ID="lblName" runat="server" Text="Add User" CssClass="username"></asp:Label>
            <br />
            <asp:TextBox ID="txtName" runat="server" placeholder="First and Last Name"></asp:TextBox>

            <asp:Button ID="btnName" runat="server" OnClick="User_Clicked" Text="Add User"></asp:Button>
            <br />
            <asp:CheckBox ID="chkAddToVehcileHours" runat="server" Text="Add to Vehicle Hours" />
            <br />
            <br />
            <asp:Label ID="lblNR" runat="server" Text="Remove User" CssClass="username"></asp:Label>
            <br />
            <asp:TextBox ID="txtNR" runat="server" placeholder="First and Last Name"></asp:TextBox>

            <asp:Button ID="btnNR" runat="server" OnClick="Remove_User_Clicked" Text="Remove User"></asp:Button>
            <br />
            <br />
               <asp:GridView ID="dgvUsers" runat="server" Width="200" HeaderStyle-BackColor="#3AC0F2"
                CssClass="Grid"
                AlternatingRowStyle-CssClass="alt"
                PagerStyle-CssClass="pgr"
                HeaderStyle-ForeColor="White" RowStyle-BackColor="#A1DCF2" AlternatingRowStyle-BackColor="White"
                RowStyle-ForeColor="#3A3A3A" AutoGenerateColumns="true">
            </asp:GridView>
          
        </div>
        <div class="col-md-4">
            <asp:Label ID="lblProject" runat="server" Text="Add Project" CssClass="username"></asp:Label>
            <br />
            <asp:TextBox ID="txtProject" runat="server" placeholder="Project Name"></asp:TextBox>

            <asp:Button ID="btnProject" runat="server" OnClick="Project_Clicked" Text="Add Project"></asp:Button>
            <br />
            <br />
            <br />
            <asp:Label ID="lblPR" runat="server" Text="Remove Project" CssClass="username"></asp:Label>
            <br />
            <asp:TextBox ID="txtPR" runat="server" placeholder="Project Name"></asp:TextBox>

            <asp:Button ID="btnPR" runat="server" OnClick="Remove_Project_Clicked" Text="Remove Project"></asp:Button>
            <br />
            <br />
            <asp:GridView ID="dgvProjects" runat="server" Width="200" HeaderStyle-BackColor="#3AC0F2"
                CssClass="Grid"
                AlternatingRowStyle-CssClass="alt"
                PagerStyle-CssClass="pgr"
                HeaderStyle-ForeColor="White" RowStyle-BackColor="#A1DCF2" AlternatingRowStyle-BackColor="White"
                RowStyle-ForeColor="#3A3A3A" AutoGenerateColumns="true">
            </asp:GridView>
          
        </div>
        <div class="col-md-4">
            <asp:Label ID="lblCar" runat="server" Text="Add Vehicle" CssClass="username"></asp:Label>
            <br />
            <asp:TextBox ID="txtCar" runat="server" placeholder="Vehicle Name"></asp:TextBox>

            <asp:Button ID="btnCar" runat="server" OnClick="Car_Clicked" Text="Add Vehicle"></asp:Button>
            <br />
            <br />
            <br />
            <asp:Label ID="lblCR" runat="server" Text="Remove Vehicle" CssClass="username"></asp:Label>
            <br />
            <asp:TextBox ID="txtCR" runat="server" placeholder="Vehicle Name"></asp:TextBox>

            <asp:Button ID="btnCR" runat="server" OnClick="Remove_Car_Clicked" Text="Remove Vehicle"></asp:Button>
            <br />
            <br />
               <asp:GridView ID="dgvCars" runat="server" Width="200" HeaderStyle-BackColor="#3AC0F2"
                CssClass="Grid"
                AlternatingRowStyle-CssClass="alt"
                PagerStyle-CssClass="pgr"
                HeaderStyle-ForeColor="White" RowStyle-BackColor="#A1DCF2" AlternatingRowStyle-BackColor="White"
                RowStyle-ForeColor="#3A3A3A" AutoGenerateColumns="true">
            </asp:GridView>
         
        </div>

    </div>





</asp:Content>
