<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CTBTeam._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Project Hours</h1>
        <p class="lead"> </p>

        <p><asp:Button runat="server" OnClick=" View_More_onClick"
                    Text="Click Here" CssClass="btn btn-primary btn-lg" /></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>CRC Application</h2>
            <p>
                
            </p>
            <p>
                <asp:Button runat="server" OnClick="download_file_crc"
                    Text="Download" CssClass="btn btn-default" />   
            </p>
        </div>
        <div class="col-md-4">
            <h2>Reset Application</h2>
            <p>
                
            </p>
            <p>
                 <asp:Button runat="server" OnClick="download_file_reset"
                    Text="Download" CssClass="btn btn-default" />  
            </p>
        </div>
        <div class="col-md-4">
            <h2>Hex Generator</h2>
            <p>
                
            </p>
            <p>
                <asp:Button runat="server" OnClick="download_file_hexGenerator"
                    Text="Download" CssClass="btn btn-default" />  
            </p>
        </div>
    </div>

</asp:Content>