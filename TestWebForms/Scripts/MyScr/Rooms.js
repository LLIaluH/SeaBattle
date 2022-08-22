window.onload = function () {
    var a = $('body');
    a[0].style.backgroundImage = "url(https://zn.ua/img/forall/u/0/-1/users/Dec2015/136472.jpg)";
    a[0].style.backgroundSize = "cover";

    $(document).keydown(function (e) {        
        ArrowMoveSelectionOnGrid(e);
    });
    //GetRooms();
}

var CurrentGrid;
var CurrentNameRoom;

function Connect() {
    if (CurrentNameRoom.length < 1) {
        alert("Выберите комнату из списка!");
        //$('#Name')[0].style = "border-color:red;"
    } else {

        //Старое подключение к комнате
        //StartSpiner("Tab1");
        //var q = JSON.stringify({ NameRoom: CurrentNameRoom });
        //$.ajax({
        //    type: "POST",
        //    url: "/WebService.asmx/ConnectToRoom",
        //    data: q,
        //    contentType: "application/json; charset=utf-8",
        //    dataType: "json",
        //    success: function (jObj) {
        //        var a = jQuery.parseJSON(jObj.d);
                
        //        //CheckError(a);
        //        //GridsUpdate_1(a);
        //        if (a.CanConnectToGame) {
        //            //rederect
        //        } else {
        //            GetRooms();
        //        }
        //        StopSpiner("Tab1");
        //    },
        //    failure: function (response) {
        //        StopSpiner("Tab1");
        //        //alert(response.d);
        //    },
        //    error: function (jqXHR, textStatus, errorThrown) {
        //        StopSpiner("Tab1");
        //    }
        //});
    }
}

function CreateNewRoom() {
    var nameNewRoom = $('#Name')[0].value;
    if (nameNewRoom.length < 1) {
        //alert("Введите название комнаты!");
        $('#Name')[0].style = "border-color:red;"
    } else {
        StartSpiner("Tab1");
        var q = JSON.stringify({ NameRoom: nameNewRoom });
        $.ajax({
            type: "POST",
            url: "/WebService.asmx/CreateNewRoom",
            data: q,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (jObj) {
                var a = jQuery.parseJSON(jObj.d);
                CheckError(a);
                StopSpiner("Tab1");
                GetRooms();
                //FillGrid("#Tab1", json.GridsData, GetDataForChart_1, { layout: "fitDataStretch" });
            },
            failure: function (response) {
                StopSpiner("Tab1");
                //alert(response.d);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                StopSpiner("Tab1");
            }
        });
    }
}

function GetRooms() {
    StartSpiner("Tab1");
    $.ajax({
        type: "POST",
        url: "/WebService.asmx/GetRooms",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (jObj) {
            var a = jQuery.parseJSON(jObj.d);
            CheckError(a);
            StopSpiner("Tab1");                    
            FillGrid("#Tab1", a.GridsData, function (nameGrid, data) {
                CurrentNameRoom = data["Название комнаты"];
            }, { layout: "fitColumns" }); /*layout: "fitDataStretch",*/
        },
        failure: function (response) {
            StopSpiner("Tab1");
            //alert(response.d);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            StopSpiner("Tab1");
        }
    });
}

function ClickRow(nameGrid, data) {
    CurrentNameRoom = data.Name;
}

function deRed(obj) {
    var a = $('#' + obj.id)[0];
    $('#' + obj.id)[0].style = " ";
}

function ArrowMoveSelectionOnGrid(e) {
    var kk = e.keyCode;
    //87 или 38 вверх
    //83 или 40 вниз
    if (CurrentGrid != undefined) {
        if (kk == 87 || kk == 38) {
            var nextRow = CurrentGrid.getSelectedRows()[0].getPrevRow();
        }
        else if (kk == 83 || kk == 40)
        {
            var nextRow = CurrentGrid.getSelectedRows()[0].getNextRow();
        }
        if (nextRow != false && nextRow != undefined) {
            CurrentGrid.deselectRow(CurrentGrid.getSelectedRows()[0])
            CurrentGrid.selectRow(nextRow);
            CurrentGrid.scrollToRow(CurrentGrid.getSelectedRows()[0], "center", true);
        }
    }
}

function SwichCurrentGrid(row) {
    CurrentGrid = row._row.parent.table;
}

function RowSelect1(nameTable, row) {
    var piece_id = row.Id;
}
////////////////////////////////////
