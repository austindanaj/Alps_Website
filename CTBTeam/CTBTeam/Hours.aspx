<%@ Page Title="Hours" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Hours.aspx.cs" Inherits="CTBTeam.Hours" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
   
    <div>
        <asp:Label ID="lblTitle" runat="server" Text="CTB Time Tracking" CssClass="lbl main-title"></asp:Label>
    </div>
    <div>
        <asp:Label ID="lblWeekOf" runat="server" Text="Week Of: 1/16/2017" CssClass="lbl time-title"></asp:Label>
        <asp:Panel ID="Panel1" runat="server" Height="48px" Width="1536px">
            <br />
          
        </asp:Panel>
    </div>
    

    <br />
    <br />
    <br />



    <div class="row">

        <div class="col-md-6">
            <asp:GridView ID="dgvProject" runat="server" CssClass="gridview">
            </asp:GridView>

        </div>
    </div>
     <br />
    <br />
  
    <div class="row">
        <div class="col-md-6">
            <asp:GridView ID="dgvCars" runat="server" CssClass="gridview">
            </asp:GridView>
        </div>

    </div>
    <br />
    <br />
    <br />
    <div class="row">
        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>                       
                            <asp:CheckBox ID="chkPB" runat="server" Text="Project B" CssClass="lbl chk-default" OnCheckedChanged="On_Click_PB" Visible="False" AutoPostBack="true" />                     
                            <asp:TextBox ID="txtPB" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />               
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkCar1" runat="server" Text="   Bolt" CssClass="lbl chk-default" OnCheckedChanged="On_Click_Car1" Visible="False" AutoPostBack="true" />
                        <asp:TextBox ID="txtCar1" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkCar2" runat="server" Text="   Tahoe" CssClass="lbl chk-default" OnCheckedChanged="On_Click_Car2" Visible="False" AutoPostBack="true" />
                        <asp:TextBox ID="txtCar2" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkTherm" runat="server" Text="   Thermostat" CssClass="lbl chk-default" OnCheckedChanged="On_Click_Therm" Visible="False" AutoPostBack="true" />
                        <asp:TextBox ID="txtTherm" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkCar3" runat="server" Text="   Volt" CssClass="lbl chk-default" OnCheckedChanged="On_Click_Car3" Visible="False" AutoPostBack="true" />
                        <asp:TextBox ID="txtCar3" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkCar4" runat="server" Text="   EV Spark" CssClass="lbl chk-default" OnCheckedChanged="On_Click_Car4" Visible="False" AutoPostBack="true" />
                        <asp:TextBox ID="txtCar4" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkGA" runat="server" Text="   Global A" CssClass="lbl chk-default" OnCheckedChanged="On_Click_GA" Visible="False" AutoPostBack="true" />
                        <asp:TextBox ID="txtGA" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkCar5" runat="server" Text="   Trax" CssClass="lbl chk-default" OnCheckedChanged="On_Click_Car5" Visible="False" AutoPostBack="true" />
                        <asp:TextBox ID="txtCar5" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>


        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkCar6" runat="server" Text="   Tahoe" CssClass="lbl chk-default" OnCheckedChanged="On_Click_Car6" Visible="False" AutoPostBack="true" />
                        <asp:TextBox ID="txtCar6" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkRadar" runat="server" Text="   Radar" CssClass="lbl chk-default" OnCheckedChanged="On_Click_Radar" Visible="False" AutoPostBack="true" />
                        <asp:TextBox ID="txtRadar" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkCar7" runat="server" Text="   Black XTS" CssClass="lbl chk-default" OnCheckedChanged="On_Click_Car7" Visible="False" AutoPostBack="true" />
                        <asp:TextBox ID="txtCar7" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkCar8" runat="server" Text="   White XTS" CssClass="lbl chk-default" OnCheckedChanged="On_Click_Car8" Visible="False" AutoPostBack="true" />
                        <asp:TextBox ID="txtCar8" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkIR" runat="server" Text="   IR Sensor" CssClass="lbl chk-default" OnCheckedChanged="On_Click_IR" Visible="False" AutoPostBack="true" />
                        <asp:TextBox ID="txtIR" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkCar9" runat="server" Text="   Cruze" CssClass="lbl chk-default" OnCheckedChanged="On_Click_Car9" Visible="False" AutoPostBack="true" />
                        <asp:TextBox ID="txtCar9" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkCar10" runat="server" Text="   Spark" CssClass="lbl chk-default" OnCheckedChanged="On_Click_Car10" Visible="False" AutoPostBack="true" />
                        <asp:TextBox ID="txtCar10" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <div class="form-group">
                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkOther" runat="server" Text="   Other" CssClass="lbl chk-default" OnCheckedChanged="On_Click_Other" Visible="False" AutoPostBack="true" />
                        <asp:TextBox ID="txtOther" runat="server" Text="0" CssClass="txt txt-default" Visible="False" Enabled="False" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
        <div class="col-md-4">
            <div class="form-group">
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
            </div>



        </div>
    </div>
    <div class="row">
        <div class="col-md-4">
            <div class="form-group">
            </div>

        </div>
        <div class="col-md-4">
            <div style="text-align: center">
                <asp:Button ID="btnSubmit" runat="server" OnClick="On_Click_Submit" Text="Submit" CssClass="btn btn-primary btn-lg" Visible="false" Text-Align="Center" />
            </div>
        </div>
        <div class="col-md-4">
            <div class="form-group">
            </div>
        </div>
    </div>
  
                       
    
</asp:Content>
