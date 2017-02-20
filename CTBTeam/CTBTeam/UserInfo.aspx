<%@ Page Title="User Info" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" Inherits="CTBTeam.UserInfo" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
   
      <div class="login-page">
        <div class="login">

            <div class="login-form">

                <asp:Label ID="lblLogin" runat="server" Text="Sign In" CssClass="username"></asp:Label>
                <br />
                <br />
                <asp:TextBox ID="txtUser" runat="server" placeholder="username"></asp:TextBox>
                <asp:TextBox ID="txtPass" runat="server" placeholder="password" TextMode="Password"></asp:TextBox>
                <asp:Button ID="btnLogin" runat="server" OnClick="Login_Clicked" Text="Sign In"></asp:Button>
                <br />
                <br />
                <br />
                <asp:Label ID="lblRegister" runat="server" Text="Register" CssClass="username" Visible="False"></asp:Label>
                <br />
                <asp:TextBox ID="txtName" runat="server" placeholder="First and Last Name" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtRUser" runat="server" placeholder="Username" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtRPass" runat="server" placeholder="Password" TextMode="Password" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtRConfirm" runat="server" placeholder="Confirm Password" TextMode="Password" Visible="False"></asp:TextBox>
                <asp:Button ID="btnRegister" runat="server" OnClick="Register_Clicked" Text="Register" Visible="False"></asp:Button>
            </div>
        </div>
    </div>
 
    
</asp:Content>
