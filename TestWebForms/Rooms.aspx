<%@ Page Title="Rooms" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Rooms.aspx.cs" Inherits="TestWebForms.Rooms" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src='<%=ResolveClientUrl("~/Scripts/MyScr/Rooms.js") %>' type="text/javascript"></script>
    <div style="margin:10px;">
        <span style="font-size: 48px;">Доступные игровые комнаты</span>
    </div>
    <div class="shadow" style="width: 90%; height: 850px; margin: auto; background-color: #eeeeeebf; padding: 6px; border-radius: 6px; position:relative">
        <div id="Tab1" style="width:100%; height:800px; background-color: #f0ffff80;"></div>
        <div style="right: 6px; bottom:6px; position:absolute;">
            
            <input id="Name" type="text" placeholder="Название комнаты" onchange="deRed(this)"/>
            <button id="Create" type="button"> <%--onclick="CreateNewRoom()"--%>
                Создать
            </button>
            <button id="Refrash" type="button"> <%--onclick="GetRooms()"--%>
                Обновить
            </button>
            <button id="ConnectBtn" type="button"> <%--onclick="Connect()"--%>
                Подключиться
            </button>
        </div>
    </div>
    <!--Ссылка на автоматически сгенерированный скрипт хаба SignalR -->
    <script src="/Scripts/jquery.signalR-2.4.3.min.js"></script>
    <!--Ссылка на автоматически сгенерированный скрипт хаба SignalR -->
    <script src="/signalr/Hubs"></script>
    <script src='<%=ResolveClientUrl("~/Scripts/MyScr/RoomsSR.js") %>'></script>
</asp:Content>
