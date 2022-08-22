$(function () {
    var _SH = $.connection.roomsHub;
    _SH.client.getingRooms = function (jObj) {
        var a = jQuery.parseJSON(jObj);
        CheckError(a);
        StopSpiner("Tab1");
        FillGrid("#Tab1", a.GridsData, function (nameGrid, data) { CurrentNameRoom = data["Название комнаты"];}, { layout: "fitColumns" });
    }

    _SH.client.hasRoom = function () {
        alert("Комната с таким названием уже существует!");
    }

    _SH.client.onConnected = function (type, roomName) {
        window.location.href = '/GameProc?roomName=' + roomName;
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

        StartSpiner("Tab1");
        setTimeout(function () { _SH.server.getRooms(); }, 500);
    });
})