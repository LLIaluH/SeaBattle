<!DOCTYPE html>
<html>
<head>
    <link href="/Content/ChatStyle.css" rel="stylesheet">
    <title></title>
</head>
<body>
    <h2>Чат-комната</h2>
    <div class="main">
        <div id="loginBlock">
            Введите логин:<br />
            <input id="txtUserName" type="text" />
            <input id="btnLogin" type="button" value="Войти" />
        </div>
        <div id="chatBody" hidden>
            <div id="header"></div>
            <div id="inputForm">
                <input type="text" id="message" />
                <input type="button" id="sendmessage" value="Отправить" />
            </div>
            <div id="chatroom"></div>
            <div id="chatusers">
                <b>Все пользователи:</b>
            </div>
        </div>
        <input id="hdId" type="hidden" />
        <input id="username" type="hidden" />
    </div>

<%--    <object id="soundSourceObj">
        <embed id="soundSource" src="/Content/message.mp3" autostart="true"  loop="true"/>
    </object>--%>
    <audio id="soundSource" src="/Content/message.mp3"></audio>

    <script src="/Scripts/jquery-3.4.1.min.js"></script>
    <!--Ссылка на библиотеку SignalR -->
    <script src="/Scripts/jquery.signalR-2.4.3.min.js"></script>
    <!--Ссылка на автоматически сгенерированный скрипт хаба SignalR -->
    <script src="/signalr/Hubs"></script>
    <script src="/Scripts/MyScr/ChatScr.js"></script>
</body>
</html>
