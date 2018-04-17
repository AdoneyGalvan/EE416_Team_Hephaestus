<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<<<<<<< HEAD
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
=======
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-lg-2 col-md-12 col-sm-12 col-sx-12">
                <div class="SideBar">
                    <div class="row no-gutters">
                        <h5 id="StartDateHeader">Select Start Date</h5>
                    </div>
                    <div class="row no-gutters">
                        <asp:TextBox ID="StartDate" runat="server" CssClass="form-control col-11" Height="25px" BackColor="Black" ForeColor="Gold" BorderStyle="None" BorderColor="Black" ReadOnly="True"></asp:TextBox>
                        <asp:ImageButton ID="ImageStartDate" runat="server" ImageUrl="~/Images/calendar (3).png" CssClass="form-control col-1" Height="30px" OnClick="ImageStartDate_Click" BackColor="Black" BorderStyle="None" BorderColor="Black" ImageAlign="Top"/>
                        </div>
                    <div class="row no-gutters">
                        <asp:Calendar ID="CalendarFrom" CssClass="container" runat="server" BackColor="Black" ForeColor="#FFCC00" DayHeaderStyle-ForeColor="Black" DayHeaderStyle-BackColor="Black" TitleStyle-BackColor="#FFCC00" TitleStyle-ForeColor="Black" SelectedDayStyle-BackColor="#FFCC00" SelectedDayStyle-ForeColor="Black" OnSelectionChanged="CalendarFrom_SelectionChanged"></asp:Calendar>
                    </div>
                    <div class="row no-gutters">
                        <h5 id="EndDateHeader">Select End Date</h5>
                    </div>
                    <div class="row no-gutters">
                        <asp:TextBox ID="EndDate" runat="server" CssClass="form-control col-11" Height="25px" BackColor="Black" ForeColor="Gold" BorderStyle="None" BorderColor="Black" ReadOnly="True"></asp:TextBox>
                        <asp:ImageButton ID="ImageEndDate" runat="server" ImageUrl="~/Images/calendar (3).png" CssClass="form-control col-1" Height="30px" OnClick="ImageEndDate_Click" BackColor="Black" BorderStyle="None" BorderColor="Black" ImageAlign="Top"/>
                    </div>
                    <div class="row no-gutters">
                        <asp:Button ID="SelectDates" runat="server" Text="Select Dates" CssClass="btn btn-warning btn-block" Height="35px" OnClick="SelectDates_Click" />
                    </div>
                    <div class="row no-gutters">
                        <asp:Calendar ID="CalendarTo" CssClass="container" runat="server" BackColor="Black" ForeColor="#FFCC00" DayHeaderStyle-ForeColor="Black" DayHeaderStyle-BackColor="Black" TitleStyle-BackColor="#FFCC00" TitleStyle-ForeColor="Black" SelectedDayStyle-BackColor="#FFCC00" SelectedDayStyle-ForeColor="Black" OnSelectionChanged="CalendarTo_SelectionChanged"></asp:Calendar>
                    </div>
                    <div class="row no-gutters">
                        <h5 id="GroupHeader">Group</h5>
                    </div>
                    <div class="row no-gutters">
                        <asp:DropDownList ID="DropDownList1" runat="server" CssClass="dropdownlist"></asp:DropDownList>
                    </div>
                    <div class="row no-gutters">
                        <asp:Button ID="SelectGroup" runat="server" Text="Select Group" CssClass="btn btn-warning btn-block" Height="35px" OnClick="SelectGroup_Click" />
                    </div>
                    <div class="row no-gutters">
                        <div class="col-4 align-content-center">
                            <asp:CheckBox ID="CheckBoxX" Text="-X-Axis-" runat="server" ForeColor="#FFCC00" CssClass="CheckBox"/>
                        </div>
                        <div class="col-4 align-content-center"">
                            <asp:CheckBox ID="CheckBoxY" Text="-Y-Axis-"  runat="server" ForeColor="#FFCC00" CssClass="CheckBox"/>
                        </div>
                        <div class="col-4 align-content-center"">
                            <asp:CheckBox ID="CheckBoxZ" Text="-Z-Axis-" runat="server" ForeColor="#FFCC00" CssClass="CheckBox"/>
                        </div>
                    </div>
                    <div class="row no-gutters">
                        <asp:Button ID="SelectAxis" runat="server" Text="Select Axis" CssClass="btn btn-warning btn-block" Height="35px" OnClick="SelectAxis_Click" />
                    </div>
                    <div class="row no-gutters">
                        <h5 id="ExportHeader">Enter Filename</h5>
                    </div>
                    <div class="row no-gutters">
                        <asp:TextBox ID="ExportTextBox" CssClass="form-control col-12" Height="25px" BackColor="Gray" ForeColor="Black" runat="server" ></asp:TextBox>
                    </div>
                    <div class="row no-gutters">
                        <asp:Button ID="Export" runat="server" Text="Export To Excel" CssClass="btn btn-warning btn-block" Height="35px" OnClick="Export_Click"/>
                    </div>
                    <div class="row no-gutters">
                        <asp:TextBox ID="AgentTextBox" CssClass="form-control col-12" Height="25px" runat="server" ReadOnly="True"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-lg-10 col-md-12 col-sm-12 col-sx-12">
                <div class="MainContent">
                    <div class="row">
                        <div class="col-12">
                                <asp:Chart ID="DataPointRMSChart" runat="server" CssClass="table  table-bordered table-condensed table-responsive" BackColor="Gray" Height="500px" Width="1500px">
                                    <Series>
                                        <asp:Series Name="X-Axis" ChartArea="ChartArea1" ChartType="Line" Color="#66ff33" ToolTip="RMS: #VALY, Time: #VALX"></asp:Series>
                                        <asp:Series Name="Y-Axis" ChartArea="ChartArea1" ChartType="Line" Color="#0099ff" ToolTip="RMS: #VALY, Time: #VALX"></asp:Series>
                                        <asp:Series Name="Z-Axis" ChartArea="ChartArea1" ChartType="Line" Color="Red" ToolTip="RMS: #VALY, Time: #VALX"></asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1" BackColor="Black">
                                            <AxisX Title="Time" TitleFont="Franklin Gothic Medium, 20pt, style=Bold" TitleForeColor="Gold" LineColor="Gold">
                                            <MinorTickMark LineColor="Gold"/>
                                            <MajorTickMark LineColor="Gold"/>
                                            <MajorGrid LineColor="Gold"/>
                                            <MinorGrid LineColor="Gold"/>
                                            <LabelStyle ForeColor="Gold"/>
                                            </AxisX>
                                            <AxisY Title="RMS" TitleFont="Franklin Gothic Medium, 20pt, style=Bold" TitleForeColor="Gold" LineColor="Gold">
                                            <MinorTickMark LineColor="Gold"/>
                                            <MajorTickMark LineColor="Gold"/>
                                            <MajorGrid LineColor="Gold"/>
                                            <MinorGrid LineColor="Gold"/>
                                            <LabelStyle ForeColor="Gold"/>
                                            </AxisY>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                    <Legends>
                                        <asp:Legend Alignment="Near" Docking="Right" BackColor="Black" TitleForeColor="Gold" ForeColor="Gold"/>
                                    </Legends>
                                    <Titles>
                                        <asp:Title Text="RMS vs Time" Font="Franklin Gothic Medium, 20pt, style=Bold" ForeColor="Gold"/>
                                    </Titles>
                                </asp:Chart>

                        </div>
                        <div class="col-12">
                                <asp:Chart ID="DataPointTimeChart" runat="server" CssClass="table  table-bordered table-condensed table-responsive" BackColor="Gray" Height="500px" Width="1500px">
                                    <Series>
                                        <asp:Series Name="X-Axis" ChartArea="ChartArea1" ChartType="Line" Color="#66ff33" ToolTip="G's: #VALY, Time: #VALX"></asp:Series>
                                        <asp:Series Name="Y-Axis" ChartArea="ChartArea1" ChartType="Line" Color="#0099ff" ToolTip="G's: #VALY, Time: #VALX"></asp:Series>
                                        <asp:Series Name="Z-Axis" ChartArea="ChartArea1" ChartType="Line" Color="Red" ToolTip="G's: #VALY, Time: #VALX"></asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1" BackColor="Black">
                                            <AxisX Title="Time" TitleFont="Franklin Gothic Medium, 20pt, style=Bold" TitleForeColor="Gold" LineColor="Gold">
                                            <MinorTickMark LineColor="Gold"/>
                                            <MajorTickMark LineColor="Gold"/>
                                            <MajorGrid LineColor="Gold"/>
                                            <MinorGrid LineColor="Gold"/>
                                            <LabelStyle ForeColor="Gold"/>
                                            </AxisX>
                                            <AxisY Title="G's" TitleFont="Franklin Gothic Medium, 20pt, style=Bold" TitleForeColor="Gold" LineColor="Gold">
                                            <MinorTickMark LineColor="Gold"/>
                                            <MajorTickMark LineColor="Gold"/>
                                            <MajorGrid LineColor="Gold"/>
                                            <MinorGrid LineColor="Gold"/>
                                            <LabelStyle ForeColor="Gold"/>
                                            </AxisY>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                    <Legends>
                                        <asp:Legend Alignment="Near" Docking="Right" BackColor="Black" TitleForeColor="Gold" ForeColor="Gold"/>
                                    </Legends>
                                    <Titles>
                                        <asp:Title Text="G's vs Time" Font="Franklin Gothic Medium, 20pt, style=Bold" ForeColor="Gold"/>
                                    </Titles>
                                </asp:Chart>

                        </div>
                        <div class="col-12">
                                <asp:Chart ID="DataPointFFTChart" runat="server" CssClass="table  table-bordered table-condensed table-responsive" BackColor="Gray" Height="500px" Width="1500px">
                                    <Series>
                                        <asp:Series Name="X-Axis" ChartArea="ChartArea1" ChartType="Line" Color="#66ff33" ToolTip="Amplitude: #VALY, Frequency: #VALX"></asp:Series>
                                        <asp:Series Name="Y-Axis" ChartArea="ChartArea1" ChartType="Line" Color="#0099ff" ToolTip="Amplitude: #VALY, Frequency: #VALX"></asp:Series>
                                        <asp:Series Name="Z-Axis" ChartArea="ChartArea1" ChartType="Line" Color="Red" ToolTip="Amplitude: #VALY, Frequency: #VALX"></asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1" BackColor="Black">
                                            <AxisX Title="Frequency (Hz)" TitleFont="Franklin Gothic Medium, 20pt, style=Bold" TitleForeColor="Gold" LineColor="Gold">
                                            <MinorTickMark LineColor="Gold"/>
                                            <MajorTickMark LineColor="Gold"/>
                                            <MajorGrid LineColor="Gold"/>
                                            <MinorGrid LineColor="Gold"/>
                                            <LabelStyle ForeColor="Gold"/>
                                            </AxisX>
                                            <AxisY Title="Amplitude" TitleFont="Franklin Gothic Medium, 20pt, style=Bold" TitleForeColor="Gold" LineColor="Gold">
                                            <MinorTickMark LineColor="Gold"/>
                                            <MajorTickMark LineColor="Gold"/>
                                            <MajorGrid LineColor="Gold"/>
                                            <MinorGrid LineColor="Gold"/>
                                            <LabelStyle ForeColor="Gold"/>
                                            </AxisY>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                    <Legends>
                                        <asp:Legend Alignment="Near" Docking="Right" BackColor="Black" TitleForeColor="Gold" ForeColor="Gold"/>
                                    </Legends>
                                    <Titles>
                                        <asp:Title Text="Amplitude vs Frequency (Hz)" Font="Franklin Gothic Medium, 20pt, style=Bold" ForeColor="Gold"/>
                                    </Titles>
                                </asp:Chart>                    
                        </div>
                    </div> 
                </div>
            </div>
        </div>
>>>>>>> 9f0b4938a3dd091a5acf559c933e58839cc23d7a
    </div>
</asp:Content>

