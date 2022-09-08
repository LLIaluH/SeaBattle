var roomName;
var userId;
var _SH = $.connection.sheepsHub;
$(function () {
    
    roomName = (new URL(document.location)).searchParams.get("roomName");
    userId = (new URL(document.location)).searchParams.get("userId");

    _SH.client.enemyConnected = function (ready) {
        alert("Соперник подключился!");
    }

    _SH.client.startGame = function (step) {
        if (step == 'FirstStep') {
            MyTurn = true;
            $('#readyEnemy').empty();
            $('#readyEnemy').append('<p>Ваш ход!</p>')
        } else {
            $('#readyEnemy').empty();
            $('#readyEnemy').toggleClass('enemyNoReady enemyReady');
            $('#readyEnemy').append('<p>Ход соперника!</p>');
        }
        GameStarted = true;
        InitGame();
        $('#readyMe').remove();
    }

    _SH.client.catchShot = function (where, x, y, type) {
        ShotInCell(where, x, y, type);
    }

    _SH.client.win = function () {
        $('body').toggleClass('win');
        alert('Вы победили');
    }

    _SH.client.loose = function () {
        $('body').toggleClass('loose');
        alert('Вы проиграли');
    }

    _SH.client.swichTurn = function (b) {
        MyTurn = b;
        $('#readyEnemy').empty();
        $('#readyEnemy').toggleClass('enemyNoReady enemyReady');
        if (b) {
            $('#readyEnemy').append('<p>Ваш ход!</p>');
        } else {
            $('#readyEnemy').append('<p>Ход соперника!</p>');
        }
    }

    _SH.client.setEnemyIsReady = function () {
        $('#readyEnemy').empty();
        $('#readyEnemy').toggleClass('enemyNoReady enemyReady');
        $('#readyEnemy').append('<p>Соперник готов к бою!</p>');
    }

    _SH.client.iAmReady = function (ready) {
        $('#readyMe').append('<p>Ваш флот готов!</p>');
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