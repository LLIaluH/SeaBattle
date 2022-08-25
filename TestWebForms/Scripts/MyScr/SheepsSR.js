var roomName;
var userId;

$(function () {
    var _SH = $.connection.sheepsHub;
    roomName = (new URL(document.location)).searchParams.get("roomName");
    userId = (new URL(document.location)).searchParams.get("userId");

    _SH.client.enemyConnected = function (ready) {
        alert("Соперник подключился!");
    }

    _SH.client.setEnemyIsReady = function () {
        $('#readyEnemy').empty();
        $('#readyEnemy').toggleClass('enemyNoReady enemyReady');
        $('#readyEnemy').append('<p>Соперник готов к бою!</p>')
    }

    _SH.client.iAmReady = function (ready) {

    }

    _SH.client.outRoomError = function (message) {
        alert(message);
        window.location.href = '/Rooms';
    }

    _SH.client.sendErrorMap = function (errors) {
        alert(errors);
    }

    $.connection.hub.start().done(function () {
        $('#Start').click(function () {
            var fleet = JSON.stringify( MyShips );
            _SH.server.tryReady(fleet);
        });

        _SH.server.connect(roomName, userId);
    });
})