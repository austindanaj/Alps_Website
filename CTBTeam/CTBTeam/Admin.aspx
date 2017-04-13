<%@ Page Title="Admin" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="CTBTeam.Admin" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
   
    <div class="row">
        <div class="col-md-4">


            <asp:Label ID="lblRegister" runat="server" Text="Register" CssClass="username" ></asp:Label>
            <br />
            <asp:TextBox ID="txtName" runat="server" placeholder="First and Last Name"></asp:TextBox>
            <asp:TextBox ID="txtRUser" runat="server" placeholder="Username" ></asp:TextBox>
            <asp:TextBox ID="txtRPass" runat="server" placeholder="Password" TextMode="Password"></asp:TextBox>
            <asp:TextBox ID="txtRConfirm" runat="server" placeholder="Confirm Password" TextMode="Password"></asp:TextBox>
            <asp:Button ID="btnRegister" runat="server" OnClick="Register_Clicked" Text="Register"></asp:Button>
        </div>
        <div class="col-md-4">
            <asp:Label ID="lblProject" runat="server" Text="Project" CssClass="username" ></asp:Label>
            <br />
            <asp:TextBox ID="txtProject" runat="server" placeholder="Project Name" ></asp:TextBox>

            <asp:Button ID="Button1" runat="server" OnClick="Project_Clicked" Text="Register" ></asp:Button>
        </div>
        <div class="col-md-4">
            </div>

    </div>
       <div class="row">
        <div class="col-md-4">
            </div>
           </div>

      
 
    
</asp:Content>
