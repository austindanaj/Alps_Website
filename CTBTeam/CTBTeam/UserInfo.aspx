<%@ Page Title="User Info" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" Inherits="CTBTeam.UserInfo" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
   
      <div class="login-page">
        <div class="login">

            <div class="login-form">                             

                
                <asp:Label ID="lblChangePass" runat="server" Text="Change Password" CssClass="username" Visible="False"></asp:Label>
                <br />
                <asp:TextBox ID="txtOldPass" runat="server" placeholder="Old Password" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtNewPass" runat="server" placeholder="New Password" TextMode="Password" Visible="False"></asp:TextBox>
                <asp:TextBox ID="txtConfirmPass" runat="server" placeholder="Confirm Password" TextMode="Password" Visible="False"></asp:TextBox>
                <asp:Button ID="btnSubmit" runat="server" OnClick="Change_Password" Text="Change Password" Visible="False"></asp:Button>
            </div>
        </div>
    </div>
 
    
</asp:Content>
