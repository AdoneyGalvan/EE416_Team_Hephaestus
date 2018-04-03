﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

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
 <asp:Chart ID="DataPointTimeChart" runat="server" CssClass="chart" BackColor="Black" Height="1000px" Width="1000px">
            <Series>
                <asp:Series Name="DataPointSeriesX" ChartArea="ChartArea1" ChartType="Line" Color="Blue"></asp:Series>
                <asp:Series Name="DataPointSeriesY" ChartArea="ChartArea1" ChartType="Line" Color="Green"></asp:Series>
                <asp:Series Name="DataPointSeriesZ" ChartArea="ChartArea1" ChartType="Line" Color="Red"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1" BackColor="Black">
                    <AxisX Title="Points" TitleFont="Franklin Gothic Medium, 20pt, style=Bold" LineColor="Gold"><MajorGrid LineColor="Gold" /></AxisX>
                    <AxisY Title="G's" TitleFont="Franklin Gothic Medium, 20pt, style=Bold" LineColor="Gold"><MajorGrid LineColor="Gold" /></AxisY>
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Alignment="Near" Docking="Right" BackColor="Black" ForeColor="Gold"/>
            </Legends>
            <Titles>
                <asp:Title Text="G's vs Points" Font="Franklin Gothic Medium, 20pt, style=Bold" BackColor="Black" ForeColor="Gold"/>
            </Titles>
        </asp:Chart>
        </div>
        <div class="container">
 <asp:Chart ID="DataPointFFTChart" runat="server" CssClass="chart" BackColor="Gold" >
            <Series>
                <asp:Series Name="DataPointSeriesX" ChartArea="ChartArea1" ChartType="Line" Color="Blue"></asp:Series>
                <asp:Series Name="DataPointSeriesY" ChartArea="ChartArea1" ChartType="Line" Color="Yellow"></asp:Series>
                <asp:Series Name="DataPointSeriesZ" ChartArea="ChartArea1" ChartType="Line" Color="Red"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1" BackColor="Black">
                    <AxisX Title="Frequency" TitleFont="Franklin Gothic Medium, 20pt, style=Bold"></AxisX>
                    <AxisY Title="Amplitude" TitleFont="Franklin Gothic Medium, 20pt, style=Bold"></AxisY>
                </asp:ChartArea>
            </ChartAreas>
            <Legends>
                <asp:Legend Alignment="Near" Docking="Right"/>
            </Legends>
            <Titles>
                <asp:Title Text="Frequncy Spectrum" Font="Franklin Gothic Medium, 20pt, style=Bold"/>
            </Titles>
        </asp:Chart>
    </div>
</asp:Content>

