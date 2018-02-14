<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="startdroplist">
        <h1>Start Date</h1>
        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="dropdownlist"></asp:DropDownList>
    </div>
    <div id="enddroplist">
        <h1>End Date</h1>
        <asp:DropDownList ID="DropDownList2" runat="server" CssClass="dropdownlist"></asp:DropDownList>
    </div>
         <asp:Button ID="SubmitButton" runat="server" Text="Submit" CssClass="submitbutton" OnClick="SubmitButton_Click"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <div>
        <asp:GridView ID="GridView1" runat="server" CellPadding="5" CssClass="gridview">
            <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />

        </asp:GridView>
    </div>
</asp:Content>

