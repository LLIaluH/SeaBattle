var chat = $.connection.chatHub;
$(function () {
    $('#chatBody').hide();
    $('#loginBlock').show();
    var audio = $('#soundSource')[0];
    //audio.currentTime = 0;
    //audio.play();
    // Ссылка на автоматически-сгенерированный прокси хаба
    //var chat = $.connection.chatHub;
    // Объявление функции, которая хаб вызывает при получении сообщений
    chat.client.addMessage = function (name, message) {
        // Добавление сообщений на веб-страницу 
        //$('#soundSource').Play();

        $('#chatroom').append('<p><b>' + htmlEncode(name)
            + '</b>: ' + htmlEncode(message) + '</p>');
    };

    // Функция, вызываемая при подключении нового пользователя
    chat.client.onConnected = function (id, userName, allUsers) {

        $('#loginBlock').hide();
        $('#chatBody').show();
        // установка в скрытых полях имени и id текущего пользователя
        $('#hdId').val(id);
        $('#username').val(userName);
        $('#header').html('<h3>Добро пожаловать, ' + userName + '</h3>');

        // Добавление всех пользователей
        for (i = 0; i < allUsers.length; i++) {

            AddUser(allUsers[i].ConnectionId, allUsers[i].Name);
        }
    }

    chat.client.playSound = function () {
        audio.play();
        //alert(answer);
    }

    // Добавляем нового пользователя
    chat.client.onNewUserConnected = function (id, name) {

        AddUser(id, name);
    }

    // Удаляем пользователя
    chat.client.onUserDisconnected = function (id, userName) {

        $('#' + id).remove();
    }

    document.addEventListener('keydown', function (event) {
        switch (event.keyCode) {
            case 13: // Space
                sendMessage();
                break;
        }
    });

    // Открываем соединение
    $.connection.hub.start().done(function () {

        $('#sendmessage').click(function () {
            // Вызываем у хаба метод Send
            sendMessage();
        });

        // обработка логина
        $("#btnLogin").click(function () {

            var name = $("#txtUserName").val();
            if (name.length > 0 && name.length <= 30) {
                chat.server.connect(name);
            }
            else {
                alert("Введите имя (оно не должно быть пустым и больше 30 символов)");
            }
        });
    });
});
// Кодирование тегов
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
//Добавление нового пользователя
function AddUser(id, name) {

    var userId = $('#hdId').val();

    if (userId != id) {

        $("#chatusers").append('<p id="' + id + '"><b>' + name + '</b></p>');
    }
}

function sendMessage() {
    // Вызываем у хаба метод Send
    if (isEmpty($('#message').val())) {
        return;
    }
    chat.server.send($('#username').val(), $('#message').val());
    $('#message').val('');

    //chat.server.myTest("Привет");
}

function isEmpty(str) {
    if (str.trim() == '')
        return true;

    return false;
}