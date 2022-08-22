<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GameProc.aspx.cs" Inherits="TestWebForms.GameProc" %>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script src='<%=ResolveClientUrl("~/Scripts/MyScr/Flot.js") %>' type="text/javascript"></script>
<%--    <div style="margin: 12px">
        <h1 id="NameRoom" style="color:crimson; font-style:italic"></h1>
    </div>--%>
    <div style="float: left; margin: 12px">
        <h1>Союзный флот</h1>
        <canvas id="can1" width="600" height="600" style="background-color:#e4fff19e;"></canvas>
    </div>

    <div style="float: left; margin: 12px">
        <h1>Вражеский флот</h1>
        <canvas id="can2" width="600" height="600" style="background-color:#ffefefa8;"></canvas>
    </div>

    <div>
        <button id="Start" type="button"> <%--onclick="SendMyMap()"--%>
            Готов!
        </button>
    </div>
    <!--Ссылка на автоматически сгенерированный скрипт хаба SignalR -->
    <script src="/Scripts/jquery.signalR-2.4.3.min.js"></script>
    <!--Ссылка на автоматически сгенерированный скрипт хаба SignalR -->
    <script src="/signalr/Hubs"></script>
    <script src='<%=ResolveClientUrl("~/Scripts/MyScr/SheepsSR.js") %>'></script>
</asp:Content>
