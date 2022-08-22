<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestWebForms._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <canvas id="can" width="600" height="600"></canvas>
    <div style="position:absolute; bottom: 6px; background-color:#ff00d91c; border-radius:6px 6px 0 0; padding:26px 6px 6px 6px;">
        <input id="sc1" type="range" min="1" max="100" step="1" value="50" oninput="ChangeScroll()"> 
        <div id="qwe1" style="color:aliceblue; width:200px; height:20px; background-color:darkorchid; border-radius:9px; margin:6px;"></div>
        <input id="sc2" type="range" min="1" max="100" step="1" value="1" oninput="ChangeScroll()"> 
        <div id="qwe2" style="color:aliceblue; width:200px; height:20px; background-color:darkorchid; border-radius:9px; margin:6px;"></div>

        <input id="color1" type="range" min="0" max="255" step="1" value="1" oninput="ChangeColor()" style="float:left; width:150px;"> 
        <input id="checkbox1" type="checkbox" min="1" max="100" step="1" oninput="ChangeColor()" style="float:left; margin:2px !important;"> 
        <br />
        <input id="color2" type="range" min="0" max="255" step="1" value="1" oninput="ChangeColor()" style="float:left; width:150px;"> 
        <input id="checkbox2" type="checkbox" min="1" max="100" step="1" value="1" oninput="ChangeColor()" style="float:left; margin:2px !important;"> 
        <br />
        <input id="color3" type="range" min="0" max="255" step="1" value="1" oninput="ChangeColor()" style="float:left; width:150px;"> 
        <input id="checkbox3" type="checkbox" min="1" max="100" step="1" value="1" oninput="ChangeColor()" style="float:left; margin:2px !important;"> 
        <br />
        <span style="float:left; width: 50px;">Размер точки</span> <input id="size" type="range" min="0" max="10" step="0.1" value="0.3" oninput="ChangeScroll()"> 

        <br />
        <span style="float:left;">Предел светимости</span> <input id="lumen" type="range" min="0" max="1.4" step="0.05" value="0" oninput="ChangeScroll()"> 

        <br />
        <span style="float:left;">Прозрачность</span> <input id="liquid" type="range" min="0" max="1.5" step="0.01" value="0" oninput="ChangeScroll()"> 
        
        <br />
        <span style="float:left;">Диаметр</span> <input id="r1" type="range" min="10" max="120" step="1" value="0" oninput="diaChange()"> 
        
        <br />
        <span style="float:left;">Толщина</span> <input id="r2" type="range" min="5" max="60" step="1" value="0" oninput="diaChange()"> 
    </div>

</asp:Content>
