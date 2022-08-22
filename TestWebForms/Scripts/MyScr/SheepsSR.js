var roomName;
$(function () {
    var _SH = $.connection.roomsHub;
    roomName = (new URL(document.location)).searchParams.get("roomName");

    _SH.client.enemyConnected = function () {
        alert("Соперник подключился!");
    }

    _SH.client.setEnemyIsReady = function (ready){

    }

    $.connection.hub.start().done(function () {
        $('#Create').click(function () {
            var nameNewRoom = $('#Name').val();
            if (nameNewRoom.length > 3 && nameNewRoom.length <= 30) {
                _SH.server.createRoom(nameNewRoom);
            } else {
                alert("Название комнаты должно быть не короче 4 и не длиннее 30 символов!");
            }
        });

        $("#Refrash").click(function () {
            StartSpiner("Tab1");
            _SH.server.getRooms();
        });

        $('#ConnectBtn').click(function () {
            if (isEmpty(CurrentNameRoom)) {
                alert("Выберите комнату из списка или создайте новую комнату!");                
            } else {
                _SH.server.connect(CurrentNameRoom);
            }
        });
    });
    var aaaa = 1;
})